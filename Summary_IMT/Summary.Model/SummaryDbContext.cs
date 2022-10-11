using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Summary.Model.Models;

namespace Summary.Model
{
    public class SummaryDbContext : DbContext
    {
        public SummaryDbContext() : base("SummaryConnection")
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderDetail> OrderDetails { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Post> Posts { set; get; }
        public DbSet<PostCategory> PostCategories { set; get; }
        public DbSet<PostTag> PostTags { set; get; }
        public DbSet<Product> Products { set; get; }

        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<SupportOnline> SupportOnlines { set; get; }
        public DbSet<SystemConfig> SystemConfigs { set; get; }

        public DbSet<Tag> Tags { set; get; }

        public DbSet<VisitorStatistic> VisitorStatistics { set; get; }
        public DbSet<Error> Errors { set; get; }
        public DbSet<ContactDetail> ContactDetails { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }

        public DbSet<Function> Functions { set; get; }
        //public DbSet<Permission> Permissions { set; get; }
        //public DbSet<AppRole> AppRoles { set; get; }
        //public DbSet<IdentityUserRole> UserRoles { set; get; }


        public DbSet<Color> Colors { set; get; }
        public DbSet<Size> Sizes { set; get; }
        public DbSet<ProductQuantity> ProductQuantities { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }

        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }

        public static SummaryDbContext Create()
        {
            return new SummaryDbContext();
        }

    }
}
