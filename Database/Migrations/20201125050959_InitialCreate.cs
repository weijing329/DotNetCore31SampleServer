using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNetCore31SampleServer.Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestTypeBoolean = table.Column<bool>(nullable: false),
                    TestTypeStringMax = table.Column<string>(nullable: true),
                    TestTypeString255 = table.Column<string>(maxLength: 255, nullable: true),
                    TestTypeInt16 = table.Column<short>(nullable: false),
                    TestTypeInt32 = table.Column<int>(nullable: false),
                    TestTypePeriod = table.Column<Period>(nullable: true),
                    CreatedAt = table.Column<Instant>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<Instant>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRecords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestRecords");
        }
    }
}
