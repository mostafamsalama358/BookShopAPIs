using Domains;
using static Domains.DTOS.PasswordRecoveryDto;

namespace Bl;

public interface IEmailServices
{
    Task SendEmail(MailRequest mailRequest);
}
