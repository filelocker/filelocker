using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Filelocker.DataAccess;

namespace Filelocker.Api.Migrations
{
    [DbContext(typeof(EfUnitOfWork))]
    [Migration("20161015054126_FilelockerMigration")]
    partial class FilelockerMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("Filelocker.Domain.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Filelocker.Domain.FilelockerFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ApplicationUserId");

                    b.Property<string>("EncryptionKey");

                    b.Property<Guid>("EncryptionSalt");

                    b.Property<string>("Name");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Filelocker.Domain.FilelockerFile", b =>
                {
                    b.HasOne("Filelocker.Domain.ApplicationUser")
                        .WithMany("Files")
                        .HasForeignKey("ApplicationUserId");
                });
        }
    }
}
