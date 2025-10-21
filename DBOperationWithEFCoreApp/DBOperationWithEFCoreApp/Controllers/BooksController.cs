using DBOperationWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBOperationWithEFCoreApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(AppDbContext _appDbContext) : ControllerBase
{

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


    [HttpGet]
    public async Task<ActionResult<Book>> GetAllBooks()
    {
        var books = await _appDbContext.Books
            .Include(b => b.Language) // Include must come before Select
            .Select(b => new
            {
                b.Id,
                b.Title,
                b.Description,
                b.NoofPages,
                b.IsActive,
                b.CreatedOn,
                b.LanguageID,
                Language = b.Language == null ? null : new
                {
                    b.Language.Id,
                    b.Language.Title,
                    b.Language.Description
                }
            })
            .ToListAsync();
        return Ok(books);
    }




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
            //existingBook.NoofPages = bookmodel.NoofPages;
            //existingBook.IsActive = bookmodel.IsActive;
            //existingBook.LanguageID = bookmodel.LanguageID;
            await _appDbContext.SaveChangesAsync();
            return Ok(bookmodel);
        }
    }
}
