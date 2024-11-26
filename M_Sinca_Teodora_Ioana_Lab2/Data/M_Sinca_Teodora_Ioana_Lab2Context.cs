using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using M_Sinca_Teodora_Ioana_Lab2.Models;

namespace M_Sinca_Teodora_Ioana_Lab2.Data
{
    public class M_Sinca_Teodora_Ioana_Lab2Context : DbContext
    {
        public M_Sinca_Teodora_Ioana_Lab2Context (DbContextOptions<M_Sinca_Teodora_Ioana_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<M_Sinca_Teodora_Ioana_Lab2.Models.Book> Book { get; set; } = default!;

        public DbSet<M_Sinca_Teodora_Ioana_Lab2.Models.Author> Author { get; set; } = default!;
        public DbSet<M_Sinca_Teodora_Ioana_Lab2.Models.Customer> Customer { get; set; } = default!;
        public DbSet<M_Sinca_Teodora_Ioana_Lab2.Models.Genre> Genre { get; set; } = default!;

        public DbSet<M_Sinca_Teodora_Ioana_Lab2.Models.Order> Order { get; set; } = default!;

        public DbSet<M_Sinca_Teodora_Ioana_Lab2.Models.Publisher> Publisher { get; set; } = default!;
        public DbSet<M_Sinca_Teodora_Ioana_Lab2.Models.PublishedBook> PublishedBook { get; set; } = default!;
    }
}

