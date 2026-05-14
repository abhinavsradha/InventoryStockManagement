using InventoryStockManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryStockManagement.Data;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

    public DbSet<ProductVariantOption> ProductVariantOptions => Set<ProductVariantOption>();

    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.Id);
            entity.HasIndex(product => product.ProductCode).IsUnique();
            entity.Property(product => product.TotalStock).HasColumnType("decimal(18,2)");
            entity.Property(product => product.ProductCode).HasMaxLength(50);
            entity.Property(product => product.ProductName).HasMaxLength(200);
            entity.Property(product => product.HSNCode).HasMaxLength(100);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(variant => variant.Id);
            entity.Property(variant => variant.Name).HasMaxLength(100);
            entity.HasOne(variant => variant.Product)
                .WithMany(product => product.Variants)
                .HasForeignKey(variant => variant.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductVariantOption>(entity =>
        {
            entity.HasKey(option => option.Id);
            entity.Property(option => option.Value).HasMaxLength(100);
            entity.HasOne(option => option.ProductVariant)
                .WithMany(variant => variant.Options)
                .HasForeignKey(option => option.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.HasKey(transaction => transaction.Id);
            entity.Property(transaction => transaction.Quantity).HasColumnType("decimal(18,2)");
            entity.Property(transaction => transaction.Notes).HasMaxLength(250);
            entity.HasOne(transaction => transaction.Product)
                .WithMany(product => product.StockTransactions)
                .HasForeignKey(transaction => transaction.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
