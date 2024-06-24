using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SolutionName.Application.Enums;
using SolutionName.Application.Utilities.Helpers;
using SolutionName.Domain.Entities;
using SolutionName.Domain.Entities.Common;
using SolutionName.Domain.Entities.Identity;
using SolutionName.Persistence.Extensions;

namespace SolutionName.Persistence.Contexts
{    
    public class EFDbContext : IdentityDbContext<UserEntity, RoleEntity, int, UserClaimEntity, UserRoleEntity, IdentityUserLogin<int>, RoleClaimEntity, UserTokenEntity>
    {
        public EFDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }

     
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderEntity>()
                .HasKey(b => b.Id);

            builder.Entity<OrderEntity>()
                .HasIndex(o => o.OrderCode)
                .IsUnique();

       
            builder.ApplyGlobalFilters<IEntity>(e => e.IsDeleted == false);
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// SaveChanges Interceptor
        /// </summary>
        /// <param name="cancellationToken"></param>      
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //ChangeTracker : It is the property that enables the capture of changes made on entities or newly added data. It allows us to capture and obtain the data tracked in Update operations.

            var currentUserId = IdentityHelper.GetUserId();
            ChangeTracker.DetectChanges();

            var added = ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Added)
                .Select(t => t.Entity);

            foreach (var entity in added)
            {
                if (entity is IAuditableEntity)
                {
                    var track = entity as IAuditableEntity;

                    try
                    {
                        if (track.CreatedUserId == 0)
                            track.CreatedUserId = currentUserId != 0
                                ? currentUserId
                                : 1;

                        // track.CreateDate default datetime not equal
                        if (track.CreatedDate == null || track.CreatedDate == DateTime.MinValue)
                            track.CreatedDate = DateTime.Now;
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException(HttpResponseStatus.Exception.ToString());
                    }
                }
            }

            var modified = ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Modified)
                .Select(t => t.Entity);

            foreach (var entity in modified)
            {
                if (entity is IAuditableEntity)
                {
                    var track = entity as IAuditableEntity;
                    track.UpdatedDate = DateTime.Now;
                    try
                    {
                        if (track.UpdatedUserId == 0)
                            track.UpdatedUserId = currentUserId != 0
                                ? currentUserId
                                : 1;
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException(HttpResponseStatus.Exception.ToString());
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
