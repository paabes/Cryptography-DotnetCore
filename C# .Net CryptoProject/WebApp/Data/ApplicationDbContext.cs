using System;
using Domain;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DiffeDB> DiffeDbs { get; set; }
        public DbSet<RsaDb> RsaDbs { get; set; }
        public DbSet<RsaDecDb> RsaDecDbs { get; set; }
    }
}