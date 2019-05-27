using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend6.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Backend6.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PostCategory> PostCategories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostComment> PostComments { get; set; }

        public DbSet<PostAttachment> PostAttachments { get; set; }

        public DbSet<CarBrand> CarBrands { get; set; }

        public DbSet<CarModel> CarModels { get; set; }

        public DbSet<CarGrade> CarGrades { get; set; }

        public DbSet<CarPart> CarParts { get; set; }

        public DbSet<CarDetail> CarDetails { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>().HasOne(x => x.Creator).WithMany().OnDelete(DeleteBehavior.Restrict);
            builder.Entity<PostComment>().HasOne(x => x.Creator).WithMany().OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
