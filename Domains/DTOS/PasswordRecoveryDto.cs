using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS
{
    public class PasswordRecoveryDto
    {
        public class RequestPasswordResetDto
        {
            public string Email { get; set; }
        }

        public class ResetPasswordDto
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public string NewPassword { get; set; }
        }
    }
}
