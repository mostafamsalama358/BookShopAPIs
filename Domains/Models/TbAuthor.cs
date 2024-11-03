using System.ComponentModel.DataAnnotations;

namespace Domains.Models
{
    public class TbAuthor
    {
    [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; } = string.Empty;  // Author's first name

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; } = string.Empty;  // Author's last name

        [MaxLength(500)]
        public string? Biography { get; set; }   // Short biography (optional)

        // Relationship with Book (one author can have many books)
        public ICollection<TbAuthorBook> TbAuthorBooks { get; set; } = new List<TbAuthorBook>();
    }
}
