using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data
{
    public class GalleryContext : IdentityDbContext<ApplicationUser>
    {
        public GalleryContext(DbContextOptions<GalleryContext> options) : base(options)
        {
        }

        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Artworks)
                .WithMany();
        }
    }
}
