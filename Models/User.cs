namespace Backend.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class User: IdentityUser<string> {
    public string Password { get; set; } = "Abed-0000";
    public string Name { get; set; } = "abed";
    public string Avatar { get; set; } = "https://picsum.photos/seed/picsum/200/300";

    public ICollection<ListItem> ListItems { get; set; } = new List<ListItem>();
}