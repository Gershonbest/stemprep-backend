using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class singleRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Admins_AdminId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Parents_ParentId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Students_StudentId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Tutors_TutorId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_AdminId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_ParentId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_StudentId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_TutorId",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId",
                table: "Tutors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId1",
                table: "Tutors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId",
                table: "Students",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId1",
                table: "Students",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId",
                table: "Parents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId1",
                table: "Parents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId",
                table: "Admins",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId1",
                table: "Admins",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tutors_RefreshTokenId1",
                table: "Tutors",
                column: "RefreshTokenId1");

            migrationBuilder.CreateIndex(
                name: "IX_Students_RefreshTokenId1",
                table: "Students",
                column: "RefreshTokenId1");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_RefreshTokenId1",
                table: "Parents",
                column: "RefreshTokenId1");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_RefreshTokenId1",
                table: "Admins",
                column: "RefreshTokenId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_RefreshTokens_RefreshTokenId1",
                table: "Admins",
                column: "RefreshTokenId1",
                principalTable: "RefreshTokens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_RefreshTokens_RefreshTokenId1",
                table: "Parents",
                column: "RefreshTokenId1",
                principalTable: "RefreshTokens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_RefreshTokens_RefreshTokenId1",
                table: "Students",
                column: "RefreshTokenId1",
                principalTable: "RefreshTokens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutors_RefreshTokens_RefreshTokenId1",
                table: "Tutors",
                column: "RefreshTokenId1",
                principalTable: "RefreshTokens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_RefreshTokens_RefreshTokenId1",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_RefreshTokens_RefreshTokenId1",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_RefreshTokens_RefreshTokenId1",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutors_RefreshTokens_RefreshTokenId1",
                table: "Tutors");

            migrationBuilder.DropIndex(
                name: "IX_Tutors_RefreshTokenId1",
                table: "Tutors");

            migrationBuilder.DropIndex(
                name: "IX_Students_RefreshTokenId1",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Parents_RefreshTokenId1",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Admins_RefreshTokenId1",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId1",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId1",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId1",
                table: "Admins");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AdminId",
                table: "RefreshTokens",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ParentId",
                table: "RefreshTokens",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_StudentId",
                table: "RefreshTokens",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TutorId",
                table: "RefreshTokens",
                column: "TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Admins_AdminId",
                table: "RefreshTokens",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Parents_ParentId",
                table: "RefreshTokens",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Students_StudentId",
                table: "RefreshTokens",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Tutors_TutorId",
                table: "RefreshTokens",
                column: "TutorId",
                principalTable: "Tutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
