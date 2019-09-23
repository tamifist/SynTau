using Data.Common.Contracts.Entities;
using Data.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Data.Common.Services
{
    public class BaseDbContextSeed
    {
        private ModelBuilder modelBuilder;

        public virtual void Seed(ModelBuilder builder)
        {
            modelBuilder = builder;

            SeedRoles();
        }

        private void SeedRoles()
        {
            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = "1", Name = "Guest"}, 
                new Role() { Id = "2", Name = "Admin" }
            );
        }
    }
}
