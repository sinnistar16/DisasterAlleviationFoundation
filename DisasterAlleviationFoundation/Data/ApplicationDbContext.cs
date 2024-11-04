using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<disasters> disasters { get; set; }
        public DbSet<good_donations> good_donations { get; set; }
        public DbSet<monetary> monetary{ get; set; }
        public DbSet<purchased_goods> purchased_goods { get; set; }
        public DbSet<funds> funds { get; set; }
    }
}