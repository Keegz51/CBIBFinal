using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CBIB.Models;

namespace CBIB.Migrations
{
    [DbContext(typeof(CBIBContext))]
    [Migration("20170913131034_Journal-textArea")]
    partial class JournaltextArea
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CBIB.Models.Author", b =>
                {
                    b.Property<long>("AuthorID");

                    b.Property<string>("Name");

                    b.HasKey("AuthorID");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("CBIB.Models.Journal", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abstract");

                    b.Property<long>("AuthorID");

                    b.Property<string>("Title");

                    b.Property<string>("Year");

                    b.HasKey("ID");

                    b.HasIndex("AuthorID");

                    b.ToTable("Journal");
                });

            modelBuilder.Entity("CBIB.Models.Journal", b =>
                {
                    b.HasOne("CBIB.Models.Author")
                        .WithMany("Journals")
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
