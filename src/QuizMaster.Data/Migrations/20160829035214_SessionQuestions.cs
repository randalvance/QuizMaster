using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizMaster.Data.Migrations
{
    public partial class SessionQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionQuestions",
                columns: table => new
                {
                    SessionQuestionId = table.Column<Guid>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    SessionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionQuestions", x => x.SessionQuestionId);
                    table.ForeignKey(
                        name: "FK_SessionQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionQuestions_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "Questions",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuestions_QuestionId",
                table: "SessionQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuestions_SessionId",
                table: "SessionQuestions",
                column: "SessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionQuestions");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "Questions",
                nullable: true);
        }
    }
}
