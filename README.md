<h1 align="center">API .NET Core 5</h1>

<p align="center">
  <a href="#-tecnologias">Tecnologias</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#-projeto">Projeto</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#-instalação">Instalação</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#-testes">Testes</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#-docker">Docker</a>&nbsp;&nbsp;&nbsp;
</p>

## ✨ Tecnologias

Esse projeto foi desenvolvido com as seguintes tecnologias:

- [x] [JWT](https://jwt.io/)
- [x] [Cors](https://www.nuget.org/packages/Microsoft.AspNetCore.Cors/)
- [x] [xUnit](https://www.nuget.org/packages/xunit/2.4.2-pre.12)
- [x] [Docker](https://www.docker.com/)
- [x] [Swagger](https://swagger.io/)
- [x] [Postman](https://www.postman.com/)
- [x] [AutoMapper](https://automapper.org/)
- [x] [HealthChecks](https://www.nuget.org/packages/Microsoft.Extensions.Diagnostics.HealthChecks/)
- [x] [Pomelo-MySql](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/)
- [x] [API versioning](https://github.com/dotnet/aspnet-api-versioning)
- [x] [Identity Server](https://docs.microsoft.com/pt-br/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity)
- [x] [FluentAssertions](https://www.nuget.org/packages/FluentAssertions/)
- [x] [ASP.NET Core 5](https://docs.microsoft.com/pt-br/aspnet/core/?view=aspnetcore-5.0)
- [x] [Entity Framework Core](https://docs.microsoft.com/pt-br/ef/core/)


## 💻 Projeto

Este projeto é uma aplicação REST usando ASP.NET Core WebApi 5, que simula um fluxo de CRUD de produtos e categoria para uma empresa. </br></br>
Pensando em evitar duplicidade de código, diminuir o acoplamento (IoC) e evitar a complexidade os princípios SOLID, DRY, KISS e YAGNI foram aplicados.</br></br>
No quesito segurança, implementei o JWT utilizando IdentityServer.
Para documentação e versionamento visando atualizações futuras, foi usado as ferramentas Versioning ApiExplorer e Swagger.
Como ponte de monitoramento da saúde dos endpoints e do ambiente foi implantando o uso de HealthChecks; </br>
Para os testes unitários foi usado Xunit.


## 🚀 Instalação

Essas instruções fornecerão uma cópia do projeto instalado e funcional para fins de desenvolvimento e testes.

### 1º Passo - Clonar o respositório
Comece clonando esse projeto para sua máquina local.
```sh
git clone https://github.com/Shilton7/servico-catalogo
```

### 2º Passo - Banco de dados (Migrations)
```sh
- CRUD (Solution Data): update-database -Context AppDbContext
- IdentityServer (Solution Api): update-database -Context IdentityDbContext
```

![](https://i.imgur.com/BpI20Kn.png)


### 3º Passo - Configurar o ambiente
Para configurar o ambiente é necessário alterar as informações do arquivo `appsettings.json` para as informações correspondentes a conexão do seu banco de dados e chaves do JWT</br>
```sh
- Ambiente localhost/desenvolvimento: appsettings.json
```

### 4º Passo - Executando a aplicação
Depois de tudo configurado basta setar a Solution Api como o principal (Set as Startup project). </br>
Agora você pode acessar [`localhost:44343`](http://localhost:44343) do seu navegador.

## 💻 Testes

Para os testes via postman importe as collections e environments presentes na pasta postman desse projeto:
```sh
- Collections: https://github.com/Shilton7/servico-catalogo/tree/main/postman/collections
- Environments: https://github.com/Shilton7/servico-catalogo/tree/main/postman/environments
```
<strong>Postman </strong>
![](https://i.imgur.com/YO7ziSz.png)

<strong> Swagger </strong>

![](https://i.imgur.com/E7sDk87.png)

*Para os testes unitários executar o projeto: CatalogoAPIxUnitTests*

## 💻 Docker

Para fazer o up do ambiente usando containers é necessário alterar as informações dos arquivos do docker-compose para as de sua preferência:</br>
```sh
cd .\servico-catalogo\deploy\
- Ambiente localhost/desenvolvimento: docker-compose up
```
