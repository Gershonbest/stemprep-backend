using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class objectsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserGuId",
                table: "Documents",
                newName: "UserGuid");

            migrationBuilder.AddColumn<int>(
                name: "AccountStatus",
                table: "Tutors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccountStatusDesc",
                table: "Tutors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityStatus",
                table: "Tutors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AvailabilityStatusDesc",
                table: "Tutors",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "AccountStatusDesc",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "AvailabilityStatus",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "AvailabilityStatusDesc",
                table: "Tutors");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "Documents",
                newName: "UserGuId");
        }
    }
}
