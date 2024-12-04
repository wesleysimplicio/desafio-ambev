using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;

namespace Sales.Data.Contexts
{
    public class SalesContext : DbContext
    {
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ItemSale> ItensSale { get; set; }

        public SalesContext(DbContextOptions<SalesContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relacionamento de Sale e ItemSale
            modelBuilder.Entity<Sale>()
                .HasMany(v => v.Itens)
                .WithOne(iv => iv.Sale)
                .HasForeignKey(iv => iv.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar relacionamento de Client e Sale
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Sales)
                .WithOne(v => v.Client)
                .HasForeignKey(v => v.ClientId);

            // Configurar relacionamento de Branch e Sale
            modelBuilder.Entity<Branch>()
                .HasMany(f => f.Sales)
                .WithOne(v => v.Branch)
                .HasForeignKey(v => v.BranchId);

            // Configurar relacionamento de Product e ItemSale
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ItensSale)
                .WithOne(iv => iv.Product)
                .HasForeignKey(iv => iv.ProductId);
        }
    }
}
