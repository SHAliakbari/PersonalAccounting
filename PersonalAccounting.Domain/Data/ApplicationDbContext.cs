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

        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }
        public DbSet<ReceiptItemShare> ReceiptItemShares { get; set; }

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

            modelBuilder.Entity<ReceiptItem>()
                .HasOne(d => d.Receipt)
                .WithMany(m => m.Items)
                .HasForeignKey(d => d.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReceiptItemShare>()
                .HasOne(d => d.ReceiptItem)
                .WithMany(m => m.Shares)
                .HasForeignKey(d => d.ReceiptItemId)
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
