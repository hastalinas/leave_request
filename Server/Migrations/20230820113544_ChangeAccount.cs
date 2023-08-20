using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class ChangeAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_account_guid",
                table: "tb_tr_account_roles");

            migrationBuilder.DeleteData(
                table: "tb_m_roles",
                keyColumn: "guid",
                keyValue: new Guid("5dc74b69-3bde-48f2-95e6-0fc1dadf2aa7"));

            migrationBuilder.DeleteData(
                table: "tb_m_roles",
                keyColumn: "guid",
                keyValue: new Guid("7a9ac9ab-d473-4954-89d5-eb085c7d474b"));

            migrationBuilder.DeleteData(
                table: "tb_m_roles",
                keyColumn: "guid",
                keyValue: new Guid("cb821f06-efb1-44db-88d6-6dc4405de080"));

            migrationBuilder.DropColumn(
                name: "status",
                table: "tb_m_leave_requests");

            migrationBuilder.AddColumn<Guid>(
                name: "account_role",
                table: "tb_tr_account_roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<byte[]>(
                name: "attachment",
                table: "tb_m_leave_requests",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<int>(
                name: "leave_remain",
                table: "tb_m_employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<byte[]>(
                name: "profil_picture",
                table: "tb_m_accounts",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.InsertData(
                table: "tb_m_departments",
                columns: new[] { "guid", "code", "created_date", "modified_date", "name" },
                values: new object[,]
                {
                    { new Guid("02988287-198d-4dab-53cc-08dba0d4ed05"), "RND", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2052), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2053), "Research and Development" },
                    { new Guid("1e4f0537-3ca0-4d64-53cf-08dba0d4ed05"), "CS", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2058), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2059), "Customer Service" },
                    { new Guid("1fcc1546-78e3-4baf-53ca-08dba0d4ed05"), "FINANCE", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2048), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2048), "Finance" },
                    { new Guid("51d55a47-1cab-42e6-53c9-08dba0d4ed05"), "MARKETING", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2046), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2046), "Marketing" },
                    { new Guid("5eac3979-fc26-4017-53d1-08dba0d4ed05"), "QA", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2066), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2067), "Quality Assurance" },
                    { new Guid("8ccd5722-3f93-484d-53cd-08dba0d4ed05"), "IT", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2054), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2054), "Information Technology" },
                    { new Guid("9b3c7c65-c99a-4e97-53ce-08dba0d4ed05"), "OPS", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2056), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2057), "Operations" },
                    { new Guid("b41e4d54-2ffe-4619-53c8-08dba0d4ed05"), "SALES", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2043), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2044), "Sales" },
                    { new Guid("bb4e21b9-f8ac-40ad-53d0-08dba0d4ed05"), "PROD", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2065), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2065), "Production" },
                    { new Guid("e707fb58-cdb1-4c2a-53cb-08dba0d4ed05"), "HR", new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2050), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(2050), "Human Resources" }
                });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_date", "modified_date", "name" },
                values: new object[,]
                {
                    { new Guid("36350d33-42d7-4c63-a244-29b0a8d13bce"), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(1876), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(1880), "admin" },
                    { new Guid("4887ec13-b482-47b3-9b24-08db91a71770"), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(1884), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(1884), "employee" },
                    { new Guid("a7e15d29-9c74-4e72-ae63-5a47d69b27d6"), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(1888), new DateTime(2023, 8, 20, 11, 35, 44, 600, DateTimeKind.Utc).AddTicks(1889), "manager" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_account_role",
                table: "tb_tr_account_roles",
                column: "account_role");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_account_role",
                table: "tb_tr_account_roles",
                column: "account_role",
                principalTable: "tb_m_roles",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_account_role",
                table: "tb_tr_account_roles");

            migrationBuilder.DropIndex(
                name: "IX_tb_tr_account_roles_account_role",
                table: "tb_tr_account_roles");

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("02988287-198d-4dab-53cc-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("1e4f0537-3ca0-4d64-53cf-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("1fcc1546-78e3-4baf-53ca-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("51d55a47-1cab-42e6-53c9-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("5eac3979-fc26-4017-53d1-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("8ccd5722-3f93-484d-53cd-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("9b3c7c65-c99a-4e97-53ce-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("b41e4d54-2ffe-4619-53c8-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("bb4e21b9-f8ac-40ad-53d0-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_departments",
                keyColumn: "guid",
                keyValue: new Guid("e707fb58-cdb1-4c2a-53cb-08dba0d4ed05"));

            migrationBuilder.DeleteData(
                table: "tb_m_roles",
                keyColumn: "guid",
                keyValue: new Guid("36350d33-42d7-4c63-a244-29b0a8d13bce"));

            migrationBuilder.DeleteData(
                table: "tb_m_roles",
                keyColumn: "guid",
                keyValue: new Guid("4887ec13-b482-47b3-9b24-08db91a71770"));

            migrationBuilder.DeleteData(
                table: "tb_m_roles",
                keyColumn: "guid",
                keyValue: new Guid("a7e15d29-9c74-4e72-ae63-5a47d69b27d6"));

            migrationBuilder.DropColumn(
                name: "account_role",
                table: "tb_tr_account_roles");

            migrationBuilder.DropColumn(
                name: "leave_remain",
                table: "tb_m_employees");

            migrationBuilder.AlterColumn<byte[]>(
                name: "attachment",
                table: "tb_m_leave_requests",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "tb_m_leave_requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<byte[]>(
                name: "profil_picture",
                table: "tb_m_accounts",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_date", "modified_date", "name" },
                values: new object[] { new Guid("5dc74b69-3bde-48f2-95e6-0fc1dadf2aa7"), new DateTime(2023, 8, 18, 9, 23, 28, 496, DateTimeKind.Utc).AddTicks(1395), new DateTime(2023, 8, 18, 9, 23, 28, 496, DateTimeKind.Utc).AddTicks(1395), "manager" });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_date", "modified_date", "name" },
                values: new object[] { new Guid("7a9ac9ab-d473-4954-89d5-eb085c7d474b"), new DateTime(2023, 8, 18, 9, 23, 28, 496, DateTimeKind.Utc).AddTicks(1392), new DateTime(2023, 8, 18, 9, 23, 28, 496, DateTimeKind.Utc).AddTicks(1392), "employee" });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_date", "modified_date", "name" },
                values: new object[] { new Guid("cb821f06-efb1-44db-88d6-6dc4405de080"), new DateTime(2023, 8, 18, 9, 23, 28, 496, DateTimeKind.Utc).AddTicks(1388), new DateTime(2023, 8, 18, 9, 23, 28, 496, DateTimeKind.Utc).AddTicks(1390), "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid",
                principalTable: "tb_m_roles",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
