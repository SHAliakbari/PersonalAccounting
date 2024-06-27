using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace PersonalAccounting.Domain.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<TransferRequest> TransferRequests { get; set; }
        public DbSet<TransferRequestDetail> TransferRequestDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TransferRequestDetail>()
                .HasOne(d => d.TransferRequest)
                .WithMany(m => m.Details)
                .HasForeignKey(d => d.TransferRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
