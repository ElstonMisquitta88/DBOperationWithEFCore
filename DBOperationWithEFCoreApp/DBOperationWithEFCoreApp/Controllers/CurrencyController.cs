using DBOperationWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBOperationWithEFCoreApp.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public CurrencyController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllCurrencies()
    {
        var result = await _appDbContext.Currencies.Select(c => new
        {
            c.Id,
            c.Title,
            c.Description
        }).ToListAsync();
        return Ok(result);
    }

    ////api/Currency/GetCurrencyByIDAsync/1
    //[HttpGet("GetCurrencyByIDAsync/{Id}")]


    //api/Currency/1
    [HttpGet("{Id:int}")]
    public async Task<IActionResult> GetCurrencyByIDAsync([FromRoute] int Id)
    {
        //var result = await _appDbContext.Currencies.Select(c => new
        //{
        //    c.Id,
        //    c.Title,
        //    c.Description
        //}).FirstOrDefaultAsync(c => c.Id == Id);

        //Use FindAsync method to get entity by primary key
        var result = await _appDbContext.Currencies.FindAsync(Id);


        if (result == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(result);
        }
    }



    ////api/Currency/Euro/Euro
    //[HttpGet("{name},{description}")]
    //public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name, [FromRoute] string? description)
    //{
    //    // [+] both this will work the same way no Performance difference

    //    //var result1 = await _appDbContext.Currencies.Select(c => new
    //    //{
    //    //    c.Id,
    //    //    c.Title,
    //    //    c.Description
    //    //}).Where(x => x.Title == name).FirstAsync();


    //    //var result = await _appDbContext.Currencies.Select(c => new
    //    //{
    //    //    c.Id,
    //    //    c.Title,
    //    //    c.Description
    //    //}).FirstOrDefaultAsync(x => x.Title == name);

    //    var result = await _appDbContext.Currencies.Select(c => new
    //    {
    //        c.Id,
    //        c.Title,
    //        c.Description
    //    }).FirstOrDefaultAsync
    //    (x => x.Title == name
    //    && x.Description == description
    //    );

    //    // [-] both this will work the same way no Performance difference


    //    if (result == null)
    //    {
    //        return NotFound();
    //    }
    //    else
    //    {
    //        return Ok(result);
    //    }
    //}



    //api/Currency/Yen?description=From Japan




    [HttpGet("{name}")]
    public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name, [FromQuery] string? description)
    {
        // Explanation: Here we are using FirstOrDefaultAsync with a conditional filter for description.
        // If description is provided, it will be included in the filter; otherwise, it will be ignored.
        // This allows for more flexible querying based on the presence of the description parameter.

        //var result = await _appDbContext.Currencies.Select(c => new
        //{
        //    c.Id,
        //    c.Title,
        //    c.Description
        //}).FirstOrDefaultAsync
        //(
        //     x => x.Title == name
        //     && (string.IsNullOrEmpty(description) || x.Description == description) // Conditional filter for description
        //);


        // This help in performance when multiple records are expected
        // Explanation: Here we are using Where to filter records based on the provided name and optional description.
        // This approach retrieves all matching records, which is more efficient when multiple entries are expected.
        var result = await _appDbContext.Currencies.Select(c => new
        {
            c.Id,
            c.Title,
            c.Description
        }).Where
        (
             x => x.Title == name
             && (string.IsNullOrEmpty(description) || x.Description == description)
        ).ToListAsync();

        if (result == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(result);
        }
    }



    [HttpPost("ALL")]
    public async Task<IActionResult> GetCurrencyByNameAsync([FromBody] List<int> ids)
    {
        //var ids=new List<int>{1,2,3,9};
        var result = await _appDbContext.Currencies
            .Where(x => ids.Contains(x.Id))
            .Select(c => new
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description
            })
          .ToListAsync();

        return Ok(result);
    }





}
