using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS.ForBook
{
    public class UpdateBookDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int YearPublished { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty; // Cloud storage (URL)

        public List<int> AuthorIds { get; set; } // List of author IDs for the many-to-many relationship

        public int CategoryId { get; set; }

    }
}

