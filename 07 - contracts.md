# Contracts - FornecedoresApp
## 1. Base URL
/api/v1/fornecedores
Todos os endpoints exigem Basic Authentication.
## 2. Criar fornecedor
POST /api/v1/fornecedores
Request:
{
 "razaoSocial": "Empresa Exemplo LTDA",
 "nomeFantasia": "Empresa Exemplo",
 "cnpj": "11222333000181",
 "email": "contato@exemplo.com.br",
 "endereco": "Rua Exemplo, 123 - Centro - Rio de Janeiro/RJ",
 "telefone": "21999999999"
}
Response 201 Created:
{
 "id": "11111111-1111-4111-8111-111111111111",
 "razaoSocial": "Empresa Exemplo LTDA",
 "nomeFantasia": "Empresa Exemplo",
"cnpj": "11222333000181",
 "email": "contato@exemplo.com.br",
 "endereco": "Rua Exemplo, 123 - Centro - Rio de Janeiro/RJ",
 "telefone": "21999999999",
 "ativo": true,
 "dataCadastro": "2026-10-13T18:00:00Z"
}
A resposta deverá incluir Location com
a URL do fornecedor criado.
Possíveis respostas:
201 - Criado
400 - Dados inválidos
401 - Não autenticado
409 - CNPJ ou Email duplicado
## 3. Listar fornecedores
GET /api/v1/fornecedores
Filtros opcionais:
- razaoSocial
- cnpj
- ativo
Exemplo:
GET /api/v1/fornecedores?ativo=true
Response 200 OK:
[
 {
 "id": "11111111-1111-4111-8111-111111111111",
 "razaoSocial": "Empresa Exemplo LTDA",
 "nomeFantasia": "Empresa Exemplo",
 "cnpj": "11222333000181",
 "email": "contato@exemplo.com.br",
 "endereco": "Rua Exemplo, 123 - Centro - Rio de Janeiro/RJ",
 "telefone": "21999999999",
 "ativo": true,
 "dataCadastro": "2026-10-13T18:00:00Z"
 }
]
## 4. Consultar fornecedor por ID
GET /api/v1/fornecedores/{id}
Response:
200 - Fornecedor encontrado
400 - ID em formato inválido
401 - Não autenticado
404 - Fornecedor não encontrado
## 5. Atualizar fornecedor
PUT /api/v1/fornecedores/{id}
Request:
{
 "razaoSocial": "Empresa Exemplo Atualizada",
 "nomeFantasia": "Novo Nome",
 "email": "novo@exemplo.com.br",
 "endereco": "Rua Atualizada, 456 - Centro - Rio de Janeiro/RJ",
 "telefone": "21988888888",
 "ativo": true
}
Response:
200 - Retornar FornecedorResponse atualizado
400 - Dados inválidos
401 - Não autenticado
404 - Fornecedor não encontrado
409 - Email já utilizado por outro fornecedor
O campo CNPJ não faz parte do contrato
de atualização.
## 6. Excluir fornecedor
DELETE /api/v1/fornecedores/{id}
Response:
204 - Fornecedor desativado
400 - ID em formato inválido
401 - Não autenticado
404 - Fornecedor não encontrado
Não retornar corpo na resposta 204.
## 7. Contrato de erros
Utilizar ProblemDetails ou
ValidationProblemDetails.
Exemplo HTTP 400:
{
 "type": "https://httpstatuses.com/400",
 "title": "Dados inválidos",
 "status": 400,
 "errors": {
 "cnpj": [
 "O CNPJ informado é inválido."
 ]
 }
}
O atributo errors deverá aparecer
nas respostas de validação de campos.
## 8. Códigos HTTP
200 - OK
201 - Created
204 - No Content
400 - Bad Request
401 - Unauthorized
404 - Not Found
409 - Conflict
500 - Internal Server Error
## 9. Regras gerais
- Utilizar JSON.
- Utilizar camelCase no JSON.
- Não expor entidades EF Core diretamente.
- Não expor SenhaHash.
- Endereço é obrigatório no cadastro e na atualização, deve conter
 de 3 a 250 caracteres e ser armazenado sem espaços externos.
- O campo ativo é obrigatório na atualização e permite reativar um
 fornecedor inativo. O CNPJ permanece imutável.
- Email duplicado, sem distinção entre maiúsculas e minúsculas,
 retorna HTTP 409 tanto no cadastro quanto na atualização.
- Utilizar DTOs de entrada e saída.
- Respostas de erro não devem expor stack traces.
