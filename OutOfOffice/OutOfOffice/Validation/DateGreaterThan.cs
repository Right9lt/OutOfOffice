using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OutOfOffice.Validation
{
    public class DateGreaterThan(string otherDatePropertyName) : ValidationAttribute
    {
        private string OtherDatePropertyName = otherDatePropertyName;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            var isNullable = Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null;

            if (isNullable && value == null)
            {
                return ValidationResult.Success;
            }

            var otherDateProperty = validationContext.ObjectType.GetProperty(OtherDatePropertyName);
            if (otherDateProperty == null)
            {
                throw new InvalidOperationException($"Property {OtherDatePropertyName} not found");
            }

            var otherDateValue = otherDateProperty.GetValue(validationContext.ObjectInstance);
            if (otherDateValue == null)
            {
                return ValidationResult.Success;
            }
            

            var date = (DateOnly) value;
            var otherDate = (DateOnly) otherDateValue;

            if(date <=  otherDate)
            {
                return new ValidationResult($"Date must be greater than {otherDate}");
            }

            return ValidationResult.Success;
        }
    }
}
