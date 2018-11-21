using Microsoft.EntityFrameworkCore.Migrations;

namespace RIMS.Migrations
{
    public partial class addIdentityUserToPushSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PushSubscriptions");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "PushSubscriptions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PushSubscriptions_IdentityUserId",
                table: "PushSubscriptions",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PushSubscriptions_AspNetUsers_IdentityUserId",
                table: "PushSubscriptions",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PushSubscriptions_AspNetUsers_IdentityUserId",
                table: "PushSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_PushSubscriptions_IdentityUserId",
                table: "PushSubscriptions");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "PushSubscriptions");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PushSubscriptions",
                nullable: true);
        }
    }
}
