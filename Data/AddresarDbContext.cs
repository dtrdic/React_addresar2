using Adressar_TestApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adressar_TestApp.Data
{
    public class AddresarDbContext : DbContext
    {
        public DbSet<Person> Person { get; set; }

        public AddresarDbContext(DbContextOptions<AddresarDbContext> options) : base(options)
        {
        }
    }
}
