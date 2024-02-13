using Backend.Models;
using Microsoft.EntityFrameworkCore;
namespace Backend.Data;


//Application satabase context to interact with entities
public class AppDbContext : DbContext {

    //Initializing a new instance of AppDbContext class
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

    }

    //Get or Set for the collection of entities from the database
    public DbSet<User> Users { get; set; }
    public DbSet<List> Lists { get; set; }
    public DbSet<ListItem> ListItems { get; set; }
}