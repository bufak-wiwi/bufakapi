namespace BuFaKAPI.Services
{
    using System;
    using BuFaKAPI.Models;
    using Microsoft.EntityFrameworkCore;
    using WebApplication1.Models;

    public class AuthService
    {
        private readonly TelegramBot telBot;
        private readonly MyContext _context;

        public AuthService(MyContext context)
        {
            this._context = context;
            this.telBot = new TelegramBot();
        }

        public bool KeyIsValid(string apikey, int conference_id)
        {
            var auth = this._context.Auth.FirstAsync(m => m.ApiKey == apikey).Result;
            if (auth == null)
            {
                this.telBot.SendTextMessage($"API Key {apikey} not found");
                return false;
            }
            else if (DateTime.Parse(auth.ValidUntil) < DateTime.Now)
            {
                this.telBot.SendTextMessage($"API Key {apikey} Not Valid anymore");
                return false;
            }

            if (auth.ConferenceID == conference_id || auth.ConferenceID == 999)
            {
                return true;
            }

            this.telBot.SendTextMessage($"Something went wrong with API Key {apikey}");
            return false;
        }

        public bool KeyIsValid(string apikey)
        {
            var bot = new TelegramBot();
            var auth = this._context.Auth.FirstOrDefaultAsync(m => m.ApiKey == apikey).Result;
            if (auth == null)
            {
                bot.SendTextMessage($"API Key {apikey} not Found");
                return false;
            }
            else if (DateTime.Parse(auth.ValidUntil) < DateTime.Now)
            {
                bot.SendTextMessage($"API KEy {apikey} Not Valid anymore");
                return false;
            }

            return true;
        }

        public bool IsSuperAdmin(string uid)
        {
            User user = this._context.User.FindAsync(uid).Result;
            if (user != null && user.IsSuperAdmin)
            {
                return true;
            }

            return false;
        }
    }
}
