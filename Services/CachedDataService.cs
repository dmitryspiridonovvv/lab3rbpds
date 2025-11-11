using FitnessCenterLab3.Data;
using FitnessCenterLab3.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FitnessCenterLab3.Services
{
    public class CachedDataService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;

        public CachedDataService(ApplicationDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public List<Client> GetClients()
        {
            if (!_cache.TryGetValue("clients", out List<Client>? clients))
            {
                clients = _db.Clients.Take(20).ToList();

                _cache.Set("clients", clients, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
            return clients!;
        }
    }
}
