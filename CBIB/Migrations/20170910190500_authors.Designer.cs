using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CBIB.Models;

namespace CBIB.Migrations
{
    [DbContext(typeof(CBIBContext))]
    [Migration("20170910190500_authors")]
    partial class authors
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CBIB.Models.Author", b =>
                {
                    b.Property<long>("AuthorID");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.HasKey("AuthorID");

                    b.ToTable("Author");
                });
        }
    }
}
