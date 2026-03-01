using FluentValidation;
using Application.DTOs.Request;
using Domain.Common;

namespace Application.Validators;

public class CreatePessoaFisicaValidator : AbstractValidator<CreatePessoaFisicaRequest>
{
    public CreatePessoaFisicaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório.")
            .MaximumLength(150);

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .WithMessage("CPF é obrigatório.")
            .Must(CpfUtils.EhValido)
            .WithMessage("CPF inválido.");

        RuleFor(x => x.Cep)
            .NotEmpty()
            .WithMessage("CEP é obrigatório.");

        RuleFor(x => x.NumeroEndereco)
            .NotEmpty()
            .WithMessage("Número do endereço é obrigatório.");
    }
}