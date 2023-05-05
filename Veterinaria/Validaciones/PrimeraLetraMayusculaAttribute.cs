using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Validaciones
{
    public class PrimeraLetraMayusculaAttribute: ValidationAttribute
    {
        // En Value tenemos el valor que tiene el campo en el cual yo he colocado el atributo
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()[0].ToString();
            //validamos si la primera letra digitada por el usuario es mayuscula
            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayuscula");
            }

            return ValidationResult.Success;
        }
    }
}
