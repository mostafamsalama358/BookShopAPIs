using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS
{
    public class DtoUpdateBook
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int YearPublished { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty; // Cloud storage (URL)



        public List<Authordto> Authors { get; set; } = new List<Authordto>(); // Updated to hold multiple authors
        public Categorydto Category { get; set; } = null!;

        public class Authordto
        {
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
        }
        public class Categorydto
        {
            public string CategoryName { get; set; } = null!;
        }
    }
}

