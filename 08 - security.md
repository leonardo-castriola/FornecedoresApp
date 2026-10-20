# Security - FornecedoresApp
## 1. Objetivo
Implementar autenticação HTTP Basic
utilizando ASP.NET Core Authentication.
## 2. Funcionamento
O cliente deverá enviar o cabeçalho:
Authorization: Basic BASE64(login:senha)
Exemplo conceitual:
login: admin
senha: Coti@2026
O cliente deverá codificar a sequência
login:senha em Base64.
Base64 não é criptografia.
## 3. Authentication Handler
Criar:
BasicAuthenticationHandler.cs
O Handler deverá:
1. Ler o cabeçalho Authorization.
2. Verificar se o esquema é Basic.
3. Decodificar as credenciais.
4. Separar login e senha.
5. Consultar IUsuarioRepository.
6. Verificar se o usuário existe.
7. Verificar se está ativo.
8. Validar a senha utilizando password hasher.
9. Criar ClaimsPrincipal autenticado.
10. Retornar sucesso ou falha de autenticação.
## 4. Armazenamento de senha
Utilizar PasswordHasher <Usuario>
do ASP.NET Core Identity.
Não é necessário implementar
o sistema completo ASP.NET Core Identity.
Não utilizar:
- Senha em texto puro.
- MD5.
- SHA256 simples.
- Hash personalizado sem salt.
## 5. Autorização
Todos os endpoints de FornecedoresController
deverão exigir autenticação.
Utilizar:
[Authorize]
O esquema padrão de autenticação
será Basic.
Não implementar perfis ou permissões
nesta primeira versão.
## 6. Respostas de autenticação
Credenciais ausentes:
HTTP 401
Credenciais inválidas:
HTTP 401
Usuário inativo:
HTTP 401
Uma resposta 401 deverá apresentar:
WWW- Authenticate: Basic realm="FornecedoresApp"
Mensagens externas não deverão revelar
se o login existe ou se a senha está incorreta.
## 7. HTTPS
Basic Authentication transmite credenciais
recuperáveis por decodificação Base64.
Em produção, HTTPS é obrigatório.
Durante a aula, permitir HTTP somente
em localhost e rede de desenvolvimento controlada.
Não disponibilizar uma API Basic Auth
em HTTP publicamente.
## 8. Swagger
Adicionar o esquema de segurança Basic
ao documento OpenAPI.
O Swagger deverá permitir informar
as credenciais para executar endpoints protegidos.
## 9. Inicialização do usuário
O usuário inicial deverá ser criado
somente no ambiente Development.
A senha deverá ser lida de configuração
segura de desenvolvimento.
Não inserir senha fixa no código - fonte.
Não registrar credenciais no repositório Git.
## 10. Testes de segurança
- Credenciais válidas devem autenticar.
- Senha inválida deve retornar 401.
- Usuário inexistente deve retornar 401.
- Usuário inativo deve retornar 401.
- Authorization ausente deve retornar 401.
- Base64 inválido deve retornar 401.
- Esquema diferente de Basic não deve autenticar.
- Usuário autenticado poderá acessar CRUD.
