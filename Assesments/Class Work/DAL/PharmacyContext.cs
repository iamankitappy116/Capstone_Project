using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace DAL
{
    public class PharmacyContext : DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options) : base(options)
        {

        }
        public DbSet<Pharmacy> pharmacies { get; set; }
        public DbSet<Medicine> medicines { get; set; }

    }
}
