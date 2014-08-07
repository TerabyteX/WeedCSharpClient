using System.Collections.Generic;
using System.IO;

using WeedCSharpClient.Net;
using WeedCSharpClient.Status;

namespace WeedCSharpClient
{
    /// <summary>
    /// Note: fileName that exceeds 256 characters will be truncated.
    /// </summary>
    public interface IWeedCSharpClient
    {
        Assignation Assign(AssignParams assignParams);

        WriteResult Write(WeedFSFile weedFSFile, Location location, FileInfo file);

        WriteResult Write(WeedFSFile file, Location location, byte[] dataToUpload, string fileName);

        WriteResult Write(WeedFSFile file, Location location, Stream inputToUpload, string fileName);

        void Delete(string url);

        List<Location> Lookup(long volumeId);

        Stream Read(WeedFSFile file, Location location);

        MasterStatus GetMasterStatus();

        VolumeStatus GetVolumeStatus(Location location);
    }
}
