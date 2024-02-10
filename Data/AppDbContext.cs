using Backend.Models;
using Microsoft.EntityFrameworkCore;
namespace Backend.Data;

public class AppDbContext : DbContext {

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<List> Lists { get; set; }
    public DbSet<ListItem> ListItems { get; set; }
}