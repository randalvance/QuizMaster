using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizMaster.Data.Migrations
{
    public partial class QuizGroupAddDescriptionAndVariousRequiredness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "QuizGroups",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QuizGroups",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QuizCategories",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "QuizGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QuizGroups",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QuizCategories",
                nullable: true);
        }
    }
}
