namespace Backend.Models;

public class ListItem {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
    public int Estimate { get; set; }
    public string Unit { get; set; }
    public string Importance { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string ListId { get; set; }
    public List List { get; set; }
}