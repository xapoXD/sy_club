using System.ComponentModel.DataAnnotations;

namespace club.soundyard.web.Models
{
	public class AnonymousRegistration
	{

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
