using DBOperationWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBOperationWithEFCoreApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(AppDbContext _appDbContext) : ControllerBase
{

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
    public async Task<IActionResult> GetAllBooks()
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

}
