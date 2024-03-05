﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NET.Dapr.Infrastructures;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NET.Dapr.DbMigration.Migrations
{
    [DbContext(typeof(PgDbContext))]
    [Migration("20240305101546_InitialDb")]
    partial class InitialDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("DAPR")
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NET.Dapr.Domains.Entities.ApproverConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long>("Id"), 1000L, null, null, null, null, null);

                    b.Property<string>("Code")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("CODE");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATED_DATE");

                    b.Property<string>("DivisionCode")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("DIVISION_CODE");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("EMAIL");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("NAME");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATED_DATE");

                    b.HasKey("Id");

                    b.ToTable("APPROVER_CONFIG", "DAPR");
                });

            modelBuilder.Entity("NET.Dapr.Domains.Entities.EmailHistories", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long>("Id"), 1000L, null, null, null, null, null);

                    b.Property<string>("BccEmail")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("BCC_EMAIL");

                    b.Property<string>("CcEmail")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("CC_EMAIL");

                    b.Property<string>("Content")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("CONTENT");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATED_DATE");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("STATUS");

                    b.Property<string>("Subject")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("SUBJECT");

                    b.Property<string>("ToEmail")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("TO_EMAIL");

                    b.Property<long?>("TransactionID")
                        .HasColumnType("bigint")
                        .HasColumnName("TRANSACTION_ID");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATED_DATE");

                    b.HasKey("Id");

                    b.ToTable("EMAIL_HISTORIES", "DAPR");
                });

            modelBuilder.Entity("NET.Dapr.Domains.Entities.LRTasks", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long>("Id"), 1000L, null, null, null, null, null);

                    b.Property<string>("Assignee")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("ASSIGNEE");

                    b.Property<string>("AssigneeEmail")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("ASSIGNEE_EMAIL");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATED_DATE");

                    b.Property<string>("Status")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("STATUS");

                    b.Property<string>("TaskName")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("TASK_NAME");

                    b.Property<long?>("TransactionId")
                        .HasColumnType("bigint")
                        .HasColumnName("TRANSACTION_ID");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATED_DATE");

                    b.HasKey("Id");

                    b.ToTable("TASKS", "DAPR");
                });

            modelBuilder.Entity("NET.Dapr.Domains.Entities.LRTransaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long>("Id"), 1000L, null, null, null, null, null);

                    b.Property<string>("Approver")
                        .HasColumnType("text")
                        .HasColumnName("APPROVER");

                    b.Property<string>("Bcc")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("BCC");

                    b.Property<string>("Cc")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("CC");

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("COMMENT");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATED_DATE");

                    b.Property<DateTime?>("DateOffFrom")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DATE_OFF_FROM");

                    b.Property<DateTime?>("DateOffTo")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DATE_OFF_TO");

                    b.Property<string>("DivisionCode")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("DIVISION_CODE");

                    b.Property<string>("EmployeeCode")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("EMPLOYEE_CODE");

                    b.Property<string>("EmployeeName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("EMPLOYEE_NAME");

                    b.Property<string>("LeaveRequestDateType")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("LEAVE_REQUEST_DATE_TYPE");

                    b.Property<string>("LeaveRequestType")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("LEAVE_REQUEST_TYPE");

                    b.Property<string>("PeriodDateOffFrom")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("PERIOD_DATE_OFF_FROM");

                    b.Property<string>("PeriodDateOffTo")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("PERIOD_DATE_OFF_TO");

                    b.Property<string>("Reason")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("REASON");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("STATUS");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATED_DATE");

                    b.Property<string>("WfInstanceId")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("WF_INSTANCE_ID");

                    b.HasKey("Id");

                    b.ToTable("LR_TRANSACTION", "DAPR");
                });

            modelBuilder.Entity("NET.Dapr.Domains.Entities.WorkflowFormConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATED_DATE");

                    b.Property<long>("FormId")
                        .HasColumnType("bigint")
                        .HasColumnName("FORM_ID");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATED_DATE");

                    b.Property<string>("WfCode")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("WF_CODE");

                    b.Property<string>("WfStep")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("WF_STEP");

                    b.HasKey("Id");

                    b.ToTable("WORKFLOW_FORM_CONFIG", "DAPR");
                });
#pragma warning restore 612, 618
        }
    }
}
