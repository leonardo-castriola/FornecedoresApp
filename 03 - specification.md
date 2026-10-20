# Specification - FornecedoresApp
## 1. Escopo
A aplicação deverá disponibilizar uma API REST
para controle de fornecedores.
Todas as operações de fornecedores deverão exigir autenticação válida.
## 2. Cenário: autenticação válida
DADO que existe um usuário ativo no banco de dados
QUANDO o cliente envia um Authorization Basic
com login e senha corretos
ENTÃO a API deverá validar as credenciais
E permitir o acesso ao recurso solicitado.
## 3. Cenário: autenticação inválida
DADO que o cliente informa credenciais inválidas
QUANDO uma operação protegida é solicitada
ENTÃO a API deverá retornar HTTP 401
E incluir WWW - Authenticate com o esquema Basic
E não executar a operação solicitada.
## 4. Cenário: cadastrar fornecedor
DADO que o usuário está autenticado
E informa dados válidos
E o CNPJ não está cadastrado
QUANDO solicita o cadastro
ENTÃO o sistema deverá:
1. Validar os campos.
2. Normalizar o CNPJ removendo a formatação.
3. Validar os dígitos verificadores do CNPJ.
4. Verificar duplicidade.
5. Criar a entidade Fornecedor.
6. Definir Ativo como true.
7. Definir a data do cadastro em UTC.
8. Gravar no SQL Server.
9. Retornar HTTP 201 com o recurso criado.
## 5. Cenário: CNPJ duplicado
DADO que já existe fornecedor com determinado CNPJ
QUANDO um novo cadastro utiliza o mesmo CNPJ
ENTÃO retornar HTTP 409 Conflict
E não inserir outro fornecedor.
## 6. Cenário: CNPJ inválido
DADO que o usuário informa um CNPJ inválido
QUANDO solicita cadastro
ENTÃO retornar HTTP 400
E informar o erro de validação.
## 6.1. Cenário: Email duplicado
DADO que já existe fornecedor com determinado email
QUANDO um novo cadastro ou atualização utiliza o mesmo email,
sem distinção entre maiúsculas e minúsculas
ENTÃO retornar HTTP 409 Conflict
E não gravar a alteração.
## 6.2. Cenário: Email inválido
DADO que o usuário informa um email em formato inválido
QUANDO solicita cadastro ou atualização
ENTÃO retornar HTTP 400
E informar o erro de validação.
## 7. Cenário: consultar fornecedores
DADO que existem fornecedores cadastrados
QUANDO o usuário solicita a listagem
ENTÃO retornar HTTP 200
E uma coleção JSON de fornecedores.
Se não houver registros, retornar uma coleção vazia.
## 8. Cenário: pesquisar fornecedores
DADO que existem fornecedores cadastrados
QUANDO o usuário informa filtros opcionais
ENTÃO retornar somente os fornecedores
que atendam aos filtros.
O filtro de razão social será parcial e
sem distinção entre maiúsculas e minúsculas.
O filtro de CNPJ será exato após normalização.
O filtro de situação será exato.
## 9. Cenário: consultar por ID
DADO que existe fornecedor com o ID informado
QUANDO o usuário solicita sua consulta
ENTÃO retornar HTTP 200 com seus dados.
Caso não exista, retornar HTTP 404.
## 10. Cenário: atualizar fornecedor
DADO que existe fornecedor cadastrado
QUANDO o usuário solicita atualização válida
ENTÃO atualizar os campos permitidos, incluindo Endereço e Situação
E preservar o CNPJ
E retornar HTTP 200 com os dados atualizados.
## 11. Cenário: excluir fornecedor
DADO que existe fornecedor ativo
QUANDO o usuário solicita a exclusão
ENTÃO alterar sua situação para Inativo
E retornar HTTP 204.
O registro não deverá ser removido fisicamente.
## 12. Cenário: fornecedor inativo
DADO que um fornecedor está inativo
QUANDO o usuário consulta por ID
ENTÃO seus dados poderão ser retornados
com Ativo igual a false.
Um fornecedor inativo poderá ser listado
quando os filtros permitirem.
Uma segunda exclusão deverá retornar HTTP 204,
sem alterar novamente o registro.
Se a atualização informar ativo igual a true, o fornecedor deverá
ser reativado e a API deverá retornar HTTP 200.
## 13. Regras adicionais
- Todo CNPJ será armazenado somente com números.
- CNPJ será único entre ativos e inativos.
- Email será armazenado sem espaços nas extremidades.
- Razão Social será armazenada sem espaços externos.
- Endereço será armazenado sem espaços nas extremidades.
- Email será comparado sem distinção entre maiúsculas e minúsculas
 para aplicar a regra de unicidade.
- Todas as datas serão armazenadas em UTC.
- Atualizações não poderão alterar o ID.
- A autenticação não criará sessão no servidor.
