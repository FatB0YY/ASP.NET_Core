using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RR_hookah.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RR_hookah.Data
{
    // создаем DbContext
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // добавление в бд
        public DbSet<Category> Category { get; set; }
       
        public DbSet<Product> Product { get; set; }
        // public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
