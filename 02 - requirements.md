# Requirements - FornecedoresApp

## 1. Visão Geral

O fornecedoresApp será uma API para gerenciamento de fornecedores de uma empresa.
O sistema permitirá autenticar usuários e executar operações de cadastro,
consulta, atualização e exclusão de fornecedores.

## 2. Atores

### 2.1 Administrador

Usuário autenticado responsável por:

- Cadastrar fornecedores.
- Consultar fornecedores.
- Atualizar informações de fornecedores.
- Excluir fornecedores.

## 3. Requisitos Funcionais

### RF001 - Autenticar Usuário

O sistema deverá autenticar usuários utilizando HTTP Basic Authentication. 
Apenas usuários autenticados poderão acessar as funcionalidades do sistema.

Critérios de aceitação:

- Validar login e senha.
- Consultar usuário no banco de dados.
- Validar hash da senha.
- Retornar HTTP 401 Unauthorized para credenciais inválidas.
- Permitir acesso a recursos protegidos após autenticação bem-sucedida.

### RF002 - Cadastrar Fornecedor

O sistema deverá permitir que o administrador cadastre novos fornecedores.

Dados obrigatórios para cadastro:

- Razão Social.
- Endereço.
- CNPJ.
- Email.

Dados opcionais:

- Nome Fantasia.
- Telefone.

Regras:

- O CNPJ deverá ser válido.
- O CNPJ não poderá estar duplicado no sistema.
- O email deverá ser válido e não poderá estar duplicado no sistema,
  sem distinção entre maiúsculas e minúsculas.
- O fornecedor será cadastrado com status "Ativo" por padrão.

### RF003 - Consultar Fornecedor

O sistema deverá permitir listar os fornecedores cadastrados:

A consulta deverá retornar os seguintes dados:

- Identificador do fornecedor.
- Razão Social.
- Nome Fantasia.
- CNPJ.
- Email.
- Endereço.
- Telefone.
- Situação (Ativo/Inativo).
- Data de cadastro.

A consulta deverá permitir filtros adicionais por: 

- Razão Social.
- CNPJ.
- Situação (Ativo/Inativo).

### RF004 - Consultar Fornecedor por ID

O sistema deverá permitir consultar um fornecedor específico pelo seu identificador único (ID).

Caso não exista fornecedor com o ID informado, o sistema deverá retornar HTTP 404 Not Found.

### RF005 - Atualizar Fornecedor

O sistema deverá permitir atualizar:

- Razão Social.
- Nome Fantasia.
- Endereço.
- Email.
- Telefone.
- Situação (Ativo/Inativo).

O CNPJ não poderá ser alterado.

Caso o fornecedor não exista, o sistema deverá retornar HTTP 404 Not Found.

### RF006 - Excluir Fornecedor

O sistema deverá permitir excluir um fornecedor.

A exclusão será lógica.

O registro do fornecedor deverá permanecer fisicamente no banco de dados, alterando o status do fornecedor para "Inativo".

### RF007 - Validar dados

O sistema deverá validar os dados enviados.

Regras:

- Razão social de 3 a 150 caracteres.
- Nome Fantasia de 3 a 150 caracteres.
- CNPJ com 14 dígitos após normalização, válido e único; a entrada
  poderá conter formatação.
- Email formato válido e único, máximo 150 caracteres.
- Endereço de 3 a 250 caracteres.
- Telefone de 10 a 20 caracteres, podendo conter apenas números e os caracteres especiais: +, -, (, ).

### RF008 - Documentação da API

O sistema deverá fornecer documentação da API utilizando o padrão OpenAPI (Swagger) para os endpoints.

## 4. Requisitos Não Funcionais

### RFN001 - Plataforma

Utilizar .NET 10 e ASP.NET Core Web API.

### RFN002 - Arquitetura

Aplicar DDD simplificado com três projetos:

- API
- Domain
- Infrastructure

### RFN003 - Persistência

Utilizar Entity Framework Core com banco de dados SQL Server.

### RFN004 - Infraestrutura

O SQL Server deverá ser utilizado em contêineres Docker, com persistência de dados.

### RFN005 - Segurança

A API deverá utilizar HTTP Basic Authentication para autenticação de usuários.

As senhas deverão ser armazenadas de forma segura utilizando hashing (ex: BCrypt).

### RFN006 - Qualidade do Código

As operações de persistência deverão ser assíncronas.

O sistema deverá possuir testes automatizados para as funcionalidades principais, incluindo testes unitários e de integração.

### RFN007 - Documentação

O projeto deverá possuir documentação clara e objetiva, incluindo instruções de configuração, execução e testes.

## 5. Fora do escopo

Não implementar:

- Front-end.
- JWT.
- OAuth.
- Refresh Token.
- Cadastro público de usuários.
- Recuperação de senha.
- Envio de emails.
- Gestão de contratos.
- Integração com serviços externos de CNPJ.
- Arquitetura de microsserviços.
