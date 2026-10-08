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

## Executar Frontend

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

## Executar testes

```bash
npm test -- --watch=false
```


