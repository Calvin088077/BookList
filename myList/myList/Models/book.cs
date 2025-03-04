namespace myList.Models;

public partial class book
{
    public int id { get; set; }

    public string book_name { get; set; } = null!;

    public bool is_read { get; set; }

    public string? notes { get; set; }

    public DateTime? date_added { get; set; }
}
