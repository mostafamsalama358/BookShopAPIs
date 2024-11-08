﻿using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS.ForBook
{
    public class BookDetailsDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int YearPublished { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty; // Cloud storage (URL)



        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>(); // Updated to hold multiple authors
        public CategoryDto Category { get; set; } = null!;
    }

    public class AuthorDto
    {
        public string AuthorFirstName { get; set; } = null!;
        public string AuthorLastName { get; set; } = null!;
    }
    public class CategoryDto
    {
        public string CategoryNmae { get; set; } = null!;
    }
}
