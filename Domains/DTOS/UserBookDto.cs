using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS
{
    public class UserBookDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
}
