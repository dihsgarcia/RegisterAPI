# Register API

API REST para cadastro de pessoas físicas e jurídicas e endereços.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10 (SQL Server)
- FluentValidation
- Swagger/OpenAPI

## Estrutura e Arquitetura

Clean Architecture com camadas bem definidas:

```
RegisterAPI/        - Camada API (controllers, middleware)
Application/        - Serviços de aplicação, DTOs, validadores, interfaces
Domain/             - Entidades, Value Objects, exceções e contratos de repositório
Infrastructure/     - EF Core, repositórios, integrações externas, configurações
```

- `Domain`: `PessoaFisica`, `PessoaJuridica`, `Endereco`, `Cpf`, `Cnpj`, exceções de domínio.
- `Application`: `PessoaFisicaService`, `PessoaJuridicaService`, mappers e validadores (FluentValidation).
- `Infrastructure`: `AppDbContext`, `ClienteRepository`, integração `ViaCepProvider`.

Veja a inicialização em `RegisterAPI/Program.cs`.

## Pré-requisitos

- .NET 10 SDK
- SQL Server (local ou via Docker)
- Docker (opcional)

## Como executar (rápido)

1) Subir infraestrutura (opcional, docker-compose já incluído):

```bash
docker-compose up -d
```

2) Aplicar migrações (executar no root da solução):

```bash
dotnet ef database update --project Infrastructure --startup-project RegisterAPI
```

3) Rodar a API:

```bash
dotnet run --project RegisterAPI
```

- A API geralmente fica em `https://localhost:5001` (ver `launchSettings.json`).
- Swagger UI: `https://localhost:5001/swagger` (apenas em Development por padrão).

## Configuração

Configurações principais em `appsettings.json`:

- `ConnectionStrings:DefaultConnection` — string de conexão com o SQL Server.
- `ExternalServices:ViaCepBaseUrl` — URL base da API ViaCEP (ex.: `https://viacep.com.br/ws/`).

## Referência da API (endpoints principais)

Observação: rotas definidas por controllers seguem `api/[controller]` com ações específicas.

**Pessoa Física** (`PessoasFisicasController`)

- POST `/api/PessoasFisicas/Create` — cria um cliente pessoa física.
- GET `/api/PessoasFisicas/GetByClienteId/{clienteId}` — obtém por `clienteId` (GUID).
- GET `/api/PessoasFisicas/GetByCpf/{cpf}` — obtém por CPF (string limpa).
- PUT `/api/PessoasFisicas/Update` — atualiza cliente (envia `UpdatePessoaFisicaRequest`).
- DELETE `/api/PessoasFisicas/Delete/{clienteId}` — soft delete por `clienteId`.

Payload de criação (`CreatePessoaFisicaRequest`):

```json
{
  "nome": "João Silva",
  "cpf": "123.456.789-09",
  "enderecos": [
    {
      "cep": "01001-000",
      "numeroEndereco": "100",
      "complemento": "Apto 1"
    }
  ]
}
```

Payload de atualização (`UpdatePessoaFisicaRequest`):

```json
{
  "clienteId": "00000000-0000-0000-0000-000000000000",
  "nome": "João Silva Atualizado",
  "cpf": "12345678909",
  "enderecos": [
    {
      "enderecoId": null,
      "cep": "01001-000",
      "numeroEndereco": "101",
      "complemento": null
    }
  ]
}
```

**Pessoa Jurídica** (`PessoasJuridicasController`) — endpoints equivalentes com rotas:

- POST `/api/PessoasJuridicas/Create`
- GET `/api/PessoasJuridicas/GetByClienteId/{clienteId}`
- GET `/api/PessoasJuridicas/GetByCnpj/{cnpj}`
- PUT `/api/PessoasJuridicas/Update`
- DELETE `/api/PessoasJuridicas/Delete/{clienteId}`

## Tratamento de erros

- Exceções de domínio e de negócio usam tipos específicos (`DomainException`, `BusinessException`, `NotFoundException`).
- Middleware `ExceptionMiddleware` padroniza respostas HTTP com JSON:

```json
{
  "status": 400,
  "message": "Mensagem de erro"
}
```

Mapeamento de status mais comum:

- 400 — `DomainException`
- 404 — `NotFoundException`
- 409 — `BusinessException`
- 500 — erro não tratado

## Testes

Rodar testes:

```bash
dotnet test RegisterAPI.Tests
```

Projetos de teste cobrem `Application` services e integração do provider de CEP.



Testes com NUnit e Moq. Estrutura espelha a solução:

Application/ - Services principais
