# Company.Portal

Compilable slice from the design chat: Optimizely-style CMS + Angular contract + .NET BFF + invoices module.

## Build and test

```bash
dotnet test Company.Portal.sln
```

## Run

```bash
dotnet run --project src/Company.Portal.Web
```

Open http://localhost:5088 for the static page in `wwwroot`.

Angular UI (Node 22.22 or 24.15+):

```bash
cd frontend
npm ci
npm start
```

That dev server proxies `/api` to port 5088. `npm run build:wwwroot` copies a production build into `wwwroot`.

Development session: open `/bff/login`. The BFF sets an `__Host-bff` cookie for `user-1` unless that login request sends `X-User-Sub`. Production login is the authorization-code flow (`Auth:Authority`, `Auth:ClientId`, `Auth:ClientSecret`). The browser does not store tokens.

Empty `ConnectionStrings:Invoices` → EF InMemory. Set a SQL connection string for SQL Server.

CMS copy is static. Replace `StaticInvoicesPageCopy` with Optimizely in production.

## Docs packed from the chat

| File | Content |
|---|---|
| `docs/origin-requirements.md` | Job kravlista and inferred stack |
| `docs/architecture.md` | Why not full onion; runtime map |
| `docs/coding-standards.md` | Doomen + clean-code-dotnet |
| `docs/angular-build.md` | `ng build` and CI copy into wwwroot |
| `docs/frontend-code-standards.md` | Angular app conventions |
