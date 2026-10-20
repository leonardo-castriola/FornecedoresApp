# Architecture - FornecedoresApp
## 1. Estilo arquitetural
Utilizar DDD simplificado com arquitetura
em três projetos.
O objetivo é separar:
- Regras de negócio
- Acesso aos dados
- Exposição de endpoints HTTP
## 2. Domain
Projeto: FornecedoresApp.Domain
Responsabilidades:
- Representar entidades de negócio.
- Definir regras e invariantes.
- Definir interfaces de repositórios.
- Declarar exceções de domínio.
Classes:
Entities/
- Fornecedor.cs
- Usuario.cs
Interfaces/
- IFornecedorRepository.cs
- IUsuarioRepository.cs
Exceptions/
- RegraDeNegocioException.cs
O Domain não poderá referenciar outros
projetos da solução.
## 3. Infrastructure
Projeto: FornecedoresApp.Infrastructure
Responsabilidades:
- Conectar ao banco SQL Server.
- Mapear entidades.
- Implementar repositórios.
- Executar operações com EF Core.
- Manter Migrations.
Estrutura:
Contexts/
- DataContext.cs
Mappings/
- FornecedorMap.cs
- UsuarioMap.cs
Repositories/
- FornecedorRepository.cs
- UsuarioRepository.cs
Migrations/
A Infrastructure referencia somente o Domain
entre os projetos da solução.
## 4. API
Projeto: FornecedoresApp.API
Responsabilidades:
- Receber solicitações HTTP.
- Executar autenticação.
- Validar DTOs.
- Chamar serviços de aplicação.
- Converter resultados em respostas HTTP.
- Configurar injeção de dependências.
Estrutura:
Controllers/
- FornecedoresController.cs
DTOs/
- CadastrarFornecedorRequest.cs
- AtualizarFornecedorRequest.cs
- FornecedorResponse.cs
Services/
- FornecedorService.cs
Authentication/
- BasicAuthenticationHandler.cs
Program.cs
A API referencia Domain e Infrastructure.
## 5. Dependências
FornecedoresApp.API
 - > FornecedoresApp.Domain
 - > FornecedoresApp.Infrastructure
FornecedoresApp.Infrastructure
 - > FornecedoresApp.Domain
FornecedoresApp.Domain
 - > Nenhum projeto da solução
## 6. Fluxo de uma requisição
1. Cliente envia requisição HTTP.
2. Middleware valida Basic Authentication.
3. Controller recebe a requisição.
4. Controller valida DTO e chama Service.
5. Service aplica o caso de uso.
6. Service utiliza interface de repositório.
7. Repository executa EF Core.
8. SQL Server persiste ou consulta dados.
9. Service devolve o resultado.
10. Controller retorna HTTP apropriado.
## 7. Injeção de dependências
Registrar no Program.cs:
- DataContext como Scoped.
- IFornecedorRepository como Scoped.
- IUsuarioRepository como Scoped.
- FornecedorService como Scoped.
- Authentication Handler no esquema Basic.
## 8. Princípios
- Dependency Inversion.
- Separation of Concerns.
- Repository Pattern.
- Encapsulamento das entidades.
- DTO Pattern.
- Controllers pequenos.
## 9. Limitações intencionais
Não criar projeto Application separado.
Não utilizar CQRS.
Não utilizar MediatR.
Não utilizar Domain Events.
Não utilizar Generic Repository.
A arquitetura deve permanecer simples
para fins didáticos.