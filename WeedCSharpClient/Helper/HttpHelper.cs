using System;
using System.IO;
using System.Net;
using System.Text;

namespace WeedCSharpClient.Helper
{
    public static class HttpHelper
    {
        /// <summary>
        /// Post multipart form data
        /// </summary>
        public static string MultipartPost(string url, byte[] buffer, string name, string fileName, string contentType, bool async = true)
        {
            var boundaryStr = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            var bytes = Encoding.ASCII.GetBytes("\r\n--" + boundaryStr + "\r\n");

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundaryStr;
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

            var requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            var format = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            var fileInfo = string.Format(format, name, fileName, contentType);

            var fileInfoBuffer = Encoding.UTF8.GetBytes(fileInfo);
            requestStream.Write(fileInfoBuffer, 0, fileInfoBuffer.Length);
            requestStream.Write(buffer, 0, buffer.Length);

            var boundaryBuffer = Encoding.ASCII.GetBytes("\r\n--" + boundaryStr + "--\r\n");
            requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            requestStream.Close();

            WebResponse webResponse = null;
            StreamReader streamReader = null;
            string result = null;

            try
            {
                if (async)
                {
                    var asyncResult = httpWebRequest.BeginGetResponse(null, null);
                    webResponse = httpWebRequest.EndGetResponse(asyncResult);
                }
                else
                {
                    webResponse = httpWebRequest.GetResponse();
                }

                streamReader = new StreamReader(webResponse.GetResponseStream());
                result = streamReader.ReadToEnd();
            }
            finally 
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }

                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }

            return result;
        }
    }
}
