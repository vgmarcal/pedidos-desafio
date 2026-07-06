# Pedidos API

API REST em .NET para gerenciamento de pedidos, desenvolvida para o desafio de backend da Stefanini. Implementa CRUD completo de pedidos seguindo Clean Architecture, DDD e SOLID.

> **Front-end:** há um cliente Angular 22 consumindo esta API na pasta irmã [`Pedidos-WEB`](../Pedidos-WEB), com listagem, criação, edição e exclusão de pedidos. Veja o README de lá para instruções. O [README na raiz do repositório](../README.md) explica como subir a stack completa (front + API + banco) com um único `docker compose`.

## Arquitetura

O projeto é dividido em quatro camadas (Clean Architecture). Na prática, a organização equivale ao modelo clássico em N camadas do ASP.NET: `Api` é a camada de apresentação, `Application` é a camada de negócio (BLL), `Infrastructure` é a camada de acesso a dados (DAL) e `Domain` concentra as entidades/modelo:

- **Pedidos.Domain** — entidades e regras de negócio (`Order`, `OrderItem`, `Product`), sem dependências externas.
- **Pedidos.Application** — casos de uso, DTOs, interfaces de repositório e serviços (`OrderService`).
- **Pedidos.Infrastructure** — persistência com Entity Framework Core (SQL Server / InMemory), repositórios e migrations.
- **Pedidos.Api** — controllers REST, Swagger e configuração da aplicação.

```
src/
  Pedidos.Domain/
  Pedidos.Application/
  Pedidos.Infrastructure/
  Pedidos.Api/
tests/
  Pedidos.Tests/
```

Regras de negócio (validação de cliente, cálculo do valor total, snapshot de preço do produto no item) ficam encapsuladas nas entidades de domínio. A camada de aplicação orquestra casos de uso via interfaces (`IOrderRepository`, `IOrderService`), permitindo testar o serviço isoladamente com mocks (Moq), sem depender de banco de dados ou do ASP.NET.

## Modelo de dados

- `Order` (Pedido): cliente, data, status de pagamento e lista de itens. `TotalPrice` é calculado a partir dos itens.
- `OrderItem` (Item do Pedido): referência ao produto (`ProductId`), nome e preço unitário **capturados no momento da compra** (snapshot), e quantidade.
- `Product`: value object usado para validar e transportar nome/preço do produto ao criar um item de pedido (não possui tabela própria — os dados relevantes já ficam gravados no item).

O `GET` de um pedido retorna o JSON abaixo, conforme especificado no desafio:

```json
{
  "id": 0,
  "nomeCliente": "",
  "emailCliente": "",
  "pago": true,
  "valorTotal": 0.0,
  "itensPedido": [
    {
      "id": 0,
      "idProduto": 0,
      "nomeProduto": "",
      "valorUnitario": 0.0,
      "quantidade": 0
    }
  ]
}
```

## Endpoints

Base route: `api/pedidos`

| Método | Rota              | Descrição                     |
|--------|--------------------|--------------------------------|
| GET    | `/api/pedidos`      | Lista todos os pedidos         |
| GET    | `/api/pedidos/{id}` | Busca um pedido por id         |
| POST   | `/api/pedidos`      | Cria um novo pedido            |
| PUT    | `/api/pedidos/{id}` | Atualiza um pedido (substitui os itens) |
| DELETE | `/api/pedidos/{id}` | Remove um pedido               |

Exemplos de requisição estão em [`src/Pedidos.Api/Pedidos.Api.http`](src/Pedidos.Api/Pedidos.Api.http).

Erros de validação de domínio (ex.: nome/e-mail do cliente vazios) retornam **400** com corpo `application/problem+json`; um pedido inexistente retorna **404**.

## Como executar

### Opção 1 — sem Docker/SQL Server (InMemory)

Em ambiente `Development`, a aplicação usa o provider InMemory do EF Core automaticamente (`UseInMemoryDatabase: true` em `appsettings.Development.json`), sem necessidade de banco de dados instalado.

```bash
dotnet run --project src/Pedidos.Api
```

Acesse o Swagger em `https://localhost:{porta}/swagger`.

### Opção 2 — com SQL Server via Docker Compose

```bash
docker compose up --build
```

Isso sobe um container SQL Server e a API (porta `8080`), aplicando as migrations automaticamente na inicialização.

- Swagger: `http://localhost:8080/swagger`
- Connection string configurada via variável de ambiente `ConnectionStrings__DefaultConnection` no `docker-compose.yml`.

### Opção 3 — SQL Server local (fora do Docker)

Ajuste `ConnectionStrings:DefaultConnection` em `src/Pedidos.Api/appsettings.json` e rode com `ASPNETCORE_ENVIRONMENT=Production` (ou remova o `UseInMemoryDatabase` do `appsettings.Development.json`) para usar o SQL Server em vez do InMemory. As migrations são aplicadas automaticamente no start (`Database.Migrate()`), ou manualmente com:

```bash
dotnet ef database update --project src/Pedidos.Infrastructure --startup-project src/Pedidos.Api
```

## Testes

Testes unitários cobrem o método GET tanto na camada de serviço (`OrderServiceTests`) quanto na camada de apresentação/controller (`OrderControllerTests`), usando mocks (Moq) e FluentAssertions.

```bash
dotnet test
```

## Stack

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core 10 (SQL Server e InMemory providers)
- Swashbuckle (Swagger)
- xUnit, Moq, FluentAssertions
- Docker / Docker Compose
