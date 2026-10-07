## Executar o backend

Pré-requisito: SDK do .NET 10 e SQL Server LocalDB.

```bash
cd backend
dotnet restore
dotnet build
dotnet run --project DesafioVendas.Api
```

Swagger:
`https://localhost:7043/swagger`

A connection string padrão é:

```text
Server=(localdb)\MSSQLLocalDB;Database=DesafioVendas;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

Criar o banco via migration:

```bash
dotnet ef migrations add InitialCreate --project DesafioVendas.Infrastructure --startup-project DesafioVendas.Api
dotnet ef database update --project DesafioVendas.Infrastructure --startup-project DesafioVendas.Api
```


## Executar testes

```bash
dotnet test
```

## Executar Angular

Pré-requisito: Node compatível com Angular 13.

```bash
cd frontend/desafio-vendas-angular
npm install
npm start
```

Acesse:

`http://localhost:4200`

O Angular envia cada venda importada para:

`POST https://localhost:7043/api/vendas`

A URL da API está em:

`src/app/core/services/vendas.service.ts`

## Arquitetura

```text
Angular 13
   |
   | HTTP/JSON
   v
ASP.NET Core Web API
   |
   +-- Controllers
   |
   +-- IVendaRepository
   |
   +-- VendaRepository
   |
   +-- Entity Framework Core
   |
   v
SQL Server / LocalDB
```


