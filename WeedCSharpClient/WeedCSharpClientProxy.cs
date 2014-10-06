using System;
using System.Configuration;
using System.IO;

using WeedCSharpClient.Net;

namespace WeedCSharpClient
{
    public interface IWeedCSharpClientSubject 
    {
        WriteResult Upload(byte[] buffer, string fileName = null, string fid = null, 
            ReplicationStrategy replicationStrategy = ReplicationStrategy.None);
        WriteResult Upload(Stream stream, string fileName = null, string fid = null, 
            ReplicationStrategy replicationStrategy = ReplicationStrategy.None);
        void Delete(string fid);
        string Lookup(long volumeId);
    }

    //Singleton
    internal sealed class WeedCSharpClientSubject : IWeedCSharpClientSubject
    {
        private WeedCSharpClientSubject() { }
        public static readonly WeedCSharpClientSubject Instance = new WeedCSharpClientSubject();

        private readonly WeedCSharpClientImpl weedCSharpClient = new WeedCSharpClientImpl(new Uri(ConfigurationManager.AppSettings["WeedMasterUrl"]));

        /// <summary>
        /// store or update the file content with byte array
        /// </summary>
        /// <param name="buffer">byte array</param>
        /// <param name="fileName">file name</param>
        /// <param name="fid">fid</param>
        /// <param name="replicationStrategy">replication strategy</param>
        /// <returns>Write Result</returns>
        public WriteResult Upload(byte[] buffer, string fileName = null, string fid = null, 
            ReplicationStrategy replicationStrategy = ReplicationStrategy.None)
        {
            var assignResult = weedCSharpClient.Assign(new AssignParams(replicationStrategy));
            return weedCSharpClient.Write(assignResult.WeedFSFile, assignResult.Location, buffer, fileName);
        }
        /// <summary>
        /// store or update the file content with stream
        /// </summary>
        /// <param name="stream">stream</param>
        /// <param name="fileName">file name</param>
        /// <param name="fid">fid</param>
        /// <param name="replicationStrategy">replication strategy</param>
        /// <returns>Write Result</returns>
        public WriteResult Upload(Stream stream, string fileName = null, string fid = null, 
            ReplicationStrategy replicationStrategy = ReplicationStrategy.None)
        {
            var assignResult = weedCSharpClient.Assign(new AssignParams(replicationStrategy));
            return weedCSharpClient.Write(assignResult.WeedFSFile, assignResult.Location, stream, fileName);
        }

        /// <summary>
        /// delete the file
        /// </summary>
        /// <param name="url">url</param>
        public void Delete(string url)
        {
            weedCSharpClient.Delete(url);
        }

        /// <summary>
        /// lookup the file
        /// </summary>
        /// <param name="volumeId">volume id</param>
        /// <returns>url</returns>
        public string Lookup(long volumeId)
        {
            var locations = weedCSharpClient.Lookup(volumeId);
            if (locations.Count > 0)
            {
                return locations[0].publicUrl;
            }
            else
            {
                throw new ArgumentException("There is no location", "locations");
            }
        }
    }

    //Proxy
    public class WeedCSharpClientProxy : IWeedCSharpClientSubject 
    {
        public WriteResult Upload(byte[] buffer, string fileName = null, string fid = null, 
            ReplicationStrategy replicationStrategy = ReplicationStrategy.None)
        {
            return WeedCSharpClientSubject.Instance.Upload(buffer, fileName, fid, replicationStrategy);
        }

        public WriteResult Upload(Stream stream, string fileName = null, string fid = null, 
            ReplicationStrategy replicationStrategy = ReplicationStrategy.None)
        {
            return WeedCSharpClientSubject.Instance.Upload(stream, fileName, fid, replicationStrategy);
        }

        public void Delete(string fid)
        {
            WeedCSharpClientSubject.Instance.Delete(fid);
        }

        public string Lookup(long volumeId)
        {
            return WeedCSharpClientSubject.Instance.Lookup(volumeId);
        }
    }
}
