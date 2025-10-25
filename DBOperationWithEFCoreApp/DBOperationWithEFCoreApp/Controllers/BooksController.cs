using DBOperationWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBOperationWithEFCoreApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(AppDbContext _appDbContext) : ControllerBase
{

    #region GETMethods



    [HttpGet]
    public async Task<ActionResult<Book>> GetAllBooksAync()
    {
        // Include not required here as we are projecting the Author data explicitly

        /*
         * When you write:
            x.Author.Name
            inside the projection, EF knows it must join the Author table to get that column value.
            So it automatically generates SQL similar to:
            SELECT b.Id, b.Title, b.Description, b.NoofPages, a.Name, a.Email
            FROM Books AS b
            LEFT JOIN Authors AS a ON b.AuthorId = a.Id;
            That’s why .Include() is not needed here — the data is already explicitly fetched in your projection.
         */
        var book = await _appDbContext.Books
            .Select(x => new Book() // Projection to Book object - Avoid using Anonymous Object . Good for API response consistency
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                NoofPages = x.NoofPages,
                Author = x.Author == null ? null : new Author
                {
                    Name = x.Author.Name,
                    Email = x.Author.Email
                },
                Language = x.Language == null ? null : new Language
                {
                    Title = x.Language.Title,
                    Description = x.Language.Description
                }
            })
            .ToListAsync();

        return Ok(book);
    }




    #endregion

    #region ADDMethods

    [HttpPost]
    public async Task<IActionResult> AddNewBook([FromBody] Book bookmodel)
    {
        await _appDbContext.Books.AddAsync(bookmodel); // Just adds to the tracking, not yet saved to DB
        await _appDbContext.SaveChangesAsync(); // Now it saves to the database

        // Return the added book with its generated ID
        // Object ID will be generated after SaveChangesAsync.
        return Ok(bookmodel);
    }



    [HttpPost("Bulk")]
    public async Task<IActionResult> AddBooks([FromBody] List<Book> bookmodel)
    {
        await _appDbContext.Books.AddRangeAsync(bookmodel); // Just adds to the tracking, not yet saved to DB
        await _appDbContext.SaveChangesAsync(); // Now it saves to the database

        // Return the added book with its generated ID
        // Object ID will be generated after SaveChangesAsync.
        return Ok(bookmodel);
    }


    //Adding Record with Related Data
    [HttpPost("AddwithAuthor")]
    public async Task<IActionResult> AddwithAuthor([FromBody] Book bookmodel)
    {
        //var author = new Author()
        //{
        //    Name = "Test_Author",
        //    Email = "Test_Author@gmail.com"
        //};

        //bookmodel.Author = author;

        await _appDbContext.Books.AddAsync(bookmodel); // Just adds to the tracking, not yet saved to DB
        await _appDbContext.SaveChangesAsync(); // Now it saves to the database

        // Return the added book with its generated ID
        // Object ID will be generated after SaveChangesAsync.
        return Ok(bookmodel);
    }

    #endregion

    #region UPDATEMethods

    // Update Book Record - However, there are 2 database hits happening here

    //(1) to fetch the existing record and (2) to save the updated record.

    [HttpPut("{Bookid}")]
    public async Task<IActionResult> UpdateBook([FromRoute] int Bookid, [FromBody] Book bookmodel)
    {

        //Check if book exists
        var existingBook = await _appDbContext.Books.FindAsync(Bookid);
        if (existingBook == null)
        {
            return NotFound($"Book with ID {Bookid} not found.");
        }
        else
        {
            existingBook.Title = bookmodel.Title;
            existingBook.Description = bookmodel.Description;
            await _appDbContext.SaveChangesAsync();
            return Ok(bookmodel);
        }
    }



    // Update Book Record - Single Database Hit
    [HttpPut("UpdateBookWithSingleQuery")]
    public async Task<IActionResult> UpdateBookWithSingleQuery([FromBody] Book bookmodel)
    {
        _appDbContext.Update(bookmodel);
        await _appDbContext.SaveChangesAsync();
        return Ok(bookmodel);
    }




    // Update Book Record in Bulk - Single Database Hit
    [HttpPut("UpdateBooksInBulk")]
    public async Task<IActionResult> UpdateBooksInBulk()
    {
        // (A) This is slower way as it fetches records first and then updates
        // Change Tracker used here
        /*
            _appDbContext.Books.UpdateRange(
                new Book { Id = 1, Title = "Bulk Updated Title 1", Description = "Bulk Updated Description 1" },
                new Book { Id = 2, Title = "Bulk Updated Title 2", Description = "Bulk Updated Description 2" }
            );
        */

        // (B) This is faster way as it directly updates records in DB
        // Change Tracker not used here


        //await _appDbContext.Books
        //    .Where(x=>x.IsActive==true)
        //    .ExecuteUpdateAsync(setters => setters
        //    .SetProperty(b => b.Title,  "Updated Title Bulk")
        //    .SetProperty(b => b.Description,  "Updated Description Bulk")
        //);

        var _valtoupdate = new List<int> { 1007, 1008, 1009 };
        await _appDbContext.Books
          .Where(x => _valtoupdate.Contains(x.Id))
          .ExecuteUpdateAsync(setters => setters
          .SetProperty(b => b.Title, "Updated Title Bulk V7")
          .SetProperty(b => b.Description, "Updated Description Bulk V7")
      );
        return Ok();
    }

    #endregion

    #region DELETEMethods

    // Delete with Single Database Hit
    [HttpDelete("{BookId}")]
    public async Task<IActionResult> DeleteBookByIDAsync([FromRoute] int BookId)
    {
        var book = await _appDbContext.Books.FindAsync(BookId);
        if (book == null)
        {
            return NotFound($"Book with ID {BookId} not found.");
        }
        else
        {
            _appDbContext.Books.Remove(book); // Marks the entity for deletion
            await _appDbContext.SaveChangesAsync(); // Executes the deletion in the database
        }
        return Ok($"Book with ID {BookId} Deleted.");
    }



    [HttpDelete("DeleteBookinBulkAsync")]
    public async Task<IActionResult> DeleteBookinBulkAsync()
    {
        // (A) This is slower way as it fetches records first and then deletes
        // Query hits the database as per the number of records to be deleted

        //var books = await _appDbContext.Books.Where(x => x.Id < 1009).ToListAsync();
        //_appDbContext.Books.RemoveRange(books);
        //await _appDbContext.SaveChangesAsync();


        // (B) This is faster way as it directly deletes records in DB
        // Single Query to database
        // No tracking used here
        var books = await _appDbContext.Books.Where(x => x.Id == 1008).ExecuteDeleteAsync();
        return Ok();
    }


    #endregion


}
