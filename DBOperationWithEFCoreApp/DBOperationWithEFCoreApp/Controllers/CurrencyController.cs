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



    //api/Currency/Yen
    [HttpGet("{name:string}")]
    public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name)
    {

        var result1 = await _appDbContext.Currencies.Select(c => new
        {
            c.Id,
            c.Title,
            c.Description
        }).Where(x => x.Title == name).FirstAsync();


        var result = await _appDbContext.Currencies.Select(c => new
        {
            c.Id,
            c.Title,
            c.Description
        }).FirstAsync(x => x.Title == name);



        if (result == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(result);
        }
    }

}
