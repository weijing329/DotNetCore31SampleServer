using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace DotNetCore31SampleServer.Database.Migrations
{
    public partial class AddWorkaroundForLastUpdatedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Instant>(
                name: "LastUpdated",
                table: "TestRecords",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(Instant),
                oldType: "timestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Instant>(
                name: "LastUpdated",
                table: "TestRecords",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(Instant),
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
