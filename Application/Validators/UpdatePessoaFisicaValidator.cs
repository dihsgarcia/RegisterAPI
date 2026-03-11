using Application.DTOs.Request;
using FluentValidation;

namespace Application.Validators;

public class UpdatePessoaFisicaValidator : AbstractValidator<UpdatePessoaFisicaRequest>
{
    public UpdatePessoaFisicaValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id do cliente é obrigatório.");
            
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório.")
            .MaximumLength(200);
        
        RuleFor(x => x.Enderecos)
            .NotNull()
            .WithMessage("Ao menos um registro de endereço é obrigatório.")
            .Must(x => x.Any())
            .WithMessage("Ao menos um registro de endereço é obrigatório.");
            
        RuleForEach(x => x.Enderecos)
            .SetValidator(new UpdateEnderecoClienteValidator());
    }
}