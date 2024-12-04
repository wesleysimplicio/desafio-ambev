using Microsoft.EntityFrameworkCore;
using Sales.Data.Contexts;
using Sales.Domain.Entities;

namespace Sales.Data.Seed
{
    public class DataSeeder
    {
        public static async Task SeedClientsAsync(SalesContext context)
        {
            if (!await context.Clients.AnyAsync())
            {
                var clients = new List<Client>
            {
                new Client
                {
                    NameClient = "Client  1",
                    Email = "client1@example.com",
                    Phone = "123456789"
                },
                new Client
                {
                    NameClient = "Client  2",
                    Email = "client2@example.com",
                    Phone = "987654321"
                }
            };

                await context.Clients.AddRangeAsync(clients);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedBranchesAsync(SalesContext context)
        {
            if (!await context.Branches.AnyAsync())
            {
                var branch = new Branch
                {
                    NameBranch = " Branch1",
                    State = "Estadual",
                    City = "Cityla",
                    Address = "Somewhere"
                };

                await context.Branches.AddAsync(branch);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedProductsAsync(SalesContext context)
        {
            if (!await context.Products.AnyAsync())
            {
                var product = new Product
                {
                    NameProduct = "Tst Product1",
                    PriceUnit = 14.5m,
                };

                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
            }
        }
    }
}
