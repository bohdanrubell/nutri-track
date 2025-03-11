using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DailyCalories = table.Column<float>(type: "REAL", nullable: false),
                    DailyProtein = table.Column<float>(type: "REAL", nullable: false),
                    DailyFat = table.Column<float>(type: "REAL", nullable: false),
                    DailyCarbohydrates = table.Column<float>(type: "REAL", nullable: false),
                    DiaryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivityLogId = table.Column<int>(type: "INTEGER", nullable: false),
                    GoalLogId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Record_ActivityLevelLogs_ActivityLogId",
                        column: x => x.ActivityLogId,
                        principalTable: "ActivityLevelLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Record_Diary_DiaryId",
                        column: x => x.DiaryId,
                        principalTable: "Diary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Record_GoalTypeLogs_GoalLogId",
                        column: x => x.GoalLogId,
                        principalTable: "GoalTypeLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Record_ActivityLogId",
                table: "Record",
                column: "ActivityLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_DiaryId",
                table: "Record",
                column: "DiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_GoalLogId",
                table: "Record",
                column: "GoalLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Record");
        }
    }
}
