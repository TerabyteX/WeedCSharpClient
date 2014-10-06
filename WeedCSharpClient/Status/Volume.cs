using System;

namespace WeedCSharpClient.Status
{
    public class Volume
    {
        public int Id;
        public long Size;
        public string RepType;
        public string Collection;
        public string Version;
        public long FileCount;
        public long DeleteCount;
        public long DeletedByteCount;
        public bool ReadOnly;

        public ReplicationStrategy GetReplicationStrategy()
        {
            ReplicationStrategy replication;
            Enum.TryParse(RepType, out replication);

            return replication;
        }
    }
}
