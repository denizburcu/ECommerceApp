using Microsoft.EntityFrameworkCore;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Persistence.Contexts
{
    public class PostgreDataContext : DbContext, IDataContext
    {
        public PostgreDataContext(DbContextOptions<PostgreDataContext> options) : base(options)
        {   
        }

        public DbSet<ProductEntity> Products { get; set; }
    }
}