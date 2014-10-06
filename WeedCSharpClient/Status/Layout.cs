using System;
using System.Collections.Generic;

namespace WeedCSharpClient.Status
{
    public class Layout
    {
        public string Collection;
        public string Replication;
        public List<int> Writables;

        public ReplicationStrategy GetReplicationStrategy()
        {
            ReplicationStrategy replication;
            Enum.TryParse(Replication, out replication);

            return replication;
        }
    }
}
