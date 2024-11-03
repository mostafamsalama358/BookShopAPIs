using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTOS
{
    public class RoleWithUsersDto
    {
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
