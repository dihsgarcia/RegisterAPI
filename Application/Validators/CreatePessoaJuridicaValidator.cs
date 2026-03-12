using FluentValidation;
using Application.DTOs.Request;
using Domain.Common;

namespace Application.Validators;

public class CreatePessoaJuridicaValidator : AbstractValidator<CreatePessoaJuridicaRequest>
{
    public CreatePessoaJuridicaValidator()
    {
        
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório.")
            .MaximumLength(200);
        
        RuleFor(x => x.RazaoSocial)
            .NotEmpty()
            .WithMessage("RazaoSocial é obrigatório.")
            .MaximumLength(200);
        
        RuleFor(x => x.Cnpj)
            .NotEmpty()
            .WithMessage("CNPJ é obrigatório.");
        
        RuleFor(x => x.Enderecos)
            .NotNull()
            .WithMessage("Ao menos um registro de endereço é obrigatório.")
            .Must(x => x.Any())
            .WithMessage("Ao menos um registro de endereço é obrigatório.");
        
        RuleForEach(x => x.Enderecos)
            .SetValidator(new CreateEnderecoClienteValidator());
    }
}