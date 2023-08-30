using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class DeleteFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_departments",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_departments", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_roles", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_employees",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    birth_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    hiring_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    department_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    manager_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    leave_remain = table.Column<int>(type: "int", nullable: false),
                    LastLeaveUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_employees", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_employees_tb_m_departments_department_guid",
                        column: x => x.department_guid,
                        principalTable: "tb_m_departments",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_m_employees_tb_m_employees_manager_guid",
                        column: x => x.manager_guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid");
                });

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    profil_picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    expired_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_tb_m_employees_guid",
                        column: x => x.guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_leave_requests",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    employee_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    leave_type = table.Column<int>(type: "int", nullable: false),
                    leave_start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    leave_end = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FeedbackNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_leave_requests", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_leave_requests_tb_m_employees_employee_guid",
                        column: x => x.employee_guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_account_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_role = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_account_roles", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_account_roles_tb_m_accounts_account_guid",
                        column: x => x.account_guid,
                        principalTable: "tb_m_accounts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_tr_account_roles_tb_m_roles_account_role",
                        column: x => x.account_role,
                        principalTable: "tb_m_roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tb_m_departments",
                columns: new[] { "guid", "code", "created_date", "modified_date", "name" },
                values: new object[,]
                {
                    { new Guid("02988287-198d-4dab-53cc-08dba0d4ed05"), "RND", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4756), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4757), "Research and Development" },
                    { new Guid("1e4f0537-3ca0-4d64-53cf-08dba0d4ed05"), "CS", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4762), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4763), "Customer Service" },
                    { new Guid("1fcc1546-78e3-4baf-53ca-08dba0d4ed05"), "FINANCE", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4751), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4752), "Finance" },
                    { new Guid("51d55a47-1cab-42e6-53c9-08dba0d4ed05"), "MARKETING", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4748), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4749), "Marketing" },
                    { new Guid("5eac3979-fc26-4017-53d1-08dba0d4ed05"), "QA", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4767), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4768), "Quality Assurance" },
                    { new Guid("8ccd5722-3f93-484d-53cd-08dba0d4ed05"), "IT", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4758), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4759), "Information Technology" },
                    { new Guid("9b3c7c65-c99a-4e97-53ce-08dba0d4ed05"), "OPS", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4760), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4761), "Operations" },
                    { new Guid("b41e4d54-2ffe-4619-53c8-08dba0d4ed05"), "SALES", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4745), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4746), "Sales" },
                    { new Guid("bb4e21b9-f8ac-40ad-53d0-08dba0d4ed05"), "PROD", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4765), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4766), "Production" },
                    { new Guid("e707fb58-cdb1-4c2a-53cb-08dba0d4ed05"), "HR", new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4754), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4755), "Human Resources" }
                });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_date", "modified_date", "name" },
                values: new object[,]
                {
                    { new Guid("36350d33-42d7-4c63-a244-29b0a8d13bce"), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4533), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4537), "admin" },
                    { new Guid("4887ec13-b482-47b3-9b24-08db91a71770"), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4540), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4541), "employee" },
                    { new Guid("a7e15d29-9c74-4e72-ae63-5a47d69b27d6"), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4544), new DateTime(2023, 8, 30, 2, 54, 23, 534, DateTimeKind.Utc).AddTicks(4545), "manager" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_department_guid",
                table: "tb_m_employees",
                column: "department_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_manager_guid",
                table: "tb_m_employees",
                column: "manager_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_leave_requests_employee_guid",
                table: "tb_m_leave_requests",
                column: "employee_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_account_role",
                table: "tb_tr_account_roles",
                column: "account_role");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_m_leave_requests");

            migrationBuilder.DropTable(
                name: "tb_tr_account_roles");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_employees");

            migrationBuilder.DropTable(
                name: "tb_m_departments");
        }
    }
}
