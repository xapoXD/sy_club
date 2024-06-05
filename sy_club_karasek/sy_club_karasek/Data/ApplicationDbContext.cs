using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using club.soundyard.web.Models;

namespace club.soundyard.web.Data
{
    //public class MyDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationRole> Roles { get; set; }
    }
}