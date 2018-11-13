using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RIMS.Models;

namespace RIMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Incubator> Incubators { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<EggType> EggTypes { get; set; }
        public DbSet<Tray> Trays { get; set; }
    }
}
