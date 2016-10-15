using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Filelocker.Api.Migrations
{
    public partial class FilelockerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EncryptionKey",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EncryptionSalt",
                table: "Files",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptionKey",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "EncryptionSalt",
                table: "Files");
        }
    }
}
