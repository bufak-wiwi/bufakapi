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
    public class VotingMajoritiesController : ControllerBase
    {
        private readonly MyContext _context;

        public VotingMajoritiesController(MyContext context)
        {
            _context = context;
        }

        // GET: api/VotingMajorities
        [HttpGet]
        public IEnumerable<VotingMajority> GetVotingMajority()
        {
            return _context.VotingMajority;
        }

        private bool VotingMajorityExists(int id)
        {
            return _context.VotingMajority.Any(e => e.MajorityID == id);
        }
    }
}