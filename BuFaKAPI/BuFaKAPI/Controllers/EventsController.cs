namespace BuFaKAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly MyContext _context;

        public EventsController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Events
        //[HttpGet]
        //public IEnumerable<Event> GetEvent()
        //{
        //    return _context.Event;
        //}

        // GET: api/Events/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetEvent([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var @event = await _context.Event.FindAsync(id);

        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(@event);
        //}

        // PUT: api/Events/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEvent([FromRoute] string id, [FromBody] Event @event)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != @event.EventID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(@event).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EventExists(id))
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

        // POST: api/Events
        //[HttpPost]
        //public async Task<IActionResult> PostEvent([FromBody] Event @event)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Event.Add(@event);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEvent", new { id = @event.EventID }, @event);
        //}

        // DELETE: api/Events/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEvent([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var @event = await _context.Event.FindAsync(id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Event.Remove(@event);
        //    await _context.SaveChangesAsync();

        //    return Ok(@event);
        //}

        private bool EventExists(string id)
        {
            return _context.Event.Any(e => e.EventID == id);
        }
    }
}