namespace DBOperationWithEFCoreApp.Data;

public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int NoofPages { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }


    // This will create Foreign Key Relationship with Language Table
    public int LanguageID { get; set; }

    public Language Language { get; set; }
}
