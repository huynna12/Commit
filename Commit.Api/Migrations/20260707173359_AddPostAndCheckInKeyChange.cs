using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commit.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPostAndCheckInKeyChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckIns",
                table: "CheckIns");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CheckIns",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckIns",
                table: "CheckIns",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_CheckIns_CheckInId",
                        column: x => x.CheckInId,
                        principalTable: "CheckIns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_ChallengeId_AppUserId_CheckInDate",
                table: "CheckIns",
                columns: new[] { "ChallengeId", "AppUserId", "CheckInDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CheckInId",
                table: "Posts",
                column: "CheckInId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckIns",
                table: "CheckIns");

            migrationBuilder.DropIndex(
                name: "IX_CheckIns_ChallengeId_AppUserId_CheckInDate",
                table: "CheckIns");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CheckIns");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckIns",
                table: "CheckIns",
                columns: new[] { "ChallengeId", "AppUserId", "CheckInDate" });
        }
    }
}
