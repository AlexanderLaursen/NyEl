using Common.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DbSet<BillingModel> BillingModels { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<ConsumerInvoicePreference> ConsumerInvoicePreferences { get; set; }
        public DbSet<ConsumptionReading> ConsumptionReadings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoicePreference> InvoicePreferences { get; set; }
        public DbSet<PriceInfo> PriceInfos { get; set; }
        public DbSet<FixedPriceInfo> FixedPriceInfos { get; set; }
        public DbSet<InvoicePeriodData> InvoicePeriodDatas { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Consumer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ConsumerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.BillingModel)
                .WithMany()
                .HasForeignKey(i => i.BillingModelId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BillingModel>()
                .HasIndex(b => b.BillingModelType)
                .IsUnique();

            modelBuilder.Entity<InvoicePreference>()
                .HasIndex(ip => ip.InvoicePreferenceType)
                .IsUnique();
        }
    }
}
