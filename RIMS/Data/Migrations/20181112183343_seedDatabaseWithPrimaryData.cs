using Microsoft.EntityFrameworkCore.Migrations;

namespace RIMS.Migrations
{
    public partial class seedDatabaseWithPrimaryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            SET IDENTITY_INSERT [dbo].[EggTypes] ON
INSERT INTO [dbo].[EggTypes] ([Id], [Name]) VALUES (1, N'None')
INSERT INTO [dbo].[EggTypes] ([Id], [Name]) VALUES (2, N'Quail')
INSERT INTO [dbo].[EggTypes] ([Id], [Name]) VALUES (3, N'Chicken')
INSERT INTO [dbo].[EggTypes] ([Id], [Name]) VALUES (4, N'Duck')
INSERT INTO [dbo].[EggTypes] ([Id], [Name]) VALUES (5, N'Turkey')
INSERT INTO [dbo].[EggTypes] ([Id], [Name]) VALUES (6, N'Nkhanga')
SET IDENTITY_INSERT [dbo].[EggTypes] OFF

SET IDENTITY_INSERT [dbo].[IncubatorModels] ON
INSERT INTO [dbo].[IncubatorModels] ([Id], [Capacity], [RackWidth], [RackLength], [RackHeight]) VALUES (1, 1500, 2, 5, 5)
INSERT INTO [dbo].[IncubatorModels] ([Id], [Capacity], [RackWidth], [RackLength], [RackHeight]) VALUES (2, 2400, 2, 8, 5)
INSERT INTO [dbo].[IncubatorModels] ([Id], [Capacity], [RackWidth], [RackLength], [RackHeight]) VALUES (3, 3600, 2, 10, 6)
INSERT INTO [dbo].[IncubatorModels] ([Id], [Capacity], [RackWidth], [RackLength], [RackHeight]) VALUES (4, 4800, 2, 10, 8)
SET IDENTITY_INSERT [dbo].[IncubatorModels] OFF

SET IDENTITY_INSERT [dbo].[MeasurementTypes] ON
INSERT INTO [dbo].[MeasurementTypes] ([Id], [Name], [Description]) VALUES (1, N'Tempreture', N'Tempreture In Celcius')
INSERT INTO [dbo].[MeasurementTypes] ([Id], [Name], [Description]) VALUES (2, N'Humidity', N'Humidity as Percentage')
SET IDENTITY_INSERT [dbo].[MeasurementTypes] OFF

SET IDENTITY_INSERT [dbo].[MonitoringDevices] ON
INSERT INTO [dbo].[MonitoringDevices] ([Id], [Name], [Description]) VALUES (1, N'ESP8266', N'Wifi Module')
INSERT INTO [dbo].[MonitoringDevices] ([Id], [Name], [Description]) VALUES (2, N'None', N'None')
SET IDENTITY_INSERT [dbo].[MonitoringDevices] OFF
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM EggTypes");
            migrationBuilder.Sql("DELETE FROM IncubatorModels");
            migrationBuilder.Sql("DELETE FROM MeasurementTypes");
            migrationBuilder.Sql("DELETE FROM MonitoringDevices");

        }
    }
}
