using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Data
{
    // Inherit from IdentityDbContext to include ASP.NET Identity support.
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSet for your custom models
        public DbSet<Disaster> Disasters { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }

        // Customize the model and schema for Identity and custom models if needed
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // You can add custom configurations here if needed
            // Example: Renaming ASP.NET Identity table names (optional)
            builder.Entity<IdentityUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });

            // Additional custom configuration for Disaster, Donation, and Volunteer models
            builder.Entity<Disaster>().HasKey(d => d.Id); // Make sure Id is the primary key
            builder.Entity<Donation>().HasKey(d => d.Id);
            builder.Entity<Volunteer>().HasKey(v => v.Id);
        }
    }
}
