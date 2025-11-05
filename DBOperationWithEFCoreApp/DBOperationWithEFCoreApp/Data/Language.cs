namespace DBOperationWithEFCoreApp.Data;

public class Language
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }


    // Navigation Property
    //public ICollection<Book> Books { get; set; }
}
