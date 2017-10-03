using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBIB.Migrations
{
    public partial class NodeFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_Node_NodeID",
                table: "Author");

            migrationBuilder.AlterColumn<long>(
                name: "NodeID",
                table: "Author",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Node_NodeID",
                table: "Author",
                column: "NodeID",
                principalTable: "Node",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_Node_NodeID",
                table: "Author");

            migrationBuilder.AlterColumn<long>(
                name: "NodeID",
                table: "Author",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Node_NodeID",
                table: "Author",
                column: "NodeID",
                principalTable: "Node",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
