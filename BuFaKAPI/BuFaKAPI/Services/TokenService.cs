namespace BuFaKAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class TokenService
    {
        private readonly string jwtkey;
        private readonly string firebaseapikey;
        private readonly SymmetricSecurityKey securityKey;
        private readonly JwtSecurityTokenHandler handler;
        private readonly TelegramBot telBot;

        public TokenService(IOptions<AppSettings> settings)
        {
            this.jwtkey = settings.Value.JwtKey;
            this.firebaseapikey = settings.Value.FirebaseApiKey;
            this.securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtkey));
            this.handler = new JwtSecurityTokenHandler();
            this.telBot = new TelegramBot();
        }

        public string CreateKey(string uid)
        {
            var credentials = new SigningCredentials(this.securityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            int unixTimestamp = (int)DateTime.UtcNow.AddMinutes(60).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var payload = new JwtPayload
           {
               { "uid", uid },
               { "exp", unixTimestamp },
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
                throw new SecurityTokenExpiredException();
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

        public bool JwtTokenIsValid(string token, string role)
        {
            return false;
        }
    }
}
