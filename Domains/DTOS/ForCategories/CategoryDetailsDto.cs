using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS.ForCategories
{
    public class CategoryDetailsDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;
        public List<Bookdto> Bookdtos { get; set; } = new List<Bookdto>();

    }
    public class Bookdto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearPublished { get; set; }
        public List<Authordto> Authors { get; set; } = new List<Authordto>();

    }
    public class Authordto
    {
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
    }
}
