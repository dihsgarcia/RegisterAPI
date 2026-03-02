using FluentValidation;
using Application.DTOs.Request;
using Domain.Common;

namespace Application.Validators;

public class CreatePessoaJuridicaValidator : AbstractValidator<CreatePessoaJuridicaRequest>
{
    public CreatePessoaJuridicaValidator()
    {
        RuleFor(x => x.RazaoSocial)
            .NotEmpty()
            .WithMessage("RazaoSocial é obrigatório.")
            .MaximumLength(150);

        RuleFor(x => x.Cnpj)
            .NotEmpty()
            .WithMessage("CNPJ é obrigatório.");
            
        RuleFor(x => x.Cep)
            .NotEmpty()
            .WithMessage("CEP é obrigatório.");

        RuleFor(x => x.NumeroEndereco)
            .NotEmpty()
            .WithMessage("Número do endereço é obrigatório.");
    }
}