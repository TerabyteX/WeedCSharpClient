namespace WeedCSharpClient
{
    [System.Flags]
    public enum ReplicationStrategy
    {
        /// <summary>
        /// no replication, just one copy
        /// </summary>
        None = 0,
        /// <summary>
        /// replicate once on the same rack
        /// </summary>
        OnceOnSameRack = 1,
        /// <summary>
        /// replicate twice on the same rack
        /// </summary>
        TwiceOnSameRack = 2,
        /// <summary>
        /// replicate once on a different rack in the same data center
        /// </summary>
        OnceOnDifferentRack = 10,
        /// <summary>
        /// replicate twice on a different rack in the same data center
        /// </summary>
        TwiceOnDifferentRack = 20,
        /// <summary>
        /// replicate once on a different data center
        /// </summary>
        OnceOnDifferentDC = 100,
        /// <summary>
        /// replicate twice on two other different data center
        /// </summary>
        TwiceOnDifferentDC = 200,
        /// <summary>
        /// replicate once on a different rack, once on a different data center
        /// </summary>
        OnceOnDifferentRackAndOnceOnDifferentDC = 110,
        /// <summary>
        /// replicate once on the same rack, once on a different rack, once on a different data center
        /// </summary>
        OnceOnSameRackAndOnceOnDifferentRackAndOnceOnDifferentDC = 111
    }
}
