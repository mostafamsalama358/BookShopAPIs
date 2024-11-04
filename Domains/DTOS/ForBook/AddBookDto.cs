using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS.ForBook
{
    public class AddBookDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int YearPublished { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; } = string.Empty; // Cloud storage (URL)

        [Required]
        public int AuthorId { get; set; }  // List of Author IDs for the many-to-many relationship

        [Required]
        public int CategoryId { get; set; }  // Foreign key for Category
    }

}
