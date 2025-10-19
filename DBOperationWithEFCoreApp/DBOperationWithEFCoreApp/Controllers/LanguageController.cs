using DBOperationWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBOperationWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public LanguageController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllLanguages()
        {
            var result = await _appDbContext.Languages.Select(l => new
            {
                l.Id,
                l.Title,
                l.Description
            }).ToListAsync();
            return Ok(result);
        }

        [HttpGet("GetAllLanguages1")]
        public async Task<IActionResult> GetAllLanguages1()
        {
            var result = await _appDbContext.Languages.Select(l => new
            {
                l.Id,
                l.Title,
                l.Description
            }).ToListAsync();
            return Ok(result);
        }
    }
}
