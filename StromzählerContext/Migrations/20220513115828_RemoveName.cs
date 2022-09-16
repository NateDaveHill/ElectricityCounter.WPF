using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StromzählerContext.Migrations
{
    public partial class RemoveName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CounterValues");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CounterValues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
