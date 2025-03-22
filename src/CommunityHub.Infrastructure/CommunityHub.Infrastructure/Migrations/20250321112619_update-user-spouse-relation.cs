using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunityHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateuserspouserelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpouseInfo_UserInfo_UserInfoId1",
                table: "SpouseInfo");

            migrationBuilder.DropIndex(
                name: "IX_SpouseInfo_UserInfoId",
                table: "SpouseInfo");

            migrationBuilder.DropIndex(
                name: "IX_SpouseInfo_UserInfoId1",
                table: "SpouseInfo");

            migrationBuilder.DropColumn(
                name: "UserInfoId1",
                table: "SpouseInfo");

            migrationBuilder.CreateIndex(
                name: "IX_SpouseInfo_UserInfoId",
                table: "SpouseInfo",
                column: "UserInfoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpouseInfo_UserInfoId",
                table: "SpouseInfo");

            migrationBuilder.AddColumn<int>(
                name: "UserInfoId1",
                table: "SpouseInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpouseInfo_UserInfoId",
                table: "SpouseInfo",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SpouseInfo_UserInfoId1",
                table: "SpouseInfo",
                column: "UserInfoId1",
                unique: true,
                filter: "[UserInfoId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SpouseInfo_UserInfo_UserInfoId1",
                table: "SpouseInfo",
                column: "UserInfoId1",
                principalTable: "UserInfo",
                principalColumn: "Id");
        }
    }
}
