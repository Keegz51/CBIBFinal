using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CBIB.Models;

namespace CBIB.Models
{
    public class CBIBContext : DbContext
    {
        public CBIBContext (DbContextOptions<CBIBContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Author { get; set; }

        public DbSet<Journal> Journal { get; set; }

        public DbSet<CBIB.Models.Node> Node { get; set; }
    }
}
