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
    public class PlacesController : ControllerBase
    {
        private readonly MyContext _context;

        public PlacesController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Places
        //[HttpGet]
        //public IEnumerable<Place> GetPlace()
        //{
        //    return _context.Place;
        //}

        // GET: api/Places/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetPlace([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var place = await _context.Place.FindAsync(id);

        //    if (place == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(place);
        //}

        // PUT: api/Places/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPlace([FromRoute] string id, [FromBody] Place place)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != place.PlaceID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(place).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PlaceExists(id))
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

        // POST: api/Places
        //[HttpPost]
        //public async Task<IActionResult> PostPlace([FromBody] Place place)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Place.Add(place);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPlace", new { id = place.PlaceID }, place);
        //}

        // DELETE: api/Places/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePlace([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var place = await _context.Place.FindAsync(id);
        //    if (place == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Place.Remove(place);
        //    await _context.SaveChangesAsync();

        //    return Ok(place);
        //}

        private bool PlaceExists(string id)
        {
            return _context.Place.Any(e => e.PlaceID == id);
        }
    }
}