using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeerShareV2.Data;
using PeerShareV2.Models;
using PeerShareV2.Providers;

namespace PeerShareV2.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AWSProvider _AWSProvider;
        protected readonly ISecurityProvider _securityProvider;
        protected readonly ApplicationDbContext _db;
        protected readonly DbSet<Status> _statuses;

       public BaseController() {}
       public BaseController(ApplicationDbContext db)
       {
           _AWSProvider = new AWSProvider(db);
           _db = db;
           _statuses = _db.Statuses;
       }
       public BaseController(ApplicationDbContext db, ISecurityProvider securityProvider)
       {
           _AWSProvider = new AWSProvider(db);
           _db = db;
           _securityProvider = securityProvider;
           _statuses = _db.Statuses;
       }
    }
}
