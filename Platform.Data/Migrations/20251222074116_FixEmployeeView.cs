using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Platform.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployeeView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix for Invalid object name 'dbo.hierarchy_levels'
            var sql = @"
                CREATE OR ALTER VIEW view_employee_info AS
                SELECT
                    e.id,
                    e.civil_id,
                    e.employee_id,
                    e.hierarchy_id,
                    e.aspnetusers_id,
                    u.UserName,
                    u.Email,
                    u.PhoneNumber,
                    h.name AS hierarchy_name,
                    h.name_ar AS hierarchy_name_ar,
                    hl.name AS hierarchy_level_name,
                    hl.name_ar AS hierarchy_level_name_ar
                FROM employee e
                LEFT JOIN hierarchy h ON e.hierarchy_id = h.id
                LEFT JOIN hierarchy_level hl ON h.hierarchy_level_id = hl.id
                LEFT JOIN AspNetUsers u ON e.aspnetusers_id = u.Id
            ";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Do nothing to avoid deleting existing data tables
        }
    }
}
