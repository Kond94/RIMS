using Microsoft.EntityFrameworkCore.Migrations;

namespace RIMS.Migrations
{
    public partial class addIncubatorIdtoMonitoringDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Indentifier",
                table: "MonitoringDevices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Indentifier",
                table: "MonitoringDevices",
                nullable: true);
        }
    }
}
