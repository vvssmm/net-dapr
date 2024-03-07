using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET.Dapr.DbMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveRequestNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DATE_OFF",
                schema: "DAPR",
                table: "LR_TRANSACTION",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PERIOD_DATE_OFF",
                schema: "DAPR",
                table: "LR_TRANSACTION",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DATE_OFF",
                schema: "DAPR",
                table: "LR_TRANSACTION");

            migrationBuilder.DropColumn(
                name: "PERIOD_DATE_OFF",
                schema: "DAPR",
                table: "LR_TRANSACTION");
        }
    }
}
