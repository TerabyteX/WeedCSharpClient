using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace WeedCSharpClient.Caching
{
    public class MapLookupCache : ILookupCache
    {
        private ConcurrentDictionary<long, List<Location>> _cache = new ConcurrentDictionary<long, List<Location>>();
        private readonly int _cacheNum;

        public MapLookupCache()
        {
            var cacheNum = ConfigurationManager.AppSettings["CacheNum"];

            _cacheNum = cacheNum != null ?
                int.Parse(cacheNum) : 10000;
        }

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
            if (_cache.Count() > _cacheNum)
            {
                var id = _cache.First().Key;
                Invalidate(id);
            }

            _cache.TryAdd(volumeId, locations);
        }

        #endregion
    }
}
