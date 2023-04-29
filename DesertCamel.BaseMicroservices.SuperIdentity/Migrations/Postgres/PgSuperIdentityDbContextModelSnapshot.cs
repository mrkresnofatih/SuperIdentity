﻿// <auto-generated />
using System;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DesertCamel.BaseMicroservices.SuperIdentity.Migrations.Postgres
{
    [DbContext(typeof(PgSuperIdentityDbContext))]
    partial class PgSuperIdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.ClientAuthorityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RoleResourceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClientName");

                    b.HasIndex("RoleResourceId");

                    b.ToTable("ClientAuthorities");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.ClientEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("ClientName")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.PermissionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.ResourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RoleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RolePermissionEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("PermissionName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("PermissionName");

                    b.HasIndex("RoleName");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RoleResourceEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ResourceName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("ResourceName");

                    b.HasIndex("RoleName");

                    b.ToTable("RoleResources");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserAttributeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserAttributes");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserAuthorityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("PrincipalName")
                        .IsRequired()
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RoleResourceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PrincipalName");

                    b.HasIndex("RoleResourceId");

                    b.ToTable("UserAuthorities");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("PrincipalName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("UserPoolId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PrincipalName")
                        .IsUnique();

                    b.HasIndex("UserPoolId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserPoolEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ApplicationCallbackUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("ExchangeTokenUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IssuerUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("JwksUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LoginPageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PrincipalNameKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RedirectUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("TokenLifeTime")
                        .HasColumnType("bigint");

                    b.Property<bool>("UseCache")
                        .HasColumnType("boolean");

                    b.Property<string>("UserInfoUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserPools");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserPoolVectorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DestinationKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SourceKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserPoolId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserPoolId");

                    b.ToTable("UserPoolVectorEntity");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.ClientAuthorityEntity", b =>
                {
                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.ClientEntity", "Client")
                        .WithMany("ClientAuthorities")
                        .HasForeignKey("ClientName")
                        .HasPrincipalKey("ClientName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RoleResourceEntity", "RoleResource")
                        .WithMany("ClientAuthorities")
                        .HasForeignKey("RoleResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("RoleResource");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RolePermissionEntity", b =>
                {
                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.PermissionEntity", "Permission")
                        .WithMany("PermissionRoles")
                        .HasForeignKey("PermissionName")
                        .HasPrincipalKey("Name")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RoleEntity", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleName")
                        .HasPrincipalKey("Name")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RoleResourceEntity", b =>
                {
                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.ResourceEntity", "Resource")
                        .WithMany("ResourceRoles")
                        .HasForeignKey("ResourceName")
                        .HasPrincipalKey("Name")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RoleEntity", "Role")
                        .WithMany("RoleResources")
                        .HasForeignKey("RoleName")
                        .HasPrincipalKey("Name")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resource");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserAttributeEntity", b =>
                {
                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserEntity", "User")
                        .WithMany("UserAttributes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserAuthorityEntity", b =>
                {
                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserEntity", "User")
                        .WithMany("UserAuthorities")
                        .HasForeignKey("PrincipalName")
                        .HasPrincipalKey("PrincipalName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RoleResourceEntity", "RoleResource")
                        .WithMany("UserAuthorities")
                        .HasForeignKey("RoleResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleResource");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserEntity", b =>
                {
                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserPoolEntity", "UserPool")
                        .WithMany("Users")
                        .HasForeignKey("UserPoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserPool");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserPoolVectorEntity", b =>
                {
                    b.HasOne("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserPoolEntity", "UserPool")
                        .WithMany("UserPoolVectors")
                        .HasForeignKey("UserPoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserPool");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.ClientEntity", b =>
                {
                    b.Navigation("ClientAuthorities");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.PermissionEntity", b =>
                {
                    b.Navigation("PermissionRoles");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.ResourceEntity", b =>
                {
                    b.Navigation("ResourceRoles");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RoleEntity", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("RoleResources");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.RoleResourceEntity", b =>
                {
                    b.Navigation("ClientAuthorities");

                    b.Navigation("UserAuthorities");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserEntity", b =>
                {
                    b.Navigation("UserAttributes");

                    b.Navigation("UserAuthorities");
                });

            modelBuilder.Entity("DesertCamel.BaseMicroservices.SuperIdentity.Entity.UserPoolEntity", b =>
                {
                    b.Navigation("UserPoolVectors");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
