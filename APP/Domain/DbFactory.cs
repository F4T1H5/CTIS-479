using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace APP.Domain
{
    public class DbFactory : IDesignTimeDbContextFactory<Db>
    {
        const string CONNECTIONSTRING = "data source=ProductsDB";

        public Db CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Db>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new Db(optionsBuilder.Options);
        }
    }
}