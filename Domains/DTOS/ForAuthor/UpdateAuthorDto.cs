using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS.ForAuthor
{
    public class UpdateAuthorDto
    {
        public string FirstName { get; set; } = string.Empty;  // Author's first name

        public string LastName { get; set; } = string.Empty;  // Author's last name

        [MaxLength(500)]
        public string? Biography { get; set; }
    }
}
