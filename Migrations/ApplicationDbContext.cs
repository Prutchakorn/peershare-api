using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeerShareV2.Models;

namespace PeerShareV2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Peer> Peers { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<BillSplit> BillSplits { get; set; }
        public DbSet<RTPModel> RTPModels { get; set; }
        public DbSet<PeerRTP> PeerRTPs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
              base.OnModelCreating(builder);
              // Customize the ASP.NET Identity model and override the defaults if needed.
              // For example, you can rename the ASP.NET Identity table names and more.
              // Add your customizations after calling base.OnModelCreating(builder);

              builder.Entity<UserAccount>()
                     .ToTable("UserAccount", schema: "PS");
              builder.Entity<Peer>()
                     .ToTable("Peer", schema: "PS");
              builder.Entity<Status>()
                     .ToTable("Status", schema: "PS");
              builder.Entity<BillSplit>()
                     .ToTable("BillSplit", schema: "PS");
              builder.Entity<PeerRTP>()
                     .ToTable("PeerRTP", schema: "PS");

              builder.Entity<UserAccount>()
                     .HasIndex(p => p.PromptPay).IsUnique();
              builder.Entity<Peer>()
                     .HasIndex(x => new { x.Id, x.BillSplitId });
              builder.Entity<PeerRTP>()
                     .HasKey(x => new { x.PeerId, x.RTPId});
        }
    }
}