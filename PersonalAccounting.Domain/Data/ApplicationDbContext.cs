using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace PersonalAccounting.Domain.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        public DbSet<TransferRequest> TransferRequests { get; set; }
        public DbSet<TransferRequestDetail> TransferRequestDetails { get; set; }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            return base.DisposeAsync();
        }

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

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite("Data Source=dbfile.db");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
