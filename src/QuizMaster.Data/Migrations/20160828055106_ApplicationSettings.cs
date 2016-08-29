using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizMaster.Data.Migrations
{
    public partial class ApplicationSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                columns: table => new
                {
                    ApplicationSettingId = table.Column<Guid>(nullable: false),
                    ApplicationSettingValueType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.ApplicationSettingId);
                    table.UniqueConstraint("UQ_ApplicationSetting_Name", x => x.Name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSettings");
        }
    }
}
