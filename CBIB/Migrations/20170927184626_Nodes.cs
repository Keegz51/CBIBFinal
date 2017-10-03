using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CBIB.Migrations
{
    public partial class Nodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NodeID",
                table: "Author",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Node",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Node", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Author_NodeID",
                table: "Author",
                column: "NodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Node_NodeID",
                table: "Author",
                column: "NodeID",
                principalTable: "Node",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_Node_NodeID",
                table: "Author");

            migrationBuilder.DropTable(
                name: "Node");

            migrationBuilder.DropIndex(
                name: "IX_Author_NodeID",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "NodeID",
                table: "Author");
        }
    }
}
