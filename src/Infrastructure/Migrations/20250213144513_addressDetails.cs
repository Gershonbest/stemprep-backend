using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addressDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Tutors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Tutors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Tutors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Tutors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Tutors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Parents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Parents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Parents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Parents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Parents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Parents");
        }
    }
}
