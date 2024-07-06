using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Validation
{
    public class GreaterThanNow : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateOnly)value;
            if(date < DateOnly.FromDateTime(DateTime.Now))
            {
                return new ValidationResult("Date must be greater than current date");
            }

            return ValidationResult.Success;
        }
    }
}
