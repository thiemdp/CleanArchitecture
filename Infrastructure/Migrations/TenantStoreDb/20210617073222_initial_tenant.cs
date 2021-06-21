using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorHero.CleanArchitecture.Infrastructure.Migrations.TenantStoreDb
{
    public partial class initial_tenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatabaseType = table.Column<int>(type: "int", nullable: false),
                    TenantLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantInfo", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TenantInfo",
                columns: new[] { "Id", "ConnectionString", "DatabaseType", "Identifier", "IsActive", "Name", "TenantLogo" },
                values: new object[] { "Master", null, 0, "Master", false, "Master", null });

            migrationBuilder.CreateIndex(
                name: "IX_TenantInfo_Identifier",
                table: "TenantInfo",
                column: "Identifier",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantInfo");
        }
    }
}
