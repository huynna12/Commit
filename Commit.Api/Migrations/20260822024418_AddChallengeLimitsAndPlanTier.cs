using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commit.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddChallengeLimitsAndPlanTier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DurationInDays",
                table: "Challenges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MaxParticipants",
                table: "Challenges",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanTier",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxParticipants",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "PlanTier",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "DurationInDays",
                table: "Challenges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
