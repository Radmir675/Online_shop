using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShop.db.Migrations
{
    public partial class ManyImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItem_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_Image_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Cost", "Name", "ShortDescription" },
                values: new object[,]
                {
                    { new Guid("96d166a4-0b6b-494f-ab37-03144b0e9ecb"), 850m, "Dagestan Sulak Canyon", "Visiting beautiful places in mountainous Dagestan. See the flight of an eagle." },
                    { new Guid("490b7dba-960b-4c22-b79e-0405d8d56ccd"), 2500m, "Elbrus mountain 5 642 м.", "Feel the whole world at your feet!. Prove to yourself that you can do more than you think." },
                    { new Guid("e36a90de-320b-416e-8363-f7388dbb4796"), 1000m, "Altai \"Golden Mountains\"", "Find yourself and feel life. Nature could health everybody. Just do it!" },
                    { new Guid("fca2c833-87c2-4c9d-9c17-ec6d1cc4e5af"), 500m, "Baikal\" Horizonless lake\"", "Relax on the cleanest and freshest lake. Be free!" },
                    { new Guid("a7049ef9-f027-4a69-970d-06d7dec5f8ce"), 630m, "Murmansk Сapital of the arctic", "Feel power of the ice! You can see real northern lights and polar bears!" },
                    { new Guid("0615471f-1269-4622-9812-b162a4097dd2"), 1100m, "Rep. Tatarstan “Grey Ural” ", "Visit caves, rivers, the Urals. Try the most valuable honey!" }
                });

            migrationBuilder.InsertData(
                table: "Image",
                columns: new[] { "ImageId", "Link", "ProductId" },
                values: new object[,]
                {
                    { new Guid("f42006be-b346-4ff0-8de5-56ee31ab48e0"), "/lib/images/Dagestan.jpeg", new Guid("96d166a4-0b6b-494f-ab37-03144b0e9ecb") },
                    { new Guid("f0a25af7-e2bf-4893-9bb9-48cf9c9f48dd"), "/lib/images/Elbrus.jpg", new Guid("490b7dba-960b-4c22-b79e-0405d8d56ccd") },
                    { new Guid("cd44db5a-3de6-44bf-9756-5dd99d4fe463"), "/lib/images/Altai.jpg", new Guid("e36a90de-320b-416e-8363-f7388dbb4796") },
                    { new Guid("218924a1-e5b0-4e33-848a-d710a080afd1"), "/lib/images/Baikal.jpg", new Guid("fca2c833-87c2-4c9d-9c17-ec6d1cc4e5af") },
                    { new Guid("e54565ef-f1c2-42f9-8ad7-8863afce27be"), "/lib/Images/Murmansk.jpg", new Guid("a7049ef9-f027-4a69-970d-06d7dec5f8ce") },
                    { new Guid("a3a368bb-0d4c-47d1-ae80-1e8623e113ca"), "/lib/Images/Ural.jpg", new Guid("0615471f-1269-4622-9812-b162a4097dd2") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_OrderId",
                table: "CartItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductId",
                table: "CartItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_ProductId",
                table: "Image",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
