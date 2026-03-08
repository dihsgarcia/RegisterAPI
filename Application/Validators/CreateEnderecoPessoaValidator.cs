using Application.DTOs.Request;
using FluentValidation;

namespace Application.Validators;

public class CreateEnderecoPessoaValidator : AbstractValidator<CreateEnderecoPessoa>
{
    public CreateEnderecoPessoaValidator()
    {
        RuleFor(x => x.Cep)
            .NotEmpty()
            .WithMessage("CEP é obrigatório.");

        RuleFor(x => x.NumeroEndereco)
            .NotEmpty()
            .WithMessage("Número do endereço é obrigatório.");

        RuleFor(x => x.Complemento)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Complemento));
    }
}