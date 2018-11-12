using Microsoft.EntityFrameworkCore.Migrations;

namespace RIMS.Data.Migrations
{
    public partial class addUserIdToIncubator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Incubators",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Incubators_IdentityUserId",
                table: "Incubators",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incubators_AspNetUsers_IdentityUserId",
                table: "Incubators",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incubators_AspNetUsers_IdentityUserId",
                table: "Incubators");

            migrationBuilder.DropIndex(
                name: "IX_Incubators_IdentityUserId",
                table: "Incubators");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Incubators");
        }
    }
}
