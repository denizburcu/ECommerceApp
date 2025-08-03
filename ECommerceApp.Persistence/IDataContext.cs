using ECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Persistence
{
    public interface IDataContext 
    {
        DbSet<ProductEntity> Products { get; set; }

    }
}