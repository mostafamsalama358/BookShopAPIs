using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS.ForCategories
{
    public class AddCategoryDto
    {


        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

    }
}
