using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkServer.Migrations
{
    /// <inheritdoc />
    public partial class migration_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSubscriptions_Groups_SubscribedToGroupId",
                table: "GroupSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSubscriptions_Users_SubscriberId",
                table: "GroupSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSubscriptions",
                table: "GroupSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_GroupSubscriptions_SubscriberId",
                table: "GroupSubscriptions");

            migrationBuilder.DropColumn(
                name: "GroupSubscriptionId",
                table: "GroupSubscriptions");

            migrationBuilder.RenameTable(
                name: "GroupSubscriptions",
                newName: "UserGroupSubscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSubscriptions_SubscribedToGroupId",
                table: "UserGroupSubscriptions",
                newName: "IX_UserGroupSubscriptions_SubscribedToGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroupSubscriptions",
                table: "UserGroupSubscriptions",
                columns: new[] { "SubscriberId", "SubscribedToGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupSubscriptions_Groups_SubscribedToGroupId",
                table: "UserGroupSubscriptions",
                column: "SubscribedToGroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupSubscriptions_Users_SubscriberId",
                table: "UserGroupSubscriptions",
                column: "SubscriberId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupSubscriptions_Groups_SubscribedToGroupId",
                table: "UserGroupSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupSubscriptions_Users_SubscriberId",
                table: "UserGroupSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroupSubscriptions",
                table: "UserGroupSubscriptions");

            migrationBuilder.RenameTable(
                name: "UserGroupSubscriptions",
                newName: "GroupSubscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroupSubscriptions_SubscribedToGroupId",
                table: "GroupSubscriptions",
                newName: "IX_GroupSubscriptions_SubscribedToGroupId");

            migrationBuilder.AddColumn<int>(
                name: "GroupSubscriptionId",
                table: "GroupSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSubscriptions",
                table: "GroupSubscriptions",
                column: "GroupSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubscriptions_SubscriberId",
                table: "GroupSubscriptions",
                column: "SubscriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSubscriptions_Groups_SubscribedToGroupId",
                table: "GroupSubscriptions",
                column: "SubscribedToGroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSubscriptions_Users_SubscriberId",
                table: "GroupSubscriptions",
                column: "SubscriberId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
