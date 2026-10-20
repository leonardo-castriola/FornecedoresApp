# Data Model - FornecedoresApp
## 1. Banco de dados
Nome: BDFornecedores
SGBD: SQL Server 2022
ORM: Entity Framework Core
Abordagem: Code First com Migrations
## 2. Entidade Usuario
Tabela: Usuarios
Campos:
Id
- uniqueidentifier
- Chave primária
- Obrigatório
Nome
- nvarchar(100)
- Obrigatório
Login
- nvarchar(100)
- Obrigatório
- Índice único
SenhaHash
- nvarchar(500)
- Obrigatório
Ativo
- bit
- Obrigatório
DataCadastro
- datetime2
- Obrigatório
- Armazenar em UTC
## 3. Entidade Fornecedor
Tabela: Fornecedores
Campos:
Id
- uniqueidentifier
- Chave primária
- Obrigatório
RazaoSocial
- nvarchar(150)
- Obrigatório
NomeFantasia
- nvarchar(150)
- Opcional
Cnpj
- varchar(14)
- Obrigatório
- Índice único
Email
- nvarchar(150)
- Obrigatório
- Índice único
- Unicidade sem distinção entre maiúsculas e minúsculas
Endereco
- nvarchar(250)
- Obrigatório
Telefone
- nvarchar(20)
- Opcional
Ativo
- bit
- Obrigatório
- Valor inicial true
DataCadastro
- datetime2
- Obrigatório
- Armazenar em UTC
## 4. Regras de domínio
### Usuario
- O login não poderá ser vazio.
- O login deverá ser único.
- O hash de senha não poderá ser vazio.
- Somente usuários ativos poderão autenticar.
### Fornecedor
- Razão Social deverá possuir 3 a 150 caracteres.
- CNPJ deverá possuir exatamente 14 dígitos.
- CNPJ deverá passar pela validação de
 dígitos verificadores.
- CNPJ não poderá ser duplicado.
- Email deverá possuir formato válido.
- Email deverá possuir de 1 a 150 caracteres e ser único sem
 distinção entre maiúsculas e minúsculas.
- Endereço deverá possuir de 3 a 250 caracteres e ser armazenado
 sem espaços nas extremidades.
- O CNPJ não poderá ser alterado.
- Novo fornecedor deverá iniciar ativo.
- Exclusão será lógica.
## 5. Construtores e métodos
Fornecedor deverá possuir métodos que
protejam as regras da entidade:
- Criar(...)
- Atualizar(...)
- Desativar()
As propriedades necessárias para preservar
invariantes deverão possuir setters privados.
Usuario deverá permitir:
- Criar(...)
- Desativar()
## 6. Configurações do EF Core
Utilizar IEntityTypeConfiguration.
Criar:
- FornecedorMap
- UsuarioMap
Configurar:
- Nome da tabela.
- Chave primária.
- Tamanhos máximos.
- Campos obrigatórios.
- Tipos SQL.
- Índices únicos.
Criar índices únicos para:
- Usuarios.Login
- Fornecedores.Cnpj
- Fornecedores.Email
## 7. Repositórios
### IFornecedorRepository
Métodos:
- InserirAsync
- ObterPorIdAsync
- PesquisarAsync
- ExisteCnpjAsync
- ExisteEmailAsync(email, fornecedorIdIgnorado?) para validar
  unicidade no cadastro e na atualização.
- AtualizarAsync
### IUsuarioRepository
Métodos:
- ObterPorLoginAsync
Todos os métodos que consultam ou persistem dados
deverão ser assíncronos.
## 8. Migration inicial
Criar Migration:
InitialCreate
A Migration deverá criar:
- Usuarios
- Fornecedores
- Índices únicos
- Restrições de nulidade
- Chaves primárias
## 9. Usuário de desenvolvimento
Criar mecanismo idempotente para cadastrar
um usuário inicial em ambiente Development.
Login:
admin
Senha de exemplo para desenvolvimento:
Coti@2026
A senha não deverá ser gravada em texto puro.
O valor deverá ser obtido por configuração
segura de desenvolvimento.
O mecanismo deverá calcular um hash seguro
antes de persistir.
Nunca habilitar o usuário inicial fixo
em Production.
