using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerceApp.Server.Migrations
{
    public partial class ImproveProductMediaTableV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "ProductMedias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5f5e86d0-a225-4c8d-9ddc-26c3db9c98fe", "3c8b37f1-fa39-4713-9048-48ff5a0dd50a", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bd30e79a-0268-4293-b206-392529f71b46", "8ee1c09f-6759-4dae-a71c-eaf23e923ff6", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6f7cccbe-6bed-4dcb-9606-ef97e1d6b008", "1cddf7cb-8276-40e1-8769-861e7cc83ff7", "NormalUser", "NORMUSER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f5e86d0-a225-4c8d-9ddc-26c3db9c98fe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f7cccbe-6bed-4dcb-9606-ef97e1d6b008");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd30e79a-0268-4293-b206-392529f71b46");

            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "ProductMedias");

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
    }
}
