using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBIB.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoAuthor1",
                table: "Journal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoAuthor2",
                table: "Journal",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PeerReviewed",
                table: "Journal",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PeerUrl",
                table: "Journal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProofOfpeerReview",
                table: "Journal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Journal",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoAuthor1",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "CoAuthor2",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "PeerReviewed",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "PeerUrl",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "ProofOfpeerReview",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Journal");
        }
    }
}
