namespace WeedCSharpClient
{
    public class AssignParams
    {
        public readonly ReplicationStrategy? ReplicationStrategy;
        public readonly int VersionCount;
        public readonly string Collection;
        public static readonly AssignParams Default = new AssignParams();

        public AssignParams(ReplicationStrategy? replicationStrategy)
            : this(null, 1, replicationStrategy) { }

        public AssignParams(string collection, ReplicationStrategy? replicationStrategy) 
            : this(collection, 1, replicationStrategy) { }

        public AssignParams(string collection = null, int versionCount = 1,
            ReplicationStrategy? replicationStrategy = null)
        {
            Collection = collection;
            VersionCount = versionCount;
            ReplicationStrategy = replicationStrategy;
        }
    }
}
