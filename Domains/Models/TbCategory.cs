using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains;

public class TbCategory
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    // Use [InverseProperty] to specify the relationship to Book
    [InverseProperty("Category")]
    public ICollection<TbBook> Books { get; set; } = new List<TbBook>();
}