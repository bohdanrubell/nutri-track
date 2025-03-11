using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedImportantEntittesForDiary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diary_AspNetUsers_UserId",
                table: "Diary");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_ActivityLevelLogs_ActivityLogId",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Diary_DiaryId",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_GoalTypeLogs_GoalLogId",
                table: "Record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Record",
                table: "Record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Diary",
                table: "Diary");

            migrationBuilder.RenameTable(
                name: "Record",
                newName: "Records");

            migrationBuilder.RenameTable(
                name: "Diary",
                newName: "Diaries");

            migrationBuilder.RenameIndex(
                name: "IX_Record_GoalLogId",
                table: "Records",
                newName: "IX_Records_GoalLogId");

            migrationBuilder.RenameIndex(
                name: "IX_Record_DiaryId",
                table: "Records",
                newName: "IX_Records_DiaryId");

            migrationBuilder.RenameIndex(
                name: "IX_Record_ActivityLogId",
                table: "Records",
                newName: "IX_Records_ActivityLogId");

            migrationBuilder.RenameIndex(
                name: "IX_Diary_UserId",
                table: "Diaries",
                newName: "IX_Diaries_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Records",
                table: "Records",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Diaries",
                table: "Diaries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Grams = table.Column<double>(type: "REAL", nullable: false),
                    ProductNutritionId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecordId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecords_ProductNutritions_ProductNutritionId",
                        column: x => x.ProductNutritionId,
                        principalTable: "ProductNutritions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecords_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "Records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecords_ProductNutritionId",
                table: "ProductRecords",
                column: "ProductNutritionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecords_RecordId",
                table: "ProductRecords",
                column: "RecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diaries_AspNetUsers_UserId",
                table: "Diaries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_ActivityLevelLogs_ActivityLogId",
                table: "Records",
                column: "ActivityLogId",
                principalTable: "ActivityLevelLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Diaries_DiaryId",
                table: "Records",
                column: "DiaryId",
                principalTable: "Diaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_GoalTypeLogs_GoalLogId",
                table: "Records",
                column: "GoalLogId",
                principalTable: "GoalTypeLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diaries_AspNetUsers_UserId",
                table: "Diaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_ActivityLevelLogs_ActivityLogId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_Diaries_DiaryId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_GoalTypeLogs_GoalLogId",
                table: "Records");

            migrationBuilder.DropTable(
                name: "ProductRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Records",
                table: "Records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Diaries",
                table: "Diaries");

            migrationBuilder.RenameTable(
                name: "Records",
                newName: "Record");

            migrationBuilder.RenameTable(
                name: "Diaries",
                newName: "Diary");

            migrationBuilder.RenameIndex(
                name: "IX_Records_GoalLogId",
                table: "Record",
                newName: "IX_Record_GoalLogId");

            migrationBuilder.RenameIndex(
                name: "IX_Records_DiaryId",
                table: "Record",
                newName: "IX_Record_DiaryId");

            migrationBuilder.RenameIndex(
                name: "IX_Records_ActivityLogId",
                table: "Record",
                newName: "IX_Record_ActivityLogId");

            migrationBuilder.RenameIndex(
                name: "IX_Diaries_UserId",
                table: "Diary",
                newName: "IX_Diary_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Record",
                table: "Record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Diary",
                table: "Diary",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Diary_AspNetUsers_UserId",
                table: "Diary",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Record_ActivityLevelLogs_ActivityLogId",
                table: "Record",
                column: "ActivityLogId",
                principalTable: "ActivityLevelLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Diary_DiaryId",
                table: "Record",
                column: "DiaryId",
                principalTable: "Diary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Record_GoalTypeLogs_GoalLogId",
                table: "Record",
                column: "GoalLogId",
                principalTable: "GoalTypeLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
