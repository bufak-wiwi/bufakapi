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

        private bool AdministratorExists(string id)
        {
            return this._context.Administrator.Any(e => e.UID == id);
        }
    }
}