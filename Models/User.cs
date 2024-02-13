namespace Backend.Models;
using Microsoft.AspNetCore.Identity;

public class User: IdentityUser<int> {
    override public int Id { get; set; }
    public string? Name { get; set; }
    override public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Avatar { get; set; }

    public ICollection<ListItem> ListItems { get; set; } = new List<ListItem>();
}