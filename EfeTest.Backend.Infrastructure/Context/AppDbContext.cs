using EfeTest.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace EfeTest.Backend.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            //relacion one to many order and user
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //relacion one to many order and items
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            //relacion one to one
            modelBuilder.Entity<Product>()
                .HasMany(o => o.OrderItems)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1 ,Name = "JBL Audifonos Bluetooth T510BT Wireless Pure Bass", Price = 149 },
                new Product { Id = 2 ,Name = "Audifonos Bluetooth Lenovo HE05", Price = 24.90 },
                new Product { Id = 3 ,Name = "Auriculares Inalámbricos WH-CH520", Price = 129 },
                new Product { Id = 4 ,Name = "Audífonos AirPods 2nda Generación", Price = 499 },
                new Product { Id = 5 ,Name = "Audífonos Skullcandy Dime True wireless", Price = 99 }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
    }   
}
