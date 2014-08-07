using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WeedCSharpClient.Caching
{
    public class MapLookupCache : ILookupCache
    {
        private ConcurrentDictionary<long, List<Location>> _cache = new ConcurrentDictionary<long, List<Location>>();

        #region Implement ILookupCache

        public void Invalidate()
        {
            _cache.Clear();
        }

        public void Invalidate(long volumeId)
        {
            List<Location> locations;
            _cache.TryRemove(volumeId, out locations);
        }

        public List<Location> Lookup(long volumeId)
        {
            List<Location> locations;
            _cache.TryGetValue(volumeId, out locations);
            return locations;
        }

        public void SetLocation(long volumeId, List<Location> locations)
        {
            _cache.TryAdd(volumeId, locations);
        }

        #endregion
    }
}
