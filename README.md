# FornecedoresApp

## Banco de dados local (Etapa 2)

O ambiente de desenvolvimento usa SQL Server 2022 em contêiner Docker. A porta fica disponível somente na máquina local, em `localhost,1434`. O Compose atual usa o volume Docker `fornecedores_sqlserver_data_v2`, que começa vazio; o volume anterior `fornecedores_sqlserver_data` é preservado sem alterações.

### Pré-requisitos

- Docker Desktop ou Docker Engine em execução, com Docker Compose.
- .NET SDK 10 para executar a API.
- Visual Studio com suporte a Docker Compose para iniciar a solução em containers pelo IDE.

### Configurar a senha do SQL Server

Copie `.env.example` para `.env` e configure `MSSQL_SA_PASSWORD` e `DEVELOPMENT_USER_PASSWORD`. O arquivo `.env` é ignorado pelo Git; não versione essas senhas.

Para que a API use a mesma senha sem gravá-la nos arquivos do projeto, registre a connection string nos User Secrets do projeto da API. No PowerShell, informe o mesmo valor definido no `.env` quando solicitado:

```powershell
$saPassword = Read-Host "Senha MSSQL_SA_PASSWORD definida no .env"
$connectionString = "Server=127.0.0.1,1434;Database=BDFornecedores;User Id=sa;Password=$saPassword;Encrypt=True;TrustServerCertificate=True"
dotnet user-secrets set "ConnectionStrings:BDFornecedores" $connectionString --project src/FornecedoresApp.API/FornecedoresApp.API.csproj
Remove-Variable saPassword, connectionString
```

Configure também, antes da primeira inicialização da API em Development, a senha do usuário inicial `admin`. Ela será armazenada somente como hash no banco:

```powershell
$adminPassword = "Coti@2026"
dotnet user-secrets set "DevelopmentUser:Password" $adminPassword --project src/FornecedoresApp.API/FornecedoresApp.API.csproj
Remove-Variable adminPassword
```

No Swagger local (`http://localhost:5213/swagger`), autentique com o usuário `admin` e a senha `Coti@2026`.

O seed é executado somente em Development e cria o usuário `admin` apenas se ele ainda não existir. Alterar esse segredo depois que o usuário já foi criado não troca a senha persistida.

Use uma senha que atenda à política do SQL Server e evite ponto e vírgula ou aspas neste ambiente local, pois esses caracteres exigem escape na connection string.

Cada volume Docker preserva a senha `sa` definida na primeira inicialização. Alterar `MSSQL_SA_PASSWORD` no `.env` não troca a senha de um volume existente. O volume anterior `fornecedores_sqlserver_data` continua preservado e independente do volume novo usado pelo Compose.

### Iniciar e parar o SQL Server

Na raiz do projeto, execute para iniciar o SQL Server e a API containerizada:

```powershell
docker compose up -d --build
docker compose ps
```

O serviço SQL Server estará pronto quando o estado mostrar `healthy`. O health check executa `SELECT 1`. A API aguarda essa condição e conecta ao SQL Server pelo nome de serviço `sqlserver` na rede interna do Compose. O Swagger da API containerizada fica em `http://localhost:5214/swagger/index.html`; a porta é publicada somente em loopback.

Para executar pelo Visual Studio no host, selecione o perfil `http`; o Swagger abrirá em `http://localhost:5213/swagger/index.html`. Para executar a composição de containers pelo Visual Studio, selecione o projeto de inicialização `FornecedoresApp.DockerCompose` e o perfil Docker Compose; o navegador abrirá o Swagger em `http://localhost:5214/swagger/index.html`. Pelo terminal, `docker compose up -d --build` inicia os mesmos serviços; nesse caso, acesse essa URL manualmente.

Para parar o contêiner sem apagar os dados:

```powershell
docker compose down
```

O `docker-compose.yml` define `ACCEPT_EULA=Y`; iniciar o serviço confirma o aceite dos termos de licença aplicáveis ao SQL Server. A imagem Developer é destinada a desenvolvimento e testes.
