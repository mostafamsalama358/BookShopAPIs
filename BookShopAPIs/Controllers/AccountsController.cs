using Bl.UnitOfWork;
using Domains;
using Domains.DTOS.ForLogin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Domains.DTOS.ForLogin.PasswordRecoveryDto;

namespace BookShopAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        IUnitOfWork _unitOfWork;
        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] DtoNewRegister register)
        {
            try
            {
                // Check if role exists
                if (!await _unitOfWork.RoleManager.RoleExistsAsync(register.Role))
                    return BadRequest("Role does not exist.");

                var newUser = new AppUser
                {
                    Email = register.Email,
                    UserName = register.Email
                };

                IdentityResult result = await _unitOfWork.UserManager.CreateAsync(newUser, register.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);
                // Assign role to user
                await _unitOfWork.UserManager.AddToRoleAsync(newUser, register.Role);
                //return Ok(new { message = "User registered successfully." });

                // Generate OTP
                var otp = _unitOfWork.password.GenerateOtp(newUser);
                //await _unitOfWork.password.SendEmailAsync(newUser.Email, "OTP Verification", $"Your OTP code is: {otp}");
                await _unitOfWork.password.StoreOtpAsync(newUser.Email, otp, newUser.Email);

                return Ok(new { message = "Registration successful. An OTP has been sent to your email for verification." });


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid email confirmation request.");
            }

            var result = await _unitOfWork.UserManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully.");
            }

            return BadRequest("Error confirming email.");
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(DtoLogIn login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Find the user by email
                    var user = await _unitOfWork.UserManager.FindByEmailAsync(login.Email);
                    if (user == null)
                    {
                        return NotFound("User not found");
                    }
                    var token = _unitOfWork.password.GenerateJwtToken(user);

                    return Ok(new
                    {
                        token,
                        Now = DateTime.Now,
                        expiration = DateTime.Now.AddHours(1) // Adjust according to your token expiration
                    });
                }
                else return BadRequest("Invalid login request.");
            }
            catch (Exception)
            {
                return Unauthorized("User not found.");
            }
        }

        [HttpPost("verify-registration-otp")]
        public async Task<IActionResult> VerifyRegistrationOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            var isValidOtp = await _unitOfWork.password.ValidateOtpAsync(verifyOtpDto.Email, verifyOtpDto.Otp);
            if (!isValidOtp)
            {
                return Unauthorized("Invalid or expired OTP.");
            }

            var user = await _unitOfWork.UserManager.FindByEmailAsync(verifyOtpDto.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.EmailConfirmed = true;
            await _unitOfWork.UserManager.UpdateAsync(user);

            return Ok(new { message = "OTP verified successfully. Your email is now confirmed." });
        }


        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto request)
        {
            // Fetch the user by email
            var user = await _unitOfWork.password.GetByEmailAsync(request.Email);
            if (user is null)
                // Return 404 if user is not found
                return NotFound("User not found");


            try
            {
                // Generate password reset token
                var token = await _unitOfWork.UserManager.GeneratePasswordResetTokenAsync(user);

                // Construct the reset link
                var resetLink = $"Password reset token: {token}";


                // Send the email
                await _unitOfWork.password.requestAsync(request.Email, resetLink, token);

                // Return success message
                return Ok("Password reset link has been sent to your email.");
            }
            catch (Exception ex)
            {
                // Log the exception (you could also log this for debugging purposes)
                return StatusCode(500, $"Error occurred while processing the request: {ex.Message}");
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            var user = await _unitOfWork.password.GetByEmailAsync(resetDto.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Reset the password using the provided token
            var result = await _unitOfWork.UserManager.ResetPasswordAsync(user, resetDto.Token, resetDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password has been reset successfully.");
        }

    }
}
