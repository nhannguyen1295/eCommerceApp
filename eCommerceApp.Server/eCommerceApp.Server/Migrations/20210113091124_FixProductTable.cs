using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerceApp.Server.Migrations
{
    public partial class FixProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "673a0bdd-10fb-4680-9e01-fd29ac3d3636");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab262330-3e3b-474e-a2ee-6251abc57b46");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de4605a4-3aff-4ae3-91d7-08df41f44e43");

            migrationBuilder.DropColumn(
                name: "ProductStatusId",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7ca389bf-edfe-4350-944a-ee93a667c290", "f0368386-67be-46d3-837b-7f7cccd5cd27", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f2f68f72-9c1c-47b9-8031-805bc6dc245c", "f7e5ad1a-54a0-49c2-99f9-266ce68a8c9e", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ef73e293-0c3d-46b4-befd-856ff18366ed", "8e4339ad-e124-454a-8c39-35eae6098206", "NormalUser", "NORMUSER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ca389bf-edfe-4350-944a-ee93a667c290");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef73e293-0c3d-46b4-befd-856ff18366ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2f68f72-9c1c-47b9-8031-805bc6dc245c");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductStatusId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "673a0bdd-10fb-4680-9e01-fd29ac3d3636", "41dbf1d7-628d-4df0-b05a-84ac48e606f6", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "de4605a4-3aff-4ae3-91d7-08df41f44e43", "f5f93ce5-acb6-4d0c-aab1-487ee84e9324", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ab262330-3e3b-474e-a2ee-6251abc57b46", "348955a2-249e-4555-bbcb-99c0d9aa4d7f", "NormalUser", "NORMUSER" });
        }
    }
}
