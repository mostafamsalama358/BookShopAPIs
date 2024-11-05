using Domains;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MailKit.Security;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using Domains.DTOS.ForLogin;
using static Domains.DTOS.ForLogin.PasswordRecoveryDto;

namespace Bl.Repos.Account
{
    public class Account : IAccount
    {
        BookShopContextAPIs _context;
        public IConfiguration _configuration;
        public UserManager<AppUser> UserManager;
        IEmailServices _emailServices;

        public Account(BookShopContextAPIs _context, IEmailServices _emailServices, IConfiguration configuration, UserManager<AppUser> UserManager)
        {
            this._context = _context;
            _configuration = configuration;
            this.UserManager = UserManager;
            this._emailServices = _emailServices;
        }

        //------------------------------------------------------------------------------------------------------------
        public string GenerateJwtToken(AppUser user)
        {
            var claims = GetUserClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IEnumerable<Claim> GetUserClaims(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = UserManager.GetRolesAsync(user).Result;
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        //------------------------------------------------------------------------------------------------------------





        //------------------------------------------------------------------------------------------------------------

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
        public async Task<string> GetGmailAccessTokenAsync()
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var clientSecrets = new ClientSecrets
            {
                ClientId = emailSettings["ClientId"],
                ClientSecret = emailSettings["ClientSecret"]
            };

            //var codeReceiver = new LocalServerCodeReceiver(); // This handles local redirect URI automatically
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = new[] { "https://mail.google.com/" },
                DataStore = new FileDataStore("TokenStore") // for token persistence
            };

            // LocalServerCodeReceiver automatically handles localhost redirects
            var codeReceiver = new LocalServerCodeReceiver();

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                new[] { "https://mail.google.com/" },
                "user",
                CancellationToken.None,
                new FileDataStore("TokenStore"),
                codeReceiver
            );
            return credential.Token.AccessToken;
        }
        
        
       // ------------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------------

        public string GenerateOtp(AppUser user)
        {
            // Generate a 6-digit OTP
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            // Optionally, store the OTP and its expiration time
            // StoreOtpAsync(user.Email, otp);

            return otp;
        }

        private static readonly Dictionary<string, (string Otp, DateTime Expiration)> OtpStore = new();

        public async Task StoreOtpAsync(string email, string otp, string name)
        {
            // Set the OTP expiration time (e.g., 5 minutes)
            var expiration = DateTime.Now.AddMinutes(5);
            OtpStore[email] = (otp, expiration);

            // You may want to store it in a more persistent way, like a database.
            var mailrequest = new MailRequest();
            mailrequest.Email = email;
            mailrequest.Subject = "Thank for Registering : OTP";
            mailrequest.EmailBody = GenerateEmailBody(name, otp);
            await _emailServices.SendEmail(mailrequest);
        }
        public string GenerateEmailBody(string name, string otptext)
        {
            string emailbody = "<div style='width:100%;background-color:grey'>";
            emailbody += "<h1>Hi " + name + ", Thank You fOR Registeration</h1> ";
            emailbody += "<h2> please enter OTP text and complete the registration </h2>";
            emailbody += "<h2>OTP Text Is : " + otptext + "</h2>";
            emailbody += "</div>";
            return emailbody;
        }
        public string GenerateEmailBodyreqest(string name, string otptext)
        {
            string emailbody = "<div style='width:100%;background-color:grey'>";
            emailbody += "<h1>Hi " + name + ", Thank You </h1> ";
            emailbody += "<h2> Your Token Is : " + otptext + "</h2>";
            emailbody += "</div>";
            return emailbody;
        }


        public async Task<bool> ValidateOtpAsync(string email, string otp)
        {

            if (OtpStore.TryGetValue(email, out var storedOtp))
            {
                if (storedOtp.Otp == otp && storedOtp.Expiration > DateTime.Now)
                {
                    // Optionally remove the OTP after successful validation
                    OtpStore.Remove(email);
                    return true;
                }
            }
            return false;
        }
    }
}

        //-----------------------------------------------------------------------------------------------------------
