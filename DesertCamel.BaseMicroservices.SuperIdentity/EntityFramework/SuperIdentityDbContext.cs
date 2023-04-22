using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework
{
    public class SuperIdentityDbContext : DbContext
    {
        public SuperIdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserPoolEntity> UserPools { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<ResourceEntity> Resources { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }
        public DbSet<RoleResourceEntity> RoleResources { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserAttributeEntity> UserAttributes { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<ClientAuthorityEntity> ClientAuthorities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntity>()
                .HasIndex(role => role.Name)
                .IsUnique();

            modelBuilder.Entity<ResourceEntity>()
                .HasIndex(resource => resource.Name)
                .IsUnique();

            modelBuilder.Entity<PermissionEntity>()
                .HasIndex(permission => permission.Name)
                .IsUnique();

            modelBuilder.Entity<RoleResourceEntity>()
                .HasOne(roleResource => roleResource.Role)
                .WithMany(role => role.RoleResources)
                .HasForeignKey(roleResource => roleResource.RoleName)
                .HasPrincipalKey(role => role.Name);

            modelBuilder.Entity<RoleResourceEntity>()
                .HasOne(roleResource => roleResource.Resource)
                .WithMany(resource => resource.ResourceRoles)
                .HasForeignKey(roleResource => roleResource.ResourceName)
                .HasPrincipalKey(resource => resource.Name);

            modelBuilder.Entity<RolePermissionEntity>()
                .HasOne(rolePermission => rolePermission.Role)
                .WithMany(role => role.RolePermissions)
                .HasForeignKey(rolePermission => rolePermission.RoleName)
                .HasPrincipalKey(role => role.Name);

            modelBuilder.Entity<RolePermissionEntity>()
                .HasOne(rolePermission => rolePermission.Permission)
                .WithMany(permission => permission.PermissionRoles)
                .HasForeignKey(rolePermission => rolePermission.PermissionName)
                .HasPrincipalKey(permission => permission.Name);

            modelBuilder.Entity<UserPoolVectorEntity>()
                .HasOne(userPoolVector => userPoolVector.UserPool)
                .WithMany(userPool => userPool.UserPoolVectors)
                .HasForeignKey(userPoolVector => userPoolVector.UserPoolId);

            modelBuilder.Entity<UserEntity>()
                .HasOne(user => user.UserPool)
                .WithMany(userPool => userPool.Users)
                .HasForeignKey(user => user.UserPoolId);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(user => user.PrincipalName)
                .IsUnique();

            modelBuilder.Entity<UserAttributeEntity>()
                .HasOne(userAttribute => userAttribute.User)
                .WithMany(user => user.UserAttributes)
                .HasForeignKey(userAttribute => userAttribute.UserId);

            modelBuilder.Entity<ClientEntity>()
                .HasIndex(client => client.ClientName)
                .IsUnique();

            modelBuilder.Entity<ClientAuthorityEntity>()
                .HasOne(clientAuthority => clientAuthority.Client)
                .WithMany(client => client.ClientAuthorities)
                .HasForeignKey(clientAuthority => clientAuthority.ClientName)
                .HasPrincipalKey(client => client.ClientName);

            modelBuilder.Entity<ClientAuthorityEntity>()
                .HasOne(clientAuthority => clientAuthority.RoleResource)
                .WithMany(roleResource => roleResource.ClientAuthorities)
                .HasForeignKey(clientAuthority => clientAuthority.RoleResourceId);
        }
    }
}
