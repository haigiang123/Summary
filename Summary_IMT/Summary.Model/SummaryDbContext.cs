using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Summary.Model.Models;

namespace Summary.Model
{
    public class SummaryDbContext : IdentityDbContext<AppUser>
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

        public DbSet<Color> Colors { set; get; }
        public DbSet<Size> Sizes { set; get; }
        public DbSet<ProductQuantity> ProductQuantities { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }

        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }

        public DbSet<AppUserImage> AppUserImages { get; set; }

        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppPermission> AppPermissions { get; set; }
        public DbSet<AppRolePermission> AppRolePermissions { get; set; }
        public DbSet<IdentityUserRole> AppUserRoles { set; get; }


        // create for Identity
        public static SummaryDbContext Create()
        {
            return new SummaryDbContext();
        }

        // Set key for Identity
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityUserRole>().HasKey(x => new { x.RoleId, x.UserId }).ToTable("AppUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(x => x.UserId).ToTable("AppLogins");
            builder.Entity<IdentityRole>().HasKey(x => x.Id).ToTable("AppRoles");
            builder.Entity<IdentityUserClaim>().HasKey(x => new { x.Id, x.UserId }).ToTable("AppUserClaims");
        }

    }
}
