﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using YGate.DataAccess.Postgresql.EFCore;

#nullable disable

namespace YGate.DataAccess.Postgresql.EFCore.Migrations
{
    [DbContext(typeof(PostgreSQLContext))]
    [Migration("20250225172342_init30")]
    partial class init30
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("YGate.Entities.BasedModel.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ShowIndex")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.AccountPasswords", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AccountsPasswords");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.AccountProperties", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("PropertiesName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PropertiesValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Verified")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("AccountProperties");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.AccountRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FromGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RoleGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ToGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ValidityDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("AccountRoles");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LongDescription")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.CategoryHtmlTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CategoryHtmlTemplates");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.CategoryRoles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("RoleGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CategoryRoles");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.CategoryTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Require")
                        .HasColumnType("boolean");

                    b.Property<bool>("Seo")
                        .HasColumnType("boolean");

                    b.Property<string>("ValidateRegex")
                        .HasColumnType("text");

                    b.Property<int>("ValueType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("CategoryTemplates");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.CategoryTemplateValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryTemplateGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.Property<string>("ValueGroupGuid")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CategoryTemplateValues");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.Entitie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryDBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("ParentEntitieDBGuid")
                        .HasColumnType("text");

                    b.Property<DateTime>("SharedDateUTC")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Entities");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.EntitieOwnerTransfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTimeUTC")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EntitieGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NewOwnerGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OldOwnerGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EntitieOwner");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.EntitiePropertyValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryTemplateGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EntitieDbGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("PropertyDBGuid")
                        .HasColumnType("text");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PropertyValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EntitiePropertyValues");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.MeasurementCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LongDescription")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MeasurementCategories");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.MeasurementUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MeasurementCategoryGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MeasurementUnits");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.PropertyGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PropertyGroups");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.PropertyGroupValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("PropertyGroupGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PropertyGroupValues");
                });

            modelBuilder.Entity("YGate.Entities.BasedModel.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DBGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LongDescription")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
