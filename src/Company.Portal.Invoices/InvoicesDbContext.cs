using Microsoft.EntityFrameworkCore;

namespace Company.Portal.Invoices;

public sealed class InvoicesDbContext(DbContextOptions<InvoicesDbContext> options) : DbContext(options)
{
    internal DbSet<InvoiceRecord> Invoices => Set<InvoiceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var mapping = modelBuilder.Entity<InvoiceRecord>();
        mapping.ToTable("Invoices");
        mapping.HasKey(record => record.Id);
        mapping.HasIndex(record => record.Subject);
        mapping.Property(record => record.Subject).HasMaxLength(128).IsRequired();
        mapping.Property(record => record.Currency).HasMaxLength(3).IsRequired();
        mapping.Property(record => record.Amount).HasPrecision(18, 2);
    }
}
