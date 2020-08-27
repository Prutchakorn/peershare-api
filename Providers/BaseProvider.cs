using PeerShareV2.Data;

namespace PeerShareV2.Providers
{
    public class BaseProvider
    {
        protected readonly ApplicationDbContext _db;

        public BaseProvider(ApplicationDbContext db)
        {
           _db = db;
        }
    }
}