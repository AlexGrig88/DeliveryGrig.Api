using DeliveryGrig.Api.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace DeliveryGrig.Api.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
            Console.WriteLine("Hello");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
