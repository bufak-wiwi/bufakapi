namespace BuFaKAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using WebApplication1.Models;

    public class TokenService
    {
        private readonly string jwtkey;
        private readonly string firebaseapikey;
        private readonly MyContext _context;
        private readonly SymmetricSecurityKey securityKey;
        private readonly JwtSecurityTokenHandler handler;
        private readonly TelegramBot telBot;

        public TokenService(MyContext context, IOptions<AppSettings> settings)
        {
            this.jwtkey = settings.Value.JwtKey;
            this._context = context;
            this.firebaseapikey = settings.Value.FirebaseApiKey;
            this.securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtkey));
            this.handler = new JwtSecurityTokenHandler();
            this.telBot = new TelegramBot();
        }

        public string CreateKey(string uid, int councilID)
        {
            var credentials = new SigningCredentials(this.securityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            int unixTimestamp = (int)DateTime.UtcNow.AddDays(3).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var payload = new JwtPayload
           {
               { "uid", uid },
               { "exp", unixTimestamp },
               { "councilid",  councilID },
           };
            var secToken = new JwtSecurityToken(header, payload);
            return this.handler.WriteToken(secToken);
        }

        public bool ValidateJwtKey(string key)
        {
            var handler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                IssuerSigningKey = this.securityKey,
                ValidateAudience = false,
                ValidateIssuer = false,
            };
            try
            {
                handler.ValidateToken(key, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                // this.telBot.sendTextMessage($"JWTManager > GetPrincipal: {ex}");
                // throw new SecurityTokenExpiredException();
                return false;
            }
            catch (Exception e)
            {
                this.telBot.SendTextMessage($"New Exception {e} found at validation of Token");
                return false;
            }

            return true;
        }

        public string GetUIDfromJwtKey(string key)
        {
            var token = this.handler.ReadJwtToken(key);
            var uid = token.Claims.First(claim => claim.Type == "uid").Value;
            return uid;
        }

        public int GetCouncilfromJwtKey(string key)
        {
            var token = this.handler.ReadJwtToken(key);
            return int.Parse(token.Claims.First(claim => claim.Type == "councilid").Value);
        }

        public string GenerateApiKey()
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(32);
            Random random = new Random();
            for (int i = 0; i < 32; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }

        public bool PermissionLevelValid(string token, string level)
        {
            string uid = this.GetUIDfromJwtKey(token);

            if (level == "user")
            {
                return this.ValidateJwtKey(token);
            }
            else if (level == "admin")
            {
                return this.ValidateJwtKey(token) && this._context.Administrator.Any(a => a.UID == uid);
            }
            else if (level == "superadmin")
            {
                User user = this._context.User.Find(uid);
                if (this.ValidateJwtKey(token) && user.IsSuperAdmin)
                {
                    this._context.Entry(user).State = EntityState.Detached;
                    return true;
                }

                this._context.Entry(user).State = EntityState.Detached;
                return false;
            }

            return false;
        }
    }
}
