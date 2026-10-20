# Constitution - Fornecedores App

## 1. Objetivo

Desenvolver uma API REST para controle de fornecedores utilizando ASP.NET Core,
C#, DDD simplificado, Entity Framework Core e SQL Server.

O desenvolvimento será realizado utilizando Spec-Driven Development (SDD), 
com apoio do Codex, Claude Code ou outra ferramenta.

## 2. Princípios Obrigatórios

### 2.1 Arquitetura Limpa (Clean Architecture)

A solução deverá conter três projetos:

- FornecedoresApp.API: Projeto responsável por expor a API REST.
- FornecedoresApp.Domain: Projeto responsável por conter as entidades, agregados, interfaces e regras de negócio.
- FornecedoresApp.Infrastructure: Projeto responsável por implementar a persistência de dados,      
  utilizando Entity Framework Core e SQL Server.

O projeto Domain não poderá depender de API, Infrastructure ou Entity Framework Core. 
A dependência deve ser unidirecional, ou seja, a camada de domínio não deve conhecer as camadas superiores.

A API deverá utilizar injeção de dependência para acessar os repositórios e serviços da camada de domínio.

### 2.2 Padrões de Código

- Utilizar C# e .NET 10.
- Utilizar nomes de classes e métodos em português.
- Utilizar PascalCase para classes e métodos.
- Utilizar camelCase para parâmetros e variáveis.
- Aplicar princípios SOLID e boas práticas de programação.
- Utilizar programação assíncrona (async/await) para operações de I/O.
- Utilizar CancellationToken para operações assíncronas.
- Manter Controllers pequenos e delegar a lógica de negócio para serviços da camada de domínio.
- Evitar lógica de persistência diretamente nos Controllers.

### 2.3 Banco de dados

- Utilizar SQL Server como banco de dados.
- Utilizar SQL Server com Docker Compose para facilitar o desenvolvimento e testes.
- Utilizar Entity Framework Core para mapeamento objeto-relacional (ORM).
- Utilizar code first e Migrations para criar e atualizar o banco de dados.
- Não escrever SQL diretamente nos controllers ou serviços da camada de domínio.
- Utilizar Repository Pattern simplificado para abstrair a persistência de dados.

### 2.4 Segurança

- Implementar HTTP Basic Authentication para proteger a API.
- Validar as credenciais cadastradas no banco de dados.
- Armazenar as senhas usando hashing seguro (ex: BCrypt).
- Nunca armazenar senhas em texto puro.
- Nunca registrar senhas ou cabeçalhos Authorization completos em logs.
- Não criar autenticação baseada em tokens JWT ou OAuth2 neste projeto, apenas HTTP Basic Authentication.
- Exigir autenticação nos Endpoints dos fornecedores.
- Utilizar HTTPS fora de ambientes locais controlados.

### 2.5 API

- Desenvolver Endpoints RESTful.
- Utilizar JSON para requests e responses.
- Utilizar DTOs (Data Transfer Objects).
- Retornar códigos HTTP adequados para cada situação 
  (ex: 200 OK, 201 Created, 400 Bad Request, 401 Unauthorized, 404 Not Found, 500 Internal Server Error).
- Disponibilizar Swagger/OpenAPI para documentação da API.
- Utilizar validações de entrada.

### 2.6 Qualidade de Código

- O projeto deverá compilar sem erros e warnings, e passar em todos os testes unitários.
- Criar testes unitários e de integração utilizando xUnit.
- Não deixar funcionalidades simuladas ou incompletas.
- Não adicionar bibliotecas desnecessárias.
- Não implementar requisitos não especificados.

## 3. Regras para utilização do Codex, Claude Code ou outra ferramenta

A ferramenta de IA que irá auxiliar na geração de código deverá:

- Ler todos os arquivos da pasta docs.
- Respeitar a ordem das tarefas.
- Implementar somente a etapa solicitada.
- Preservar a arquitetura definida.
- Não alterar requisitos sem autorização.
- Informar quais arquivos foram criados ou alterados.
- Executar build e testes quando possível.
- Informar erros e limitações encontradas.
- Não considerar uma tarefa concluída sem validação.

## 4. Definição de pronto (Definition of Done)

Uma funcionalidade somente estará concluída quando:

- Atender aos requisitos especificados.
- Respeitar a arquitetura definida.
- Compilar sem erros e warnings.
- Possuir tratamento de erros adequado.
- Possuir testes aplicáveis e passando.
- Atender aos critérios de aceitação definidos.
- Não apresentar erros conhecidos de execução.

## 5. Prioridades

Em caso de conflito, seguir esta ordem de prioridade:

1. Segurança dos dados.
2. Regras de negócio.
3. Requisitos funcionais.
4. Arquitetura e padrões de código.
5. Simplicidade e legibilidade do código.
6. Otimizações técnicas e de performance.