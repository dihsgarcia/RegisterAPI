using Application.DTOs.Request;
using FluentValidation;

namespace Application.Validators;

public class UpdatePessoaJuridicaValidator : AbstractValidator<UpdatePessoaJuridicaRequest>
{
    public UpdatePessoaJuridicaValidator()
    {
        RuleFor(x => x.ClienteId)
            .NotNull()
            .WithMessage("ClienteId é obrigatório.");
            
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

    
