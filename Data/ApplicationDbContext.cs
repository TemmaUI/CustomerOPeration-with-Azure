using AzureCustomerOPeration.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureCustomerOPeration.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<LeadEntity> Leads { get; set; }
    }
}
