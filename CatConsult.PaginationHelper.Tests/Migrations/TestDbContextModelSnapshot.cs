﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using CatConsult.PaginationHelper.Tests.Helpers;

#nullable disable

namespace CatConsult.PaginationHelper.Tests.Migrations
{
    [DbContext(typeof(TestDbContext))]
    partial class TestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CatConsult.PaginationHelper.Tests.Helpers.TestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("Enum")
                        .HasColumnType("integer");

                    b.Property<decimal?>("Number")
                        .HasColumnType("numeric");

                    b.Property<string>("String")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TestEntities");
                });

            modelBuilder.Entity("CatConsult.PaginationHelper.Tests.Helpers.TestNestedEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("EntityId")
                        .HasColumnType("integer");

                    b.Property<string>("String")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.ToTable("TestNestedEntity");
                });

            modelBuilder.Entity("CatConsult.PaginationHelper.Tests.Helpers.TestNestedEntity", b =>
                {
                    b.HasOne("PaginationHelper.Tests.Helpers.TestEntity", "Entity")
                        .WithMany("List")
                        .HasForeignKey("EntityId");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("CatConsult.PaginationHelper.Tests.Helpers.TestEntity", b =>
                {
                    b.Navigation("List");
                });
#pragma warning restore 612, 618
        }
    }
}
