using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerceApp.Server.Migrations
{
    public partial class FixRelativeOfCategoryWithAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedias_Products_ProductId",
                table: "ProductMedias");

            migrationBuilder.DropTable(
                name: "CategoryAttributeValues");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AttributeValues_PrincipalKey",
                table: "AttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMedias",
                table: "ProductMedias");

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
                name: "PrincipalKey",
                table: "AttributeValues");

            migrationBuilder.RenameTable(
                name: "ProductMedias",
                newName: "ProductMedium");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMedias_ProductId",
                table: "ProductMedium",
                newName: "IX_ProductMedium_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMedium",
                table: "ProductMedium",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CategoryAttributes",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAttributes", x => new { x.CategoryId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_CategoryAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryAttributes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2333c8f8-9aae-45b4-8fea-70cafd9f662b", "29981c10-3b38-45be-ba62-35495f4b6299", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d504afc6-eead-4a53-a8f4-01f032f68814", "7bc0039a-bf64-4b21-a4c2-2dcbcc04d2c2", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1d1afdd9-1d5a-43ea-945a-1eaeff543e54", "b9b8e209-50b4-4d0e-8ab4-1a64b332c1db", "NormalUser", "NORMUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAttributes_AttributeId",
                table: "CategoryAttributes",
                column: "AttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedium_Products_ProductId",
                table: "ProductMedium",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedium_Products_ProductId",
                table: "ProductMedium");

            migrationBuilder.DropTable(
                name: "CategoryAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMedium",
                table: "ProductMedium");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d1afdd9-1d5a-43ea-945a-1eaeff543e54");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2333c8f8-9aae-45b4-8fea-70cafd9f662b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d504afc6-eead-4a53-a8f4-01f032f68814");

            migrationBuilder.RenameTable(
                name: "ProductMedium",
                newName: "ProductMedias");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMedium_ProductId",
                table: "ProductMedias",
                newName: "IX_ProductMedias_ProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "PrincipalKey",
                table: "AttributeValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AttributeValues_PrincipalKey",
                table: "AttributeValues",
                column: "PrincipalKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMedias",
                table: "ProductMedias",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CategoryAttributeValues",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAttributeValues", x => new { x.CategoryId, x.AttributeValueId });
                    table.ForeignKey(
                        name: "FK_CategoryAttributeValues_AttributeValues_AttributeValueId",
                        column: x => x.AttributeValueId,
                        principalTable: "AttributeValues",
                        principalColumn: "PrincipalKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryAttributeValues_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAttributeValues_AttributeValueId",
                table: "CategoryAttributeValues",
                column: "AttributeValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedias_Products_ProductId",
                table: "ProductMedias",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
