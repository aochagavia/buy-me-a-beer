using Microsoft.EntityFrameworkCore.Migrations;

namespace Website.Migrations
{
    public partial class PurchaseBecomesPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Purchases_PurchaseId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "PurchaseId",
                table: "Comments",
                newName: "PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PurchaseId",
                table: "Comments",
                newName: "IX_Comments_PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Purchases_PaymentId",
                table: "Comments",
                column: "PaymentId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Purchases_PaymentId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Comments",
                newName: "PurchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PaymentId",
                table: "Comments",
                newName: "IX_Comments_PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Purchases_PurchaseId",
                table: "Comments",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
