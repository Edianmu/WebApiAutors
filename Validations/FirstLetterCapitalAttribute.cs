using System.ComponentModel.DataAnnotations;

namespace WebApiAutors.Validations
{
    public class FirstLetterCapitalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success; //because there are not string with any letter (capital)
            }

            var firstLetter = value.ToString()[0].ToString(); //[0] first char

            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }

            return ValidationResult.Success;
        }
    }
}
