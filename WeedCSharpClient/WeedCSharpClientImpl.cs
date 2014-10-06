using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

using WeedCSharpClient.Caching;
using WeedCSharpClient.Helper;
using WeedCSharpClient.Net;
using WeedCSharpClient.Status;

using ServiceStack.Text;

namespace WeedCSharpClient
{
    public class WeedCSharpClientImpl : IWeedCSharpClient
    {
        private readonly Uri _masterUri;
        private readonly HttpClient _httpClient = new HttpClient();
        private static readonly ILookupCache LookupCache = new MapLookupCache();

        public WeedCSharpClientImpl(Uri masterUri)
        {
            _masterUri = masterUri;
        }

        #region Implement IWeedCSharpClient

        public Assignation Assign(AssignParams assignParams)
        {
            var url = new StringBuilder(new Uri(_masterUri, "/dir/assign").AbsoluteUri);
            url.Append("?count=").Append(assignParams.VersionCount);

            if (assignParams.ReplicationStrategy != null)
            {
                url.Append("&replication=");
                url.AppendFormat("{0:D3}", (int)assignParams.ReplicationStrategy);
            }

            if (!string.IsNullOrWhiteSpace(assignParams.Collection))
            {
                url.Append("&collection=").Append(assignParams.Collection);
            }

            using (var response = _httpClient.GetAsync(url.ToString()))
            {
                var content = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<AssignResult>(content);
                
                if (!string.IsNullOrWhiteSpace(result.error))
                {
                    throw new WeedFSException(result.error);
                }

                return new Assignation(result);
            }
        }

        public WriteResult Write(WeedFSFile file, Location location, FileInfo fileToUpload)
        {
            if (fileToUpload.Length == 0)
            {
                throw new WeedFSException("Cannot write a 0-length file");
            }

            return Write(file, location, fileToUpload.Name, fileToUpload);
        }

        public WriteResult Write(WeedFSFile file, Location location, byte[] dataToUpload, string fileName)
        {
            if (dataToUpload.Length == 0)
            {
                throw new WeedFSException("Cannot write a 0-length data");
            }
            return Write(file, location, fileName, null, dataToUpload);
        }

        public WriteResult Write(WeedFSFile file, Location location, Stream inputToUpload, string fileName)
        {
            return Write(file, location, fileName, null, null, inputToUpload);
        }

        public void Delete(string url)
        {
            using (var response = _httpClient.DeleteAsync(url))
            {
                var result = response.Result;
                var statusCode = result.StatusCode;

                if (statusCode < HttpStatusCode.OK || statusCode > HttpStatusCode.PartialContent)
                {
                    var index = url.LastIndexOf('/') + 1;
                    var fid = url.Substring(index);
                    throw new WeedFSException(string.Format("Error deleting file {0} on {1}: {2} {3}",
                        fid, url, statusCode, result.ReasonPhrase));
                }
            }
        }

        public List<Location> Lookup(long volumeId)
        {
            if (LookupCache != null)
            {
                List<Location> locations = LookupCache.Lookup(volumeId);
                if (locations != null)
                {
                    return locations;
                }
            }

            var url = new StringBuilder(new Uri(_masterUri, "/dir/lookup").AbsoluteUri);
            url.Append("?volumeId=").Append(volumeId);

            using (var response = _httpClient.GetAsync(url.ToString()))
            {
                var content = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<LookupResult>(content);       

                if (!string.IsNullOrWhiteSpace(result.error))
                {
                    throw new WeedFSException(result.error);
                }

                if (LookupCache != null)
                {
                    LookupCache.SetLocation(volumeId, result.locations);
                }

                return result.locations;
            }
        }

        public Stream Read(WeedFSFile file, Location location)
        {
            var url = new StringBuilder();
            if (!location.publicUrl.Contains("http"))
            {
                url.Append("http://");
            }
            url.AppendFormat("{0}/{1}", location.publicUrl, file.Fid);

            if (file.Version > 0)
            {
                url.Append('_').Append(file.Version);
            }

            var cts = new CancellationTokenSource();
            using (var response = _httpClient.GetAsync(url.ToString(), cts.Token))
            {
                var result = response.Result;
                var statusCode = result.StatusCode;

                if (statusCode == HttpStatusCode.NotFound)
                {
                    cts.Cancel();

                    throw new WeedFSFileNotFoundException(file, location);
                }

                if (statusCode != HttpStatusCode.OK)
                {
                    if (!response.IsCanceled)
                        cts.Cancel();

                    throw new WeedFSException(string.Format("Error reading file {0} on {1}: {2} {3}",
                        file.Fid, location.publicUrl, statusCode, result.ReasonPhrase));
                }

                return response.Result.Content.ReadAsStreamAsync().Result;
            }
        }

        public MasterStatus GetMasterStatus()
        {
            var url = new Uri(_masterUri, "/dir/status").AbsoluteUri;

            using (var response = _httpClient.GetAsync(url))
            {
                var result = response.Result;
                var statusCode = result.StatusCode;

                if (statusCode != HttpStatusCode.OK)
                {
                    throw new IOException("Not 200 status recieved for master status url: " + url);
                }

                var content = result.Content.ReadAsStringAsync().Result;
                return JsonSerializer.DeserializeFromString<MasterStatus>(content);
            }
        }

        public VolumeStatus GetVolumeStatus(Location location)
        {
            var url = new StringBuilder();
            if (!location.publicUrl.Contains("http"))
            {
                url.Append("http://");
            }
            url.Append(location.publicUrl).Append("/status");

            var urlStr = url.ToString();
            using (var response = _httpClient.GetAsync(urlStr))
            {
                var result = response.Result;
                var statusCode = result.StatusCode;
                if (statusCode != HttpStatusCode.OK)
                {
                    throw new IOException("Not 200 status recieved for master status url: " + urlStr);
                }

                var content = result.Content.ReadAsStringAsync().Result;
                return JsonSerializer.DeserializeFromString<VolumeStatus>(content);
            }
        }

        #endregion

        private string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return "file";
            }
            else if (fileName.Length > 256)
            {
                return fileName.Substring(0, 255);
            }

            return fileName;
        }

        private WriteResult Write(WeedFSFile file, Location location, string fileName = null, FileInfo fileToUpload = null,
            byte[] dataToUpload = null, Stream inputToUpload = null)
        {
            var url = new StringBuilder();
            if (!location.publicUrl.Contains("http"))
            {
                url.Append("http://");
            }
            url.AppendFormat("{0}/{1}", location.publicUrl, file.Fid);

            if (file.Version > 0)
            {
                url.Append('_').Append(file.Version);
            }

            byte[] buffer;
            if (fileToUpload != null)
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = fileToUpload.Name;
                }

                var stream = fileToUpload.OpenRead();
                buffer = StreamHelper.StreamToBytes(stream);
            }
            else if (dataToUpload != null)
            {
                buffer = dataToUpload;
            }
            else
            {
               buffer = StreamHelper.StreamToBytes(inputToUpload); 
            }

            var multipart = new MultipartFormDataContent();
            multipart.Add(new ByteArrayContent(buffer)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/octet-stream"),
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    }
                }, "file", SanitizeFileName(fileName));

            var fileUrl = url.ToString();
            using (var response = _httpClient.PostAsync(fileUrl, multipart))
            {
                var content = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<WriteResult>(content);
                result.url = fileUrl;

                if (!string.IsNullOrWhiteSpace(result.error))
                {
                    throw new WeedFSException(result.error);
                }

                return result;
            }
        }
    }
}
