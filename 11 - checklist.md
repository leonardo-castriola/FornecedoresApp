# Checklist - FornecedoresApp
## 1. Estrutura
- [x] Solução criada corretamente.
- [x] API criada.
- [x] Domain criado.
- [x] Infrastructure criada.
- [x] Projeto de testes criado.
- [x] Referências entre projetos corretas.
- [x] Domain independente de Infrastructure.
## 2. Docker
- [x] docker-compose.yml criado.
- [x] SQL Server inicializa e o health check executa `SELECT 1` com sucesso.
- [x] Porta 1434 configurada e limitada ao host local.
- [x] Volume persistente configurado.
- [x] Senha do SQL Server fornecida por variável de ambiente, fora do repositório.
- [x] Conexão da aplicação funciona quando a connection string é fornecida por variável de ambiente; User Secrets deve ser configurado para execução local normal.
## 3. Domain
- [x] Entidade Usuario criada.
- [x] Entidade Fornecedor criada.
- [x] Interfaces de repositories criadas.
- [x] Validação de CNPJ implementada.
- [x] Regras de domínio implementadas.
- [x] Exclusão lógica implementada.
## 4. Entity Framework Core
- [x] DataContext configurado.
- [x] Entidades mapeadas.
- [x] Repositórios implementados.
- [x] Índices únicos configurados.
- [x] Email possui índice único com comparação sem distinção entre maiúsculas e minúsculas.
- [x] Migration criada.
- [x] Migration aplicada no banco `BDFornecedores`.
- [x] Banco possui as tabelas esperadas.
## 5. Basic Authentication
- [x] Authentication Handler criado.
- [x] Validação por banco implementada.
- [x] Senhas armazenadas com hash.
- [x] Usuário inativo não autentica.
- [x] Requisição sem credenciais retorna 401.
- [x] Credenciais inválidas retornam 401.
- [x] WWW - Authenticate configurado.
- [x] Endpoints protegidos por padrão pela política global de autorização.
## 6. DTOs e validação da API
- [x] Requests separados para cadastro e atualização.
- [x] Validação de razão social, nome fantasia, CNPJ, email, endereço e telefone conforme limites do domínio.
- [x] Campo `ativo` obrigatório na atualização; CNPJ ausente do request de atualização.
- [x] Response mapeado sem expor a entidade de domínio.
- [x] Mapeamentos de request para entidade implementados.
- [ ] Resposta HTTP 400 para payload inválido validada com controller.
## 7. CRUD
- [x] Controller implementado com rotas de cadastro, listagem, consulta por ID, atualização e exclusão.
- [x] POST retorna 201 com Location; listagem/consulta/atualização retornam 200; DELETE retorna 204 sem corpo.
- [x] IDs malformados e payloads inválidos mapeados para 400; recurso ausente para 404; duplicidades para 409.
- [ ] Rotas e respostas HTTP verificadas em execução.
- [x] Serviço de aplicação implementa cadastro, consulta, pesquisa, atualização e exclusão lógica.
- [x] Serviço verifica duplicidade de CNPJ e email no cadastro e de email na atualização.
- [x] Serviço preserva CNPJ na atualização e permite reativação.
- [x] Exclusão lógica é idempotente para fornecedor já inativo.
- [x] Serviço registrado na injeção de dependências.
- [ ] Casos de serviço exercitados em testes automatizados.
- [ ] POST de fornecedor funciona.
- [ ] GET de fornecedores funciona.
- [ ] GET por ID funciona.
- [ ] PUT funciona.
- [ ] DELETE lógico funciona.
- [ ] CNPJ duplicado retorna 409.
- [ ] Email duplicado no cadastro e na atualização retorna 409.
- [ ] PUT permite reativar fornecedor inativo.
- [ ] Endereço é validado e normalizado conforme os limites definidos.
- [ ] Campos inválidos retornam 400.
- [ ] Recurso inexistente retorna 404.
- [ ] Respostas seguem contracts.md.
## 8. Swagger
- [x] Pacote Swashbuckle, documento OpenAPI v1 e esquema HTTP Basic registrados na API.
- [x] Compilação com a dependência Swagger concluída.
- [ ] Swagger UI disponível.
- [ ] Endpoints documentados.
- [ ] Basic Authentication disponível.
- [ ] É possível testar operações autenticadas.
## 9. Testes
- [x] Testes de domínio implementados.
- [x] Testes de autenticação implementados.
- [x] Testes de integração implementados com WebApplicationFactory e banco EF InMemory isolado.
- [x] dotnet test executado (46 aprovados).
- [x] Todos os 46 testes passam.
## 10. Qualidade
- [x] dotnet restore executado.
- [x] dotnet build sem erros (zero avisos e zero erros).
- [ ] Não existem credenciais versionadas.
- [ ] Controllers não possuem acesso direto ao banco.
- [ ] Domain não utiliza EF Core.
- [ ] Operações de I/O são assíncronas.
- [ ] Erros HTTP são tratados.
- [ ] README atualizado.
## 11. Conclusão
O projeto só poderá ser considerado concluído
quando todos os itens aplicáveis estiverem
validados e não houver falhas conhecidas.
Não marcar itens como concluídos
sem execução ou evidência.
