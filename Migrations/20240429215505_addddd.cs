using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace caiobadev_api_arqtool.Migrations
{
    public partial class addddd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dataNascimento",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dataNascimento",
                table: "AspNetUsers");
        }
    }
}
