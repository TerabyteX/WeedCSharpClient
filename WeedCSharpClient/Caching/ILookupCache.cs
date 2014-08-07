using System.Collections.Generic;

namespace WeedCSharpClient.Caching
{
    public interface ILookupCache
    {
        List<Location> Lookup(long volumeId);

        void Invalidate(long volumeId);

        void Invalidate();

        void SetLocation(long volumeId, List<Location> locations);
    }
}
