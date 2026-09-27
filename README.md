# Company.Portal

Compilable slice from the design chat: Optimizely-style CMS + Angular contract + .NET BFF + invoices module.

Drop this **entire folder** into Cursor and use Agent. Standing orders: `AGENTS.md`. How-to: `docs/cursor-agent.md`.

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

Development auth: caller is `user-1` unless you send `X-User-Sub`. Production: JWT Bearer (`Auth:Authority`, `Auth:Audience`).

Empty `ConnectionStrings:Invoices` → EF InMemory. Set a SQL connection string for SQL Server.

CMS copy is static. Replace `StaticInvoicesPageCopy` with Optimizely in production.

## Docs packed from the chat

| File | Content |
|---|---|
| `AGENTS.md` | Cursor agent brief |
| `docs/origin-requirements.md` | Job kravlista and inferred stack |
| `docs/architecture.md` | Why not full onion; runtime map |
| `docs/coding-standards.md` | Doomen + clean-code-dotnet |
| `docs/angular-build.md` | `ng build` and CI copy into wwwroot |
| `docs/cursor-agent.md` | Open this folder in Cursor |
