using Application.DTOs.Responses;
using Domain.Entities;

namespace Application.Mappers;

public static class PessoaFisicaMapper
{
    public static PessoaFisicaResponse ToResponse(Cliente cliente)
    {
        return new PessoaFisicaResponse
        {
            ClienteId = cliente.ClienteId,
            Nome = cliente.Nome,
            Cpf = cliente.Documento,
            Enderecos = cliente.Enderecos.Select(e => new EnderecoResponse
            {
                Id = e.Id,
                ClienteId = e.ClienteId,
                Cep = e.Cep,
                Logradouro = e.Logradouro,
                Numero = e.Numero,
                Complemento = e.Complemento,
                Bairro = e.Bairro,
                Cidade = e.Cidade,
                Estado = e.Estado
            }).ToList(),
            DataCriacao = cliente.DataCriacao,
            DataAtualizacao = cliente.DataAtualizacao,
            DataExclusao = cliente.DataExclusao
        };
    }
}