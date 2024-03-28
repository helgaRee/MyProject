using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<UserEntity>(options)
{

    //registrera alla extra tabeller som du själv har skapat
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<SubscribeEntity> Subscribers { get; set; }
    public DbSet<FeatureEntity> Features { get; set; }
    public DbSet<FeatureItemEntity> FeatureItems { get; set; }



    //anger detta för att lösa problem vid skapandet av ny migration
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);



        builder.Entity<CourseEntity>()
            .Property(c => c.DiscountPrice)
            .HasPrecision(18, 2);

        builder.Entity<CourseEntity>()
            .Property(c => c.LikesInNumbers)
            .HasPrecision(18, 2);

        builder.Entity<CourseEntity>()
            .Property(c => c.LikesInProcent)
            .HasPrecision(18, 2);

        builder.Entity<CourseEntity>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);


        base.OnModelCreating(builder);
    }
}
