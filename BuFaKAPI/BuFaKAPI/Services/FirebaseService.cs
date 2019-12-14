namespace BuFaKAPI.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using Firebase.Auth;
    using Microsoft.Extensions.Options;

    public class FirebaseService
    {
        private static readonly HttpClient Client = new HttpClient();
        private readonly string apikey;
        private readonly FirebaseAuthProvider authProvider;
        private readonly MyContext _context;

        public FirebaseService(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.apikey = settings.Value.FirebaseApiKey;
            this.authProvider = new FirebaseAuthProvider(new FirebaseConfig(this.apikey));
        }

        public async Task<bool> PasswordReset(string email)
        {
            await this.authProvider.SendPasswordResetEmailAsync(email);
            return true;
        }

        public async Task<bool> SendVerificationEmail(string token)
        {
            await this.authProvider.SendEmailVerificationAsync(token);
            return true;
        }

        public async Task<string> EmailChange(string email, string password, string newemail)
        {
            // throw new NotImplementedException();
            var token = this.authProvider.SignInWithEmailAndPasswordAsync(email, password).Result.FirebaseToken;
            var values = new Dictionary<string, string>
            {
                { "idToken", token },
                { "email", newemail }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await Client.PostAsync($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/setAccountInfo?key={this.apikey}", content);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

            // this.authProvider.SendEmailVerificationAsync(token);
                return responseString;
            }
            else if ((int)response.StatusCode == 302)
            {
                return "302";
            }
            else
            {
                return null;
            }
        }

        public async Task<string> PasswordChange(string email, string password, string newpassword)
        {
            // throw new NotImplementedException();
            var token = this.authProvider.SignInWithEmailAndPasswordAsync(email, password).Result.FirebaseToken;
            var values = new Dictionary<string, string>
            {
                { "idToken", token },
                { "password", newpassword }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await Client.PostAsync($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/setAccountInfo?key={this.apikey}", content);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return responseString;
            }
            else if ((int)response.StatusCode == 302)
            {
                return "302";
            }
            else
            {
                return null;
            }
        }

        public async void DeleteUser(string uid, string password)
        {
            await this.authProvider.DeleteUserAsync(this.authProvider.SignInWithEmailAndPasswordAsync(this._context.User.Find(uid).Email, password).Result.FirebaseToken);
        }

        public string CreateCustomFBKey(string uid)
        {
            var customToken = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid);
            return customToken.Result;
        }
    }
}
