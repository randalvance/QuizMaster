using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizMaker.Data.Migrations
{
    public partial class SessionAnswerHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerChronology",
                table: "SessionAnswers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "AnswersOrderImportant",
                table: "Quizes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerChronology",
                table: "SessionAnswers");

            migrationBuilder.DropColumn(
                name: "AnswersOrderImportant",
                table: "Quizes");
        }
    }
}
