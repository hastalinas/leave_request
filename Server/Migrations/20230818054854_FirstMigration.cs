using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class FirstMigration : Migration
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
                    created_time = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    created_time = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    created_time = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    profil_picture = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false),
                    expired_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_time = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_time = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    created_time = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                        name: "FK_tb_tr_account_roles_tb_m_roles_account_guid",
                        column: x => x.account_guid,
                        principalTable: "tb_m_roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_feedback",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    leave_request = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    feedback_status = table.Column<int>(type: "int", nullable: false),
                    feedback_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_feedback", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_feedback_tb_m_leave_requests_leave_request",
                        column: x => x.leave_request,
                        principalTable: "tb_m_leave_requests",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_time", "modified_date", "name" },
                values: new object[] { new Guid("062f261d-790b-47d4-b250-ca9f6cf25cb0"), new DateTime(2023, 8, 18, 5, 48, 54, 268, DateTimeKind.Utc).AddTicks(2710), new DateTime(2023, 8, 18, 5, 48, 54, 268, DateTimeKind.Utc).AddTicks(2711), "employee" });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_time", "modified_date", "name" },
                values: new object[] { new Guid("071fbe77-34e6-4baf-8739-9ebf03f5bb38"), new DateTime(2023, 8, 18, 5, 48, 54, 268, DateTimeKind.Utc).AddTicks(2734), new DateTime(2023, 8, 18, 5, 48, 54, 268, DateTimeKind.Utc).AddTicks(2735), "manager" });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_time", "modified_date", "name" },
                values: new object[] { new Guid("b52fed99-9d48-4d59-acb0-b2252f7a16da"), new DateTime(2023, 8, 18, 5, 48, 54, 268, DateTimeKind.Utc).AddTicks(2705), new DateTime(2023, 8, 18, 5, 48, 54, 268, DateTimeKind.Utc).AddTicks(2708), "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_department_guid",
                table: "tb_m_employees",
                column: "department_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_manager_guid",
                table: "tb_m_employees",
                column: "manager_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_feedback_leave_request",
                table: "tb_m_feedback",
                column: "leave_request",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_leave_requests_employee_guid",
                table: "tb_m_leave_requests",
                column: "employee_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_m_feedback");

            migrationBuilder.DropTable(
                name: "tb_tr_account_roles");

            migrationBuilder.DropTable(
                name: "tb_m_leave_requests");

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
