using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionName.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_ClaimEntity_ClaimId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_ClaimEntity_ClaimId",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClaimEntity",
                table: "ClaimEntity");

            migrationBuilder.RenameTable(
                name: "ClaimEntity",
                newName: "Claims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Claims",
                table: "Claims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_Claims_ClaimId",
                table: "AspNetRoleClaims",
                column: "ClaimId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Claims_ClaimId",
                table: "AspNetUserClaims",
                column: "ClaimId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_Claims_ClaimId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Claims_ClaimId",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Claims",
                table: "Claims");

            migrationBuilder.RenameTable(
                name: "Claims",
                newName: "ClaimEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClaimEntity",
                table: "ClaimEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_ClaimEntity_ClaimId",
                table: "AspNetRoleClaims",
                column: "ClaimId",
                principalTable: "ClaimEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_ClaimEntity_ClaimId",
                table: "AspNetUserClaims",
                column: "ClaimId",
                principalTable: "ClaimEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
