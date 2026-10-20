# Tasks - FornecedoresApp
## Regras de execução
- Executar tarefas na ordem.
- Não pular etapas.
- Não modificar requisitos.
- Atualizar o checklist após validações.
- Informar arquivos criados e alterados.
- Validar cada etapa antes de avançar.
## Tarefa 01 - Criar solução
Requisitos cobertos: RFN001, RFN002
- [x] Criar solução FornecedoresApp.
- [x] Criar projeto API.
- [x] Criar projeto Domain.
- [x] Criar projeto Infrastructure.
- [x] Criar projeto de testes.
- [x] Configurar referências.
- [x] Executar dotnet restore.
- [x] Executar dotnet build.
Critério de aceite:
A solução compila sem erros.
Resumo da validação: concluído. `dotnet restore` e `dotnet build` da solução foram executados; build finalizou com zero avisos e zero erros.
## Tarefa 02 - Configurar Docker
Requisitos cobertos: RFN003, RFN004, RFN007
- [x] Criar docker-compose.yml.
- [x] Configurar SQL Server 2022.
- [x] Configurar porta 1434:1433, limitada ao host local.
- [x] Configurar volume persistente.
- [x] Configurar senha por variável de ambiente.
- [x] Configurar connection string de desenvolvimento da API.
- [x] Documentar inicialização.
Critério de aceite:
SQL Server inicia e aceita conexões.
Resumo da validação: `docker compose config --quiet` validado; `docker compose up -d --wait` iniciou o SQL Server 2022 e o health check executou `SELECT 1`, marcando o contêiner como saudável. O `.env` local contém uma senha aleatória e é ignorado pelo Git. A conectividade da aplicação foi validada na inicialização do host da API: o EF Core executou `SELECT 1` e confirmou que o banco está atualizado, com a connection string fornecida por variável de ambiente temporária derivada do `.env`. Para iniciar a API fora dessa validação, configure a connection string em User Secrets conforme o README.
## Tarefa 03 - Criar Domain
Requisitos cobertos: RF002, RF005, RF006, RF007
- [x] Criar entidade Usuario.
- [x] Criar entidade Fornecedor.
- [x] Criar interfaces dos repositórios.
- [x] Implementar regras de domínio.
- [x] Implementar validação de CNPJ.
- [x] Implementar desativação lógica.
- [x] Criar exceções de domínio.
Critério de aceite:
Domain compila sem referência ao EF Core.
Resumo da validação: concluído. `dotnet build` do Domain passou com zero avisos e erros; a inspeção confirmou que o projeto não possui referências a outros projetos, pacotes ou Entity Framework Core.
## Tarefa 04 - Configurar EF Core
Requisitos cobertos: RF002, RF003, RF004, RF005, RF006, RFN003
- [x] Instalar pacotes necessários.
- [x] Criar DataContext.
- [x] Criar UsuarioMap.
- [x] Criar FornecedorMap.
- [x] Criar repositories.
- [x] Registrar serviços no DI.
- [x] Criar Migration InitialCreate.
- [x] Aplicar Migration no SQL Server.
Critério de aceite:
As duas tabelas existem no banco.
Resumo da validação: concluído para a Etapa 4. `dotnet build` da solução finalizou com zero avisos e zero erros. Os avisos NU1902/NU1903 foram corrigidos ao atualizar a dependência transitiva `System.Security.Cryptography.Pkcs` para 9.0.21; `dotnet list package --vulnerable --include-transitive` não reportou vulnerabilidades. A migration `InitialCreate` foi gerada e aplicada no banco `BDFornecedores`; consulta no SQL Server confirmou `Fornecedor`, `Usuario` e `__EFMigrationsHistory`. A conectividade pelo host de inicialização da API também foi confirmada: o EF Core abriu a conexão, executou `SELECT 1` e informou que o banco está atualizado. A primeira aplicação do script foi direcionada a `master` e criou ali as mesmas tabelas e o histórico da migration; a revisão automática bloqueou a remoção desses objetos por falta de confirmação independente da inexistência prévia. Esse estado pendente precisa ser revisado antes de considerar o banco local limpo.
## Tarefa 05 - Criar usuário inicial
Requisitos cobertos: RF001, RFN005
- [x] Implementar mecanismo de seed.
- [x] Executar somente em Development.
- [x] Obter senha de configuração segura.
- [x] Gerar hash da senha.
- [x] Evitar criação duplicada.
- [x] Não exibir senha em logs.
Critério de aceite:
Usuário de desenvolvimento autentica
com as credenciais configuradas.
Resumo da validação: concluído para a Etapa 5 (seed do usuário inicial). A solução compilou com zero avisos e erros. A API iniciou em Development usando a senha fornecida somente por variável de ambiente, criou o login `admin` e, numa segunda inicialização, reconheceu a conta existente sem inserir outra. Consulta ao SQL Server confirmou exatamente um `admin` ativo, hash de 84 caracteres no formato `PasswordHasher` e valor diferente da senha em texto puro; os logs não exibiram a senha. A autenticação HTTP permanece para a Etapa 6.
## Tarefa 06 - Implementar Basic Auth
Requisitos cobertos: RF001, RFN005
- [x] Criar BasicAuthenticationHandler.
- [x] Configurar AddAuthentication.
- [x] Configurar AddAuthorization.
- [x] Validar credenciais com repository.
- [x] Verificar hash da senha.
- [x] Criar ClaimsPrincipal.
- [x] Configurar WWW - Authenticate.
- [x] Proteger endpoints por padrão com política global de autorização.
Critério de aceite:
Credenciais válidas funcionam.
Credenciais inválidas retornam 401.
Resumo da validação: concluído para a Etapa 6. A solução compilou com zero avisos e erros. Uma rota temporária protegida por `[Authorize]` foi usada somente para smoke check local e removida em seguida: sem credenciais, senha incorreta, Base64 inválido e esquema Bearer retornaram HTTP 401 com `WWW-Authenticate: Basic realm="FornecedoresApp"`; as credenciais válidas do `admin` retornaram HTTP 200 com a identidade `admin`. O handler consulta o repositório, rejeita usuários inexistentes/inativos, verifica o hash via `PasswordHasher<Usuario>` e cria claims de identificador e login. A política global protege também os futuros endpoints. Nenhum controller de negócio foi criado nesta etapa; o CRUD permanece para etapas posteriores.
## Tarefa 07 - Criar DTOs
Requisitos cobertos: RF002, RF003, RF004, RF005, RF007
- [x] CadastrarFornecedorRequest.
- [x] AtualizarFornecedorRequest.
- [x] FornecedorResponse.
- [x] Implementar validações.
- [x] Definir mapeamentos para entidade.
Critério de aceite:
Dados inválidos retornam HTTP 400.
Resumo da validação: Etapa 7 implementada. Os DTOs de cadastro/atualização, validações e mapeamentos estão definidos; atualização exige `ativo` e não recebe CNPJ, e o response não expõe a entidade. Na etapa 11, `dotnet test` compilou a solução e os testes de integração confirmaram HTTP 400 para payload inválido e ID malformado. Suíte completa: 46 aprovados, zero falhas/ignorados.
## Tarefa 08 - Criar Service
Requisitos cobertos: RF002, RF003, RF004, RF005, RF006, RF007
- [x] Criar FornecedorService.
- [x] Implementar cadastro.
- [x] Implementar consulta.
- [x] Implementar pesquisa.
- [x] Implementar atualização.
- [x] Implementar exclusão lógica.
- [x] Tratar regras de negócio.
Critério de aceite:
Os casos de uso funcionam
independentemente do Controller.
Resumo da validação: Etapa 8 implementada. O serviço executa cadastro, verificação de duplicidade, pesquisa, consulta, atualização preservando CNPJ e reativando via `ativo`, e exclusão lógica idempotente. Os testes unitários da etapa 11 cobrem cadastro, duplicidade de CNPJ/email (incluindo maiúsculas/minúsculas), consulta, pesquisa, atualização, exclusão e reativação; passaram dentro da suíte completa de 46 testes. O tratamento de violações únicas em cenário de concorrência ainda não foi validado.
## Tarefa 09 - Criar Controller
Requisitos cobertos: RF002, RF003, RF004, RF005, RF006
- [x] Implementar POST.
- [x] Implementar GET de listagem.
- [x] Implementar GET por ID.
- [x] Implementar PUT.
- [x] Implementar DELETE.
- [x] Configurar códigos HTTP.
- [x] Configurar tratamento de erros.
Critério de aceite:
Todos os endpoints seguem contracts.md.
Resumo da validação: Etapa 9 implementada em `FornecedoresController`, com as cinco rotas do contrato, DTOs, Location no 201, filtros de pesquisa, códigos HTTP e respostas ProblemDetails/ValidationProblemDetails. A integração da etapa 11 verificou cadastro, listagem, consulta, atualização, exclusão lógica, ID inválido, fornecedor ausente, CNPJ/email duplicados e reativação. Compilação e suíte passaram: 46 aprovados, zero falhas/ignorados.
## Tarefa 10 - Configurar Swagger
Requisitos cobertos: RF008
- [x] Habilitar OpenAPI.
- [x] Disponibilizar Swagger UI.
- [x] Documentar requests e responses.
- [x] Adicionar esquema Basic.
- [x] Permitir informar credenciais.
Critério de aceite:
É possível testar o CRUD pelo Swagger.
Resumo da validação: Etapa 10 implementada com `Swashbuckle.AspNetCore` 10.3.0, documento OpenAPI v1, UI em `/swagger`, metadados de DTOs/respostas e esquema HTTP Basic global. A etapa 11 restaurou os quatro projetos pela avaliação estática do MSBuild e compilou a API com a dependência Swagger. A UI e o JSON OpenAPI ainda não tiveram uma verificação HTTP específica; autenticação e endpoints da API foram exercitados. Build/teste concluíram sem avisos, com 46 testes aprovados.
## Tarefa 11 - Criar testes
Requisitos cobertos: RF001, RF002, RF003, RF004, RF005, RF006, RF007, RFN006
- [x] Testar validação de CNPJ.
- [x] Testar criação de fornecedor.
- [x] Testar fornecedor duplicado.
- [x] Testar atualização.
- [x] Testar desativação.
- [x] Testar autenticação válida.
- [x] Testar autenticação inválida.
- [x] Testar endpoints REST.
Critério de aceite:
dotnet test executa com sucesso.
Resumo da validação: Etapa 11 implementada e validada. Foram criados testes unitários do Domain e do FornecedorService, testes de autenticação e testes HTTP com `WebApplicationFactory` usando banco EF InMemory isolado. `dotnet restore` (avaliação estática do grafo) e `dotnet test` concluíram com sucesso: 46 aprovados, zero falhas, zero ignorados e build sem avisos. Os testes de integração não usam o SQL Server real.
## Tarefa 12 - Finalização
Requisitos cobertos: RFN001, RFN002, RFN003, RFN004, RFN006, RFN007
- [ ] Executar restore.
- [ ] Executar build.
- [ ] Executar tests.
- [ ] Validar Migrations.
- [ ] Validar Docker.
- [ ] Validar Swagger.
- [ ] Atualizar README.md.
- [ ] Revisar checklist.md.
Critério de aceite:
Aplicação funcional, documentada e testada.
Resumo da validação: pendente. As verificações finais dependem da conclusão das etapas anteriores; restore, build, testes, migration, Docker e Swagger ainda precisam de validação integrada.
