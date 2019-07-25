using System.Reflection;
using Data.Common.Contracts.Attributes;
using Data.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Common.Services
{
    public class BaseDbContext : DbContext//<TContext> where TContext : DbContext
    {
        public BaseDbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetIndexAttribute(modelBuilder);
            SetUserRoleManyToMany(modelBuilder);
        }

        private void SetUserRoleManyToMany(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(userRole => new { userRole.UserId, userRole.RoleId });

//            modelBuilder.Entity<UserRole>()
//                .HasOne(userRole => userRole.User)
//                .WithMany(user => user.UserRoles)
//                .HasForeignKey(userRole => userRole.UserId);
//
//            modelBuilder.Entity<UserRole>()
//                .HasOne(userRole => userRole.Role)
//                .WithMany(role => role.UserRoles)
//                .HasForeignKey(userRole => userRole.RoleId);
        }

        private void SetIndexAttribute(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entity.GetProperties())
                {
                    var attr = prop.PropertyInfo.GetCustomAttribute<IndexAttribute>();
                    if (attr != null)
                    {
                        IMutableIndex index = entity.AddIndex(prop);
                        index.IsUnique = attr.IsUnique;
                        index.SqlServer().IsClustered = attr.IsClustered;
                    }
                }
            }
        }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);
//
//            Configuration.ProxyCreationEnabled = false;
//                        modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
//                        modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
//        }
    }
}