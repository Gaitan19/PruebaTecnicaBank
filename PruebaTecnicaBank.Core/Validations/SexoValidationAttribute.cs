using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Validations
{
    /// <summary>
    /// Atributo de validación para verificar que el valor del sexo sea 'Masculino' o 'Femenino'.
    /// </summary>
    public class SexoValidationAttribute : ValidationAttribute
    {
       
        public SexoValidationAttribute()
        {
            ErrorMessage = "El sexo debe ser 'Masculino' o 'Femenino'";
        }

        /// <summary>
        /// Valida el valor proporcionado.
        /// </summary>
        /// <param name="value">El valor a validar.</param>
        /// <returns>True si el valor es 'Masculino' o 'Femenino'; de lo contrario, false.</returns>
        public override bool IsValid(object? value)
        {
            if (value is string sexo)
            {
                return sexo == "Masculino" || sexo == "Femenino";
            }
            return false;
        }
    }
}
