using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS.ForBorrowing
{
    public class BorrowbookRequestDto
    {
        public string UserId { get; set; }
        public int BookId { get; set; }
    }
}
