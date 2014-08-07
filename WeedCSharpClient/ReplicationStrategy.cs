namespace WeedCSharpClient
{
    [System.Flags]
    public enum ReplicationStrategy
    {
        /// <summary>
        /// no replication
        /// </summary>
        None = 0,
        /// <summary>
        /// replicate once on the same rack
        /// </summary>
        OnceOnSameRack = 1,
        /// <summary>
        /// replicate once on a different rack, but same data center
        /// </summary>
        OnceOnDifferentRack = 10,
        /// <summary>
        /// replicate once on a different data center
        /// </summary>
        OnceOnDifferentDC = 100,
        /// <summary>
        /// replicate twice on two different data center
        /// </summary>
        TwiceOnDifferentDC = 200,
        /// <summary>
        /// replicate once on a different rack, and once on a different data center
        /// </summary>
        OnceOnDifferentRackAndOnceOnDifferentDC = 110
    }
}
