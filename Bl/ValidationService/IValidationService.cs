using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.ValidationService
{
    public interface IValidationService <T> where T : class
    {
        ValidationResult? Validate(T model);
    }
}
