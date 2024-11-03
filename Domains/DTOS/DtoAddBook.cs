﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS
{
    public class DtoAddBook
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
        public int AuthorId { get; set; } // Foreign key for Author

        [Required]
        public int CategoryId { get; set; } // Foreign key for Category
    }
}
