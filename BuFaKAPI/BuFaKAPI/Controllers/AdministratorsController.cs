namespace BuFaKAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using BuFaKAPI.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorsController : ControllerBase
    {
        private readonly MyContext _context;

        public AdministratorsController(MyContext context)
        {
            this._context = context;
        }

        // GET: api/Administrators
        /*
        [HttpGet]
        public IEnumerable<Administrator> GetAdministrator()
        {
            return _context.Administrator;
        }
        */

        // GET: api/Administrators/5
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdministrator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var administrators = _context.Administrator.Where(a => a.ConferenceID == id);

            if (administrators == null)
            {
                return NotFound();
            }

            return Ok(administrators);
        }
        */

        // PUT: api/Administrators/5
        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdministrator([FromRoute] string id, [FromBody] Administrator administrator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != administrator.UID)
            {
                return BadRequest();
            }

            _context.Entry(administrator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministratorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

        // POST: api/Administrators
        /*
        [HttpPost]
        public async Task<IActionResult> PostAdministrator([FromBody] Administrator administrator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Administrator.Add(administrator);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdministratorExists(administrator.UID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdministrator", new { id = administrator.UID }, administrator);
        }
        */

        // DELETE: api/Administrators/5
        /*
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministrator([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var administrator = await _context.Administrator.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }

            _context.Administrator.Remove(administrator);
            await _context.SaveChangesAsync();

            return Ok(administrator);
        }
        */

        private bool AdministratorExists(string id)
        {
            return this._context.Administrator.Any(e => e.UID == id);
        }
    }
}