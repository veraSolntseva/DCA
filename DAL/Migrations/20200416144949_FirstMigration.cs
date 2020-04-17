using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FirstSheetItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Col1 = table.Column<string>(nullable: true),
                    Col2 = table.Column<string>(nullable: true),
                    Col3 = table.Column<string>(nullable: true),
                    Col4 = table.Column<string>(nullable: true),
                    Col5 = table.Column<string>(nullable: true),
                    Col6 = table.Column<string>(nullable: true),
                    Col7 = table.Column<string>(nullable: true),
                    Col8 = table.Column<string>(nullable: true),
                    Col9 = table.Column<string>(nullable: true),
                    Col10 = table.Column<string>(nullable: true),
                    Col11 = table.Column<string>(nullable: true),
                    Col12 = table.Column<string>(nullable: true),
                    Col13 = table.Column<string>(nullable: true),
                    Col14 = table.Column<string>(nullable: true),
                    Col15 = table.Column<string>(nullable: true),
                    Col16 = table.Column<string>(nullable: true),
                    Col17 = table.Column<string>(nullable: true),
                    Col18 = table.Column<string>(nullable: true),
                    Col19 = table.Column<string>(nullable: true),
                    Col20 = table.Column<string>(nullable: true),
                    DtAdd = table.Column<DateTime>(nullable: false),
                    DtEdit = table.Column<DateTime>(nullable: true),
                    DtDelete = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirstSheetItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SecondSheetItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Col1 = table.Column<string>(nullable: true),
                    Col2 = table.Column<string>(nullable: true),
                    Col3 = table.Column<string>(nullable: true),
                    Col4 = table.Column<string>(nullable: true),
                    Col5 = table.Column<string>(nullable: true),
                    Col6 = table.Column<string>(nullable: true),
                    Col7 = table.Column<string>(nullable: true),
                    Col8 = table.Column<string>(nullable: true),
                    Col9 = table.Column<string>(nullable: true),
                    Col10 = table.Column<string>(nullable: true),
                    Col11 = table.Column<string>(nullable: true),
                    Col12 = table.Column<string>(nullable: true),
                    Col13 = table.Column<string>(nullable: true),
                    Col14 = table.Column<string>(nullable: true),
                    Col15 = table.Column<string>(nullable: true),
                    Col16 = table.Column<string>(nullable: true),
                    Col17 = table.Column<string>(nullable: true),
                    Col18 = table.Column<string>(nullable: true),
                    Col19 = table.Column<string>(nullable: true),
                    Col20 = table.Column<string>(nullable: true),
                    DtAdd = table.Column<DateTime>(nullable: false),
                    DtEdit = table.Column<DateTime>(nullable: true),
                    DtDelete = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondSheetItems", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FirstSheetItems");

            migrationBuilder.DropTable(
                name: "SecondSheetItems");
        }
    }
}
