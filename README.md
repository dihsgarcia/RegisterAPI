# Register API

API REST para cadastro de pessoas físicas e jurídicas com validação de endereço via api ViaCEP.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10 (SQL Server)
- FluentValidation
- Swagger/OpenAPI

## Arquitetura

Clean Architecture com camadas bem definidas:

```
RegisterAPI/          - Camada API, controllers, middleware
Application/           - Services, DTOs, validadores, interfaces
Domain/                - Entidades, value objects, contratos de repositório
Infrastructure/        - EF Core, repositórios, persistência
```

### Domain

- **Entidades**: `PessoaFisica`, `PessoaJuridica`, `Endereco`
- **Value Objects**: `Cpf`, `Cnpj` com validação
- **Exceções**: `DomainException`, `BusinessException`, `NotFoundException`

### Application

- Services de regra de negócio (`PessoaFisicaService`, `PessoaJuridicaService`)
- Integração ViaCEP para consulta de endereço
- FluentValidation para validação de requisições

### Infrastructure

- SQL Server com EF Core
- Repositórios implementando contratos do domínio

## Pré-requisitos

- .NET 10 SDK
- SQL Server (local ou Docker)
- Docker (opcional, para SQL Server em container)

## Como executar

### 1. Subir SQL Server com Docker

```bash
docker-compose up -d
```

SQL Server em `localhost:1433`. Credenciais padrão:
- Usuário: `sa`
- Senha: `Admin@123!`

### 2. Executar migrações

```bash
dotnet ef database update --project Infrastructure --startup-project RegisterAPI
```

### 3. Executar a API

```bash
dotnet run --project RegisterAPI
```

- API: `https://localhost:5001` (ou verificar `launchSettings.json`)
- Swagger: `https://localhost:5001/swagger`

## Configuração

`appsettings.json`:

| Chave | Descrição |
|-------|-----------|
| `ConnectionStrings:DefaultConnection` | String de conexão SQL Server |
| `ExternalServices:ViaCepBaseUrl` | URL base da API ViaCEP |

## Referência da API

### Pessoa Física

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/PessoasFisicas` | Criar |
| GET | `/api/PessoasFisicas/buscar?cpf={cpf}` | Buscar por CPF |
| PUT | `/api/PessoasFisicas/atualizar?cpf={cpf}` | Atualizar |
| DELETE | `/api/PessoasFisicas/deletar?cpf={cpf}` | Excluir |

**Request de criação** (`CreatePessoaFisicaRequest`):

```json
{
  "nome": "string",
  "cpf": "string",
  "cep": "string",
  "numeroEndereco": "string"
}
```

**Request de atualização** (`UpdatePessoaFisicaRequest`):

```json
{
  "nome": "string",
  "cpf": "string",
  "cep": "string",
  "numeroEndereco": "string"
}
```

### Pessoa Jurídica

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/PessoasJuridicas` | Criar |
| GET | `/api/PessoasJuridicas/buscar?cnpj={cnpj}` | Buscar por CNPJ |
| PUT | `/api/PessoasJuridicas/atualizar?cnpj={cnpj}` | Atualizar |
| DELETE | `/api/PessoasJuridicas/deletar?cnpj={cnpj}` | Excluir |

**Request de criação** (`CreatePessoaJuridicaRequest`):

```json
{
  "razaoSocial": "string",
  "cnpj": "string",
  "cep": "string",
  "numeroEndereco": "string"
}
```

**Request de atualização** (`UpdatePessoaJuridicaRequest`):

```json
{
  "razaoSocial": "string",
  "cnpj": "string",
  "cep": "string",
  "numeroEndereco": "string"
}
```

### Regras de validação

- **Nome / RazaoSocial**: Obrigatório, máx. 150 caracteres
- **CPF / CNPJ**: Obrigatório, formato validado
- **CEP**: Obrigatório, validado via ViaCEP
- **NumeroEndereco**: Obrigatório

### Respostas de erro

| Status | Tipo de exceção |
|--------|-----------------|
| 400 Bad Request | `DomainException` |
| 404 Not Found | `NotFoundException` |
| 409 Conflict | `BusinessException` |
| 500 Internal Server Error | Exceções não tratadas |

Formato da resposta:

```json
{
  "status": 400,
  "message": "Mensagem de erro"
}
```

## Testes

```bash
dotnet test RegisterAPI.Tests
```

Testes com NUnit e Moq. Estrutura espelha a solução:

- `Application/` - Services principais


