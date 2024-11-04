using Domains.DTOS.ForLogin;
using static Domains.DTOS.ForLogin.PasswordRecoveryDto;

namespace Bl;

public interface IEmailServices
{
    Task SendEmail(MailRequest mailRequest);
}
