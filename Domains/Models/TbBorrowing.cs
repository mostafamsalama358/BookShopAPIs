using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domains.Models
{
    public class TbBorrowing
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to User
        [ForeignKey("User")]
        public string UserId { get; set; }

        public AppUser User { get; set; } = null!;

        // Foreign key to Book
        [ForeignKey("Book")]
        public int BookId { get; set; }

        public TbBook Book { get; set; } = null!;

        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = status.Borrowed.ToString();
    }
    public enum status
    {
        Success,
        Borrowed,
        Returned
    }
}
