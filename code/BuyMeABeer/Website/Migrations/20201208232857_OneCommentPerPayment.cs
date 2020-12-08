using Microsoft.EntityFrameworkCore.Migrations;

namespace Website.Migrations
{
    public partial class OneCommentPerPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_PaymentId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PaymentId",
                table: "Comments",
                column: "PaymentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_PaymentId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PaymentId",
                table: "Comments",
                column: "PaymentId");
        }
    }
}
