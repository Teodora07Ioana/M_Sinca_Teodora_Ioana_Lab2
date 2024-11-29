using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using M_Sinca_Teodora_Ioana_Lab2.Models;

namespace LibraryWebAPI.Data
{
    public class LibraryWebAPIContext : DbContext
    {
        public LibraryWebAPIContext (DbContextOptions<LibraryWebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<M_Sinca_Teodora_Ioana_Lab2.Models.Customer> Customer { get; set; } = default!;
    }
}
