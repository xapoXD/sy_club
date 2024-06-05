using club.soundyard.web.Models;

namespace club.soundyard.web.Services
{
	public interface IMailService
	{
		bool SendMail(MailData mailData);
	}
}

