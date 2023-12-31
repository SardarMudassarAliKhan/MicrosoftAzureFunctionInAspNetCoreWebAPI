using Microsoft.EntityFrameworkCore;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.Model;

namespace MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.ApplicationDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FetchedDataModel> FetchedData { get; set; }
    }
}
