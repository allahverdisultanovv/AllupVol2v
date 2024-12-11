using AllupVol2.Models;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



        public DbSet<Slide> Slides { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }



    }
}
