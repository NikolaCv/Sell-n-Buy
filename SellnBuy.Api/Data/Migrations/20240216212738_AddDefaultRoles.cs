using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SellnBuy.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6f99e991-36ae-43a6-a74f-47aec0f10f6d", null, "User", "USER" },
                    { "8b8111ff-938f-4a31-a6e9-9f160b43dbf8", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6f99e991-36ae-43a6-a74f-47aec0f10f6d");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "8b8111ff-938f-4a31-a6e9-9f160b43dbf8");
        }
    }
}
