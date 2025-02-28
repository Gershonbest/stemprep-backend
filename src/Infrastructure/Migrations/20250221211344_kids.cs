using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class kids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Students_StudentId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_StudentId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "ProgrammeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Modules");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Tutors",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Students",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Students",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Parents",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "Tutors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLockedOut",
                table: "Tutors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "PhoneNumbers",
                table: "Tutors",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "Students",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLockedOut",
                table: "Students",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ParentEmail",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "PhoneNumbers",
                table: "Students",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "Parents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLockedOut",
                table: "Parents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "PhoneNumbers",
                table: "Parents",
                type: "text[]",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    UserStatus = table.Column<int>(type: "integer", nullable: false),
                    UserStatusDes = table.Column<string>(type: "text", nullable: true),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    UserTypeDesc = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "integer", nullable: false),
                    IsLockedOut = table.Column<bool>(type: "boolean", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    PhoneNumbers = table.Column<List<string>>(type: "text[]", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    Province = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "IsLockedOut",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "PhoneNumbers",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsLockedOut",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ParentEmail",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumbers",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "IsLockedOut",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "PhoneNumbers",
                table: "Parents");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Tutors",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Students",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Students",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Parents",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Tutors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgrammeId",
                table: "Students",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Parents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Modules",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_StudentId",
                table: "Modules",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Students_StudentId",
                table: "Modules",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
