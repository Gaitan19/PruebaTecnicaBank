using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Validations
{
    public class SexoValidationAttribute : ValidationAttribute
    {
        public SexoValidationAttribute()
        {
            ErrorMessage = "El sexo debe ser 'Masculino' o 'Femenino'";
        }

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
