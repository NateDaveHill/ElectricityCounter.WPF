using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StromzählerContext.Migrations
{
    public partial class CounterValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Counters");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Counters");

            migrationBuilder.CreateTable(
                name: "CounterValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    CounterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounterValues_Counters_CounterId",
                        column: x => x.CounterId,
                        principalTable: "Counters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CounterValues_CounterId",
                table: "CounterValues",
                column: "CounterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CounterValues");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Counters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Counters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
