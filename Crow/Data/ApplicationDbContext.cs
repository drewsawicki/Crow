using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Crow.Models;

namespace Crow.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Crow.Models.Bird> Bird { get; set; } = default!;
        public DbSet<Crow.Models.UserBird> UserBird { get; set; } = default!;
        public DbSet<Crow.Models.Photo> Photo { get; set; } = default!;
    }
}
