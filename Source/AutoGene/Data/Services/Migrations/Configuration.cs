using Microsoft.Azure.Mobile.Server.Tables;
using System.Data.Entity.Migrations;

namespace Data.Services.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<CommonDbContext>
    {
        public Configuration()
        {
            ContextKey = "Data.Services.CommonDbContext";
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }

        protected override void Seed(CommonDbContext context)
        {
            CommonDbInitializer.SeedInitialData(context);
        }
    }
}
