using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunityHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createuserregistrationtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpouseInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HomeTown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpouseInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeTown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpouseInfoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInfo_SpouseInfo_SpouseInfoId",
                        column: x => x.SpouseInfoId,
                        principalTable: "SpouseInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Children",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Children_UserInfo_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Children_UserInfoId",
                table: "Children",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SpouseInfo_ContactNumber",
                table: "SpouseInfo",
                column: "ContactNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpouseInfo_Email",
                table: "SpouseInfo",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_ContactNumber",
                table: "UserInfo",
                column: "ContactNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_Email",
                table: "UserInfo",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_SpouseInfoId",
                table: "UserInfo",
                column: "SpouseInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Children");

            migrationBuilder.DropTable(
                name: "RegistrationRequests");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "SpouseInfo");
        }
    }
}
