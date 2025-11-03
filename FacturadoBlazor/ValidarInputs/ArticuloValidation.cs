using FacturadorModels.Models;
using FluentValidation;
using FluentValidation;
namespace FacturadoBlazor.ValidarInputs
{
    public class ArticuloValidation :AbstractValidator<Articulo>
    {
        public ArticuloValidation()
        {

            RuleFor(c => c.ArtId)
                .NotEmpty().WithMessage("el Codigo es obligatoria.");
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("el nombre es obligatoria.");

            RuleFor(c => c.Precio)
                .NotEmpty().WithMessage("El precio es obligatorio.").GreaterThan(0).WithMessage("El precio debe ser mayor que 0.");
            RuleFor(c => c.Stock)
                .NotEmpty().WithMessage("no puede estar vacio debe poner una cantidad");
        }
    }
}
