using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CBIB.Models;

namespace CBIB.Migrations
{
    [DbContext(typeof(CBIBContext))]
    [Migration("20171006024803_Related-Node")]
    partial class RelatedNode
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

                    b.Property<long>("NodeID");

                    b.HasKey("AuthorID");

                    b.HasIndex("NodeID");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("CBIB.Models.Journal", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abstract")
                        .IsRequired();

                    b.Property<long>("AuthorID");

                    b.Property<string>("CoAuthor1");

                    b.Property<string>("CoAuthor2");

                    b.Property<bool>("PeerReviewed");

                    b.Property<string>("PeerUrl");

                    b.Property<string>("ProofOfpeerReview");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.Property<string>("Year")
                        .IsRequired();

                    b.Property<string>("url");

                    b.HasKey("ID");

                    b.HasIndex("AuthorID");

                    b.ToTable("Journal");
                });

            modelBuilder.Entity("CBIB.Models.Node", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Node");
                });

            modelBuilder.Entity("CBIB.Models.Author", b =>
                {
                    b.HasOne("CBIB.Models.Node", "Node")
                        .WithMany("Authors")
                        .HasForeignKey("NodeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CBIB.Models.Journal", b =>
                {
                    b.HasOne("CBIB.Models.Author", "Author")
                        .WithMany("Journals")
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
