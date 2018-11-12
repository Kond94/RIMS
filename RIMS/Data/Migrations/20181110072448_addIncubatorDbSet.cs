using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RIMS.Data.Migrations
{
    public partial class addIncubatorDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EggTypes",
                columns: table => new
                {
                    EggTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EggTypes", x => x.EggTypeId);
                });

            migrationBuilder.CreateTable(
                name: "IncubatorModels",
                columns: table => new
                {
                    IncubatorModelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Capacity = table.Column<int>(nullable: false),
                    RackWidth = table.Column<byte>(nullable: false),
                    RackLength = table.Column<byte>(nullable: false),
                    RackHeight = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorModels", x => x.IncubatorModelId);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementTypes",
                columns: table => new
                {
                    MeasurementTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementTypes", x => x.MeasurementTypeId);
                });

            migrationBuilder.CreateTable(
                name: "MonitoringDevices",
                columns: table => new
                {
                    MonitoringDeviceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringDevices", x => x.MonitoringDeviceId);
                });

            migrationBuilder.CreateTable(
                name: "Incubators",
                columns: table => new
                {
                    IncubatorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IncubatorModelId = table.Column<int>(nullable: false),
                    MonitoringDeviceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    LastIpAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incubators", x => x.IncubatorId);
                    table.ForeignKey(
                        name: "FK_Incubators_IncubatorModels_IncubatorModelId",
                        column: x => x.IncubatorModelId,
                        principalTable: "IncubatorModels",
                        principalColumn: "IncubatorModelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incubators_MonitoringDevices_MonitoringDeviceId",
                        column: x => x.MonitoringDeviceId,
                        principalTable: "MonitoringDevices",
                        principalColumn: "MonitoringDeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    MeasurementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IncubatorId = table.Column<int>(nullable: false),
                    MeasurementTypeId = table.Column<int>(nullable: false),
                    MeasuredValue = table.Column<decimal>(nullable: false),
                    MeasuredDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.MeasurementId);
                    table.ForeignKey(
                        name: "FK_Measurements_Incubators_IncubatorId",
                        column: x => x.IncubatorId,
                        principalTable: "Incubators",
                        principalColumn: "IncubatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Measurements_MeasurementTypes_MeasurementTypeId",
                        column: x => x.MeasurementTypeId,
                        principalTable: "MeasurementTypes",
                        principalColumn: "MeasurementTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Racks",
                columns: table => new
                {
                    RackId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IncubatorId = table.Column<int>(nullable: false),
                    RackNumber = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Racks", x => x.RackId);
                    table.ForeignKey(
                        name: "FK_Racks_Incubators_IncubatorId",
                        column: x => x.IncubatorId,
                        principalTable: "Incubators",
                        principalColumn: "IncubatorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RackContents",
                columns: table => new
                {
                    RackContentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RackId = table.Column<int>(nullable: false),
                    Row = table.Column<byte>(nullable: false),
                    Column = table.Column<byte>(nullable: false),
                    EggTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RackContents", x => x.RackContentId);
                    table.ForeignKey(
                        name: "FK_RackContents_EggTypes_EggTypeId",
                        column: x => x.EggTypeId,
                        principalTable: "EggTypes",
                        principalColumn: "EggTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RackContents_Racks_RackId",
                        column: x => x.RackId,
                        principalTable: "Racks",
                        principalColumn: "RackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incubators_IncubatorModelId",
                table: "Incubators",
                column: "IncubatorModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Incubators_MonitoringDeviceId",
                table: "Incubators",
                column: "MonitoringDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_IncubatorId",
                table: "Measurements",
                column: "IncubatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_MeasurementTypeId",
                table: "Measurements",
                column: "MeasurementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RackContents_EggTypeId",
                table: "RackContents",
                column: "EggTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RackContents_RackId",
                table: "RackContents",
                column: "RackId");

            migrationBuilder.CreateIndex(
                name: "IX_Racks_IncubatorId",
                table: "Racks",
                column: "IncubatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "RackContents");

            migrationBuilder.DropTable(
                name: "MeasurementTypes");

            migrationBuilder.DropTable(
                name: "EggTypes");

            migrationBuilder.DropTable(
                name: "Racks");

            migrationBuilder.DropTable(
                name: "Incubators");

            migrationBuilder.DropTable(
                name: "IncubatorModels");

            migrationBuilder.DropTable(
                name: "MonitoringDevices");
        }
    }
}
