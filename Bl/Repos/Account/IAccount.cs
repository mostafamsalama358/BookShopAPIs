using Domains;
using System.Security.Claims;

namespace Bl.Repos.Account
{
    public interface IAccount
    {
        Task<AppUser> GetByEmailAsync(string email);
        string GenerateJwtToken(AppUser user);
        IEnumerable<Claim> GetUserClaims(AppUser user);
        string GenerateOtp(AppUser user);
        string GenerateEmailBody(string name, string otptext);
        Task StoreOtpAsync(string email, string otp, string name);
        Task<bool> ValidateOtpAsync(string email, string otp);

        //--------------------------------
        Task requestAsync(string email, string otp, string name);
        string GenerateEmailBodyreqest(string name, string otptext);
    }
}
