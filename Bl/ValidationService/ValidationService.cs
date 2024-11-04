using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.ValidationService
{
    public class ValidationService<T> : IValidationService<T> where T : class
    {
        public ValidationResult Validate(T model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model);

            // Validate the model based on its data annotations
            bool isValid = Validator.TryValidateObject(model, context, validationResults, true);

            if (isValid)
            {
                return new ValidationResult("Validation passed successfully");
            }

            // Combine validation errors into a single error message
            string errors = string.Join("; ", validationResults.Select(v => v.ErrorMessage ?? "Unknown validation error"));
            return new ValidationResult(errors);
        }
    }
}
