using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class TbAuthorBook
    {
        public int TbAuthorId { get; set; }
        public TbAuthor TbAuthor { get; set; } = null!;

        public int TbBookId { get; set; }
        public TbBook TbBook { get; set; } = null!;
    }
}
