using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace Backend.Data;


//Application satabase context to interact with entities
public class AppDbContext : IdentityDbContext<User, IdentityRole<string>, string> {

    //Initializing a new instance of AppDbContext class
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

    }

    //Get or Set for the collection of entities from the database
    // public DbSet<User> Users { get; set; }
    public DbSet<List> Lists { get; set; }
    public DbSet<ListItem> ListItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        var keyProperties = modelBuilder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x.Properties);
        foreach (var property in keyProperties)
            property.ValueGenerated = ValueGenerated.OnAdd;

        // modelBuilder.Entity<User>(entity => { entity.ToTable("Users"); });
        // modelBuilder.Entity<IdentityRole<string>>(entity => { entity.ToTable("Roles"); });

        modelBuilder.Entity<ListItem>()
        .HasKey(li => li.Id);

        modelBuilder.Entity<ListItem>()
        .Property(li => li.Id)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<ListItem>()
        .HasOne(li => li.User)
        .WithMany(u => u.ListItems)
        .HasForeignKey(li => li.UserId);

        modelBuilder.Entity<ListItem>()
        .HasOne(li => li.List)
        .WithMany(l => l.ListItems)
        .HasForeignKey(li => li.ListId);
    }
}