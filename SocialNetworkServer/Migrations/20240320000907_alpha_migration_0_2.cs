using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkServer.Migrations
{
    /// <inheritdoc />
    public partial class alpha_migration_0_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FormattedRegistrationDate",
                table: "Users",
                type: "longtext",
                nullable: false,
                computedColumnSql: "DATE_FORMAT('2024-03-20 00:09', '%Y-%m-%d %H:%i')",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComputedColumnSql: "DATE_FORMAT('2024-03-20 00:04', '%Y-%m-%d %H:%i')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Login", "PasswordHash" },
                values: new object[] { 1, "kkkkk", "ohjhygui" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "FormattedRegistrationDate",
                table: "Users",
                type: "longtext",
                nullable: false,
                computedColumnSql: "DATE_FORMAT('2024-03-20 00:04', '%Y-%m-%d %H:%i')",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComputedColumnSql: "DATE_FORMAT('2024-03-20 00:09', '%Y-%m-%d %H:%i')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
