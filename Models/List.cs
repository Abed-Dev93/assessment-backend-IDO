namespace Backend.Models;

public class List {
    public string Id { get; set; }
    public string? Title { get; set; }

    public ICollection<ListItem> ListItems { get; set; } = new List<ListItem>();
}