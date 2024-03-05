using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NET.Dapr.DbMigration.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DAPR");

            migrationBuilder.CreateTable(
                name: "APPROVER_CONFIG",
                schema: "DAPR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DIVISION_CODE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    NAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EMAIL = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CODE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVER_CONFIG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EMAIL_HISTORIES",
                schema: "DAPR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SUBJECT = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CONTENT = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    TO_EMAIL = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CC_EMAIL = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BCC_EMAIL = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    TRANSACTION_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMAIL_HISTORIES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LR_TRANSACTION",
                schema: "DAPR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WF_INSTANCE_ID = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EMPLOYEE_CODE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EMPLOYEE_NAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DIVISION_CODE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    STATUS = table.Column<string>(type: "text", nullable: true),
                    REASON = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DATE_OFF_FROM = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DATE_OFF_TO = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    APPROVER = table.Column<string>(type: "text", nullable: true),
                    CC = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BCC = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LEAVE_REQUEST_TYPE = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PERIOD_DATE_OFF_FROM = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PERIOD_DATE_OFF_TO = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LEAVE_REQUEST_DATE_TYPE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    COMMENT = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LR_TRANSACTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TASKS",
                schema: "DAPR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TASK_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ASSIGNEE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ASSIGNEE_EMAIL = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    STATUS = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TRANSACTION_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASKS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WORKFLOW_FORM_CONFIG",
                schema: "DAPR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WF_CODE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FORM_ID = table.Column<long>(type: "bigint", nullable: false),
                    WF_STEP = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKFLOW_FORM_CONFIG", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APPROVER_CONFIG",
                schema: "DAPR");

            migrationBuilder.DropTable(
                name: "EMAIL_HISTORIES",
                schema: "DAPR");

            migrationBuilder.DropTable(
                name: "LR_TRANSACTION",
                schema: "DAPR");

            migrationBuilder.DropTable(
                name: "TASKS",
                schema: "DAPR");

            migrationBuilder.DropTable(
                name: "WORKFLOW_FORM_CONFIG",
                schema: "DAPR");
        }
    }
}
