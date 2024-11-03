using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains;

public class TbBook
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int YearPublished { get; set; } 

    // Either use ImagePath (for local storage) or ImageUrl (for cloud storage)
    [MaxLength(255)]
    public string? ImagePath { get; set; }  // Local storage (relative path)

    [MaxLength(255)]
    public string? ImageUrl { get; set; }  // Cloud storage (URL)

    public ICollection<TbAuthorBook> TbAuthorBooks { get; set; } = new List<TbAuthorBook>();

    // Foreign key to Category
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    public TbCategory Category { get; set; } = null!;

}