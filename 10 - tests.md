# Tests - FornecedoresApp
## 1. Objetivo
Garantir a qualidade das regras de negócio,
autenticação, persistência e endpoints da API.
## 2. Ferramentas
- xUnit
- Microsoft.AspNetCore.Mvc.Testing
- FluentAssertions, se compatível
- Microsoft.EntityFrameworkCore.Sqlite,
 se necessário para testes isolados de persistência
Testes de integração reais com SQL Server
deverão utilizar banco de testes isolado.
## 3. Testes unitários do Domain
### FornecedorTests
[Fact]
DeveCriarFornecedorComDadosValidos
[Fact]
DeveRejeitarRazaoSocialVazia
[Fact]
DeveRejeitarRazaoSocialMuitoCurta
[Fact]
DeveRejeitarCnpjInvalido
[Fact]
DeveNormalizarCnpjFormatado
[Fact]
DeveAtualizarDadosPermitidos
[Fact]
DeveDesativarFornecedor
[Fact]
DevePreservarCnpjNaAtualizacao
[Fact]
DeveRejeitarEnderecoVazio
[Fact]
DeveRejeitarEnderecoMuitoCurto
[Fact]
DeveRejeitarEnderecoMuitoLongo
[Fact]
DeveRemoverEspacosExternosDoEndereco
[Fact]
DeveRejeitarEmailInvalido
## 4. Testes do FornecedorService
[Fact]
DeveCadastrarFornecedorComSucesso
[Fact]
DeveRejeitarFornecedorComCnpjDuplicado
[Fact]
DeveRejeitarFornecedorComEmailDuplicado
[Fact]
DeveRejeitarEmailDuplicadoSemDiferenciarMaiusculas
[Fact]
DeveConsultarFornecedorPorId
[Fact]
DeveRetornarNaoEncontradoParaIdInexistente
[Fact]
DevePesquisarFornecedoresPorRazaoSocial
[Fact]
DevePesquisarFornecedoresPorSituacao
[Fact]
DeveAtualizarFornecedorExistente
[Fact]
DeveRejeitarEmailDuplicadoNaAtualizacao
[Fact]
DeveDesativarFornecedorExistente
[Fact]
DeveRetornar409ParaEmailDuplicadoNaAtualizacao
[Fact]
DeveReativarFornecedorInativoPeloPut
## 5. Testes de autenticação
[Fact]
DeveAutenticarUsuarioComCredenciaisValidas
[Fact]
DeveRejeitarSenhaIncorreta
[Fact]
DeveRejeitarUsuarioInexistente
[Fact]
DeveRejeitarUsuarioInativo
[Fact]
DeveRejeitarAuthorizationAusente
[Fact]
DeveRejeitarBase64Invalido
[Fact]
DeveRejeitarEsquemaDiferenteDeBasic
## 6. Testes de integração
Criar testes utilizando WebApplicationFactory.
### FornecedoresControllerTests
[Fact]
DeveRetornar401SemAutenticacao
[Fact]
DeveCadastrarFornecedorValido
[Fact]
DeveRetornar400ParaFornecedorInvalido
[Fact]
DeveRetornar409ParaCnpjDuplicado
[Fact]
DeveRetornar409ParaEmailDuplicado
[Fact]
DeveRetornar400ParaEmailInvalido
[Fact]
DeveListarFornecedoresAutenticado
[Fact]
DeveConsultarFornecedorExistente
[Fact]
DeveRetornar404ParaFornecedorInexistente
[Fact]
DeveAtualizarFornecedorExistente
[Fact]
DeveRetornar409ParaEmailDuplicadoNaAtualizacao
[Fact]
DeveDesativarFornecedorExistente
[Fact]
DeveReativarFornecedorInativoPeloPut
## 7. Organização
FornecedoresApp.Tests/
- UnitTests/
 - FornecedorTests.cs
 - FornecedorServiceTests.cs
 - BasicAuthenticationTests.cs
- IntegrationTests/
 - FornecedoresControllerTests.cs
## 8. Regras dos testes
- Utilizar Arrange, Act, Assert.
- Testes não podem depender de ordem.
- Não utilizar dados fixos que criem colisões.
- Cada teste deverá preparar seus dados.
- Utilizar banco isolado nos testes de integração.
- Não executar testes destrutivos no banco principal.
- Verificar status HTTP e conteúdo da resposta.
- Testar cenários de sucesso e falha.
- Não utilizar Skip sem justificativa.
## 9. Execução
Comando:
dotnet test
Todos os testes deverão passar antes
da conclusão do projeto.
