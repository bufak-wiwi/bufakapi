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
    public class NewslettersController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;

        public NewslettersController(MyContext context)
        {
            _context = context;
            this.auth = new AuthService(context);
        }

        // POST: api/Newsletters
        [HttpPost]
        public async Task<IActionResult> PostNewsletter([FromBody] Newsletter newsletter, [FromQuery] string apikey)
        {
            if (auth.KeyIsValid(apikey))
            {
                if (!EmailExists(newsletter.Email))
                {
                    if (!ModelState.IsValid)
                    {
                        return this.BadRequest(ModelState);
                    }

                    _context.Newsletter.Add(newsletter);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetNewsletter", new { id = newsletter.ID }, newsletter);
                }

                return this.Ok();
            }

            return this.Unauthorized();
        }

        private bool NewsletterExists(int id)
        {
            return this._context.Newsletter.Any(e => e.ID == id);
        }

        private bool EmailExists(string email)
        {
            return this._context.Newsletter.Any(e => e.Email == email);
        }
    }
}