using FacturadorModels.Models;
using FluentValidation;
using FluentValidation;

namespace FacturadoBlazor.ValidarInputs
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleFor(c => c.RazonSocial)
                .NotEmpty().WithMessage("La razón social es obligatoria.");

            RuleFor(c => c.Cuit)
                .NotEmpty().WithMessage("El CUIT es obligatorio.")
                .Length(11).WithMessage("El CUIT debe tener 11 dígitos.")
                .Matches("^[0-9]+$").WithMessage("El CUIT debe contener solo números.");

            RuleFor(c => c.Direccion)
                .NotEmpty().WithMessage("La dirección es obligatoria.");
        }
    }
}
