using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS.ForAuthor
{
    public class AuthorDetailsDto
    {
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; } = string.Empty;  // Author's first name

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; } = string.Empty;  // Author's last name

        [MaxLength(500)]
        public string? Biography { get; set; }   // Short biography (optional)

        public List<AuthorBookdto> Bookdto { get; set; } = new List<AuthorBookdto>();

    }
    public class AuthorBookdto
    {
        public string Title { get; set; } = string.Empty;
        public int YearPublished { get; set; }
    }
}
