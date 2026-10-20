# Plan - FornecedoresApp
## 1. Estratégia
Desenvolver o projeto incrementalmente,
validando cada etapa antes da próxima.
Não gerar a aplicação inteira em uma única etapa.
## 2. Tecnologias
- .NET 10
- C#
- ASP.NET Core Web API
- Entity Framework Core
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Design
- SQL Server 2022
- Docker Compose
- Swagger/OpenAPI
- xUnit
- Microsoft.AspNetCore.Mvc.Testing
## 3. Solução
Criar FornecedoresApp com os projetos:
### FornecedoresApp.Domain
Biblioteca de classes contendo:
- Entidades
- Interfaces de repositórios
- Regras de negócio
- Exceções de domínio
### FornecedoresApp.Infrastructure
Biblioteca de classes contendo:
- DbContext
- EntityTypeConfiguration
- Repositories
- Migrations
- Persistência SQL Server
### FornecedoresApp.API
Aplicação ASP.NET Core contendo:
- Controllers
- DTOs
- Services
- Authentication Handler
- Configurações
- Injeção de dependências
- Swagger/OpenAPI
## 4. Ordem de desenvolvimento
### Etapa 1 - Estrutura
Criar solução e projetos.
Configurar referências entre projetos.
### Etapa 2 - Infraestrutura
Criar docker - compose.yml.
Configurar SQL Server.
Configurar connection string.
### Etapa 3 - Domain
Criar:
- Usuario
- Fornecedor
- IUsuarioRepository
- IFornecedorRepository
- Regras e validações de domínio
### Etapa 4 - Persistence
Criar:
- DataContext
- Mapeamentos EF Core
- Repositórios
- Primeira Migration
- Inicialização do usuário de desenvolvimento
### Etapa 5 - Authentication
Implementar Basic Authentication com:
- Authentication Handler
- Validação de credenciais
- Verificação de hash de senha
- Authorization
### Etapa 6 - API
Criar endpoints:
- GET /api/v1/fornecedores
- GET /api/v1/fornecedores/{id}
- POST /api/v1/fornecedores
- PUT /api/v1/fornecedores/{id}
- DELETE /api/v1/fornecedores/{id}
### Etapa 7 - Swagger
Configurar documentação OpenAPI.
Adicionar esquema Basic Authentication.
### Etapa 8 - Testes
Criar testes unitários e de integração.
### Etapa 9 - Validação
Executar:
- dotnet restore
- dotnet build
- dotnet test
Testar a API com o SQL Server no Docker.
## 5. Restrições
- Não usar Entity Framework dentro do Domain.
- Não usar SQL diretamente nos Controllers.
- Não usar JWT.
- Não adicionar frontend.
- Não criar microsserviços.
- Não introduzir MediatR ou CQRS.
- Não criar abstrações desnecessárias.
## 6. Entregáveis
- Solução .NET compilando.
- API REST funcional.
- SQL Server no Docker.
- Autenticação Basic.
- CRUD de fornecedores.
- Migrations.
- Swagger/OpenAPI.
- Testes automatizados.
- README de execução.

