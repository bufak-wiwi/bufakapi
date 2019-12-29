using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuFaKAPI.Models;

namespace BuFaKAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly MyContext _context;

        public NewsController(MyContext context)
        {
            _context = context;
        }

        // GET: api/News
        //[HttpGet]
        //public IEnumerable<News> GetNews()
        //{
        //    return _context.News;
        //}

        // GET: api/News/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetNews([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var news = await _context.News.FindAsync(id);

        //    if (news == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(news);
        //}

        // PUT: api/News/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutNews([FromRoute] string id, [FromBody] News news)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != news.NewsID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(news).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!NewsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/News
        //[HttpPost]
        //public async Task<IActionResult> PostNews([FromBody] News news)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.News.Add(news);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetNews", new { id = news.NewsID }, news);
        //}

        // DELETE: api/News/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteNews([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var news = await _context.News.FindAsync(id);
        //    if (news == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.News.Remove(news);
        //    await _context.SaveChangesAsync();

        //    return Ok(news);
        //}

        private bool NewsExists(string id)
        {
            return _context.News.Any(e => e.NewsID == id);
        }
    }
}