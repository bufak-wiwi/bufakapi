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
    public class HistoriesController : ControllerBase
    {
        private readonly MyContext _context;

        public HistoriesController(MyContext context)
        {
            this._context = context;
        }

        private bool HistoryExists(int id)
        {
            return this._context.History.Any(e => e.HistoryID == id);
        }
    }
}