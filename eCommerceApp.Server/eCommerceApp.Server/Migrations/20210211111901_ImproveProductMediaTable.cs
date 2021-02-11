using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerceApp.Server.Migrations
{
    public partial class ImproveProductMediaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55585c99-bec8-4757-b514-4641d856d905");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9625fd3a-9355-4090-be76-fcab6224fc95");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "deabca75-bb62-4a07-bd14-38064794df06");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "ProductMedias",
                newName: "FileName");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ProductMedias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b47ccc33-6d6c-4145-b426-80e3f41227e0", "a85cdddb-9c33-4594-88a4-8f7d38fd7a16", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "df4827e6-335c-4fb9-8563-dac1f5e97905", "48aec47c-9e8c-4070-8565-2d4c564bc075", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7b5be521-af9b-4647-8180-18201a7102b3", "49451f9f-59f8-4685-a9fa-430783188317", "NormalUser", "NORMUSER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b5be521-af9b-4647-8180-18201a7102b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b47ccc33-6d6c-4145-b426-80e3f41227e0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df4827e6-335c-4fb9-8563-dac1f5e97905");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProductMedias");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "ProductMedias",
                newName: "Path");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "deabca75-bb62-4a07-bd14-38064794df06", "8508fa55-ca8c-49e8-aa5c-3645cb07a3e2", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9625fd3a-9355-4090-be76-fcab6224fc95", "b0439d7f-9b72-4d73-a886-e47ab98c389d", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "55585c99-bec8-4757-b514-4641d856d905", "bdf8cc7f-a35d-4bc4-82a3-0ccf2177012b", "NormalUser", "NORMUSER" });
        }
    }
}
