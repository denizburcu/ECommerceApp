namespace ECommerceApp.Persistence.Contexts
{
    using Microsoft.EntityFrameworkCore;
    
    public class PostgreDataContext : DbContext, IDataContext
    {
        public PostgreDataContext(DbContextOptions<PostgreDataContext> options) : base(options)
        {   
        }
        
    }
}
