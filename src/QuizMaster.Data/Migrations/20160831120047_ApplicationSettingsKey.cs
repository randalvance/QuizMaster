using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizMaster.Data.Migrations
{
    public partial class ApplicationSettingsKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "ApplicationSettings",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_ApplicationSetting_Key",
                table: "ApplicationSettings",
                column: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UQ_ApplicationSetting_Key",
                table: "ApplicationSettings");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "ApplicationSettings");
        }
    }
}
