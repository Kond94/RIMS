using Microsoft.EntityFrameworkCore.Migrations;

namespace RIMS.Migrations
{
    public partial class seedContextWithPrimaryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            SET IDENTITY_INSERT [dbo].[EggTypes] ON
INSERT INTO [dbo].[EggTypes] ([Id], [Name], [CandlingDays], [HatchPreparationDays], [HatchDays]) VALUES (1, N'None', 0, 0, 0)
INSERT INTO [dbo].[EggTypes] ([Id], [Name], [CandlingDays], [HatchPreparationDays], [HatchDays]) VALUES (2, N'Quail', 10, 15, 20)
INSERT INTO [dbo].[EggTypes] ([Id], [Name], [CandlingDays], [HatchPreparationDays], [HatchDays]) VALUES (3, N'Chicken', 15, 20, 30)
INSERT INTO [dbo].[EggTypes] ([Id], [Name], [CandlingDays], [HatchPreparationDays], [HatchDays]) VALUES (4, N'Duck', 18, 25, 30)
INSERT INTO [dbo].[EggTypes] ([Id], [Name], [CandlingDays], [HatchPreparationDays], [HatchDays]) VALUES (5, N'Turkey', 10, 28, 40)
INSERT INTO [dbo].[EggTypes] ([Id], [Name], [CandlingDays], [HatchPreparationDays], [HatchDays]) VALUES (6, N'Gunea Fowl', 20, 15, 30)
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
INSERT INTO [dbo].[MonitoringDevices] ([Id], [Name], [Description], [Indentifier]) VALUES (1, N'ESP8266', N'Wifi Module', N'ESP8266-BetaTester')
INSERT INTO [dbo].[MonitoringDevices] ([Id], [Name], [Description], [Indentifier]) VALUES (2, N'None', N'None', N'None')
SET IDENTITY_INSERT [dbo].[MonitoringDevices] OFF
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM EggTypes WHERE Name IN ('None', 'Quail', 'Chicken', 'Duck', 'Turkey', 'Gunea Fowl')");
            migrationBuilder.Sql("DELETE FROM IncubatorModels WHERE Id IN (1,2,3,4)");
            migrationBuilder.Sql("DELETE FROM MeasurementTypes WHERE Id IN (1,2) ");
            migrationBuilder.Sql("DELETE FROM MonitoringDevices WHERE Id IN (1,2)");

        }
    }
}
