# Pedidos Web

Front-end em Angular 22 + TypeScript para o desafio de backend da Stefanini. Consome a API de pedidos ([Pedidos](../Pedidos)) com listagem, criação, edição e exclusão de pedidos.

## Funcionalidades

- **Listagem de pedidos** com status de pagamento, valor total formatado em R$ e detalhe expansível dos itens.
- **Criação e edição** de pedidos com formulário template-driven (`[(ngModel)]`), múltiplos itens, validação de campos e total calculado em tempo real.
- **Exclusão** com confirmação.
- Tratamento de erros da API (mensagens amigáveis, retry na listagem).

## Stack

- Angular 22 (standalone components, signals, control flow `@if`/`@for`)
- TypeScript 6
- Formulários template-driven (`[(ngModel)]`, equivalente ao `v-model` do Vue)
- Bootstrap 5 para todo o visual (sem CSS artesanal)
- HTTP com `async/await` (`firstValueFrom` sobre o HttpClient)

> **Vem do Vue?** O código está comentado com paralelos entre os conceitos Angular usados aqui e os equivalentes no Vue.

## Requisitos

- **Node.js 24 LTS** (o Angular CLI 22 exige Node ≥ 22.22 ou ≥ 24.15).
  - Nesta máquina há um Node 24 portátil em `C:\PROJETOS\tools\node24`; o script `serve.cmd` já o usa automaticamente.

## Como executar

1. Suba a API + SQL Server com Docker (na pasta `Pedidos`):

   ```bash
   docker compose up -d
   ```

   A API sobe em `http://localhost:8080` (Swagger em `/swagger`).

   Alternativa sem Docker: `dotnet run --project src/Pedidos.Api` sobe a API em `http://localhost:5019` com banco InMemory — nesse caso ajuste o `target` do `proxy.conf.json` para `http://localhost:5019`.

2. Suba o front-end:

   ```bash
   npm install   # apenas na primeira vez
   npm start
   ```

   Ou, no Windows com o Node portátil: `serve.cmd`.

3. Acesse `http://localhost:4200`.

O dev server usa proxy (`proxy.conf.json`): chamadas a `/api/*` são encaminhadas para a API .NET em `http://localhost:8080`, dispensando configuração de CORS em desenvolvimento.

## Rodando com Docker (stack completa)

Com o repositório da API clonado ao lado (`../Pedidos`) e Docker instalado:

```bash
docker compose up --build -d
```

Isso sobe **três containers**:

| Serviço | Descrição | Acesso |
| --- | --- | --- |
| `web` | Front Angular buildado, servido por nginx | http://localhost:8081 |
| `api` | API .NET 10 (definida em `../Pedidos/docker-compose.yml`) | http://localhost:8080/swagger |
| `sqlserver` | SQL Server 2022 com volume persistente | localhost,1433 |

O nginx do container `web` faz o proxy de `/api/*` para a API (papel do `proxy.conf.json` em produção), então não há CORS. As migrations do EF Core são aplicadas automaticamente quando a API sobe.

Para derrubar tudo: `docker compose down` (adicione `-v` para apagar também os dados do banco).

## Build de produção

```bash
npm run build
```

Artefatos em `dist/pedidos-web`. Em produção, sirva o front atrás do mesmo domínio da API ou ajuste a URL base em `src/app/core/services/order.service.ts` (e o CORS da API).
