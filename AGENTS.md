# Company.Portal — Cursor agent brief

This repository is a compilable slice of a Swedish enterprise web stack inferred from a job spec: Optimizely CMS, Angular, C# / .NET, Azure, Azure DevOps.

You are working in this repo. Follow these rules unless the user overrides them.

## Product slice

Logged-in page **Mina fakturor** (`GET /api/me/invoices-page`):

1. Angular (or `wwwroot/index.html` stand-in) calls the BFF with an identity.
2. `InvoicesPageController` reads OIDC `sub` (dev: `X-User-Sub`, default `user-1`).
3. `InvoicesPageComposer` aggregates:
   - CMS chrome via `IProvideInvoicesPageCopy`
   - invoices via `ICompileInvoiceOverview`
4. Invoices module applies domain rules (`Invoice.IsOverdue`).
5. CMS module returns editor texts (static stand-in; Optimizely in production).

## Architecture (do not invent a full onion)

- Feature / deployable projects, not `Domain` / `Application` / `Infrastructure` assemblies.
- Optimizely / CMS is an **adapter**, not the domain.
- Clean / hexagonal only inside business modules (invoices, profile, integrations).
- Content reads: map CMS → DTO, cache the DTO.
- Angular is a separate app. Share JSON contracts, not C# types.
- Host (`Company.Portal.Web`) is the composition root.

Projects:

| Project | Role |
|---|---|
| `src/Company.Portal.Invoices` | Invoice domain, EF store, overview use case |
| `src/Company.Portal.Cms` | Page-copy contract, cache decorator, static/Optimizely loader |
| `src/Company.Portal.Web` | BFF, auth, static host |
| `tests/Company.Portal.Invoices.Tests` | xUnit, AAA, one concept per test |
| `frontend/` | Angular 22 app; dev server proxies `/api` to the BFF |

## C# standards (mandatory)

Combine:

- https://csharpcodingguidelines.com/ (Doomen / Aviva, C# 14)
- https://github.com/thangchung/clean-code-dotnet

When they conflict:

- No `_field` prefixes (AV1705).
- Max 3 parameters (AV1561). Use a snapshot/record instead of long ctors.
- No `Async` suffix unless a sync twin exists (AV1755).
- Early-return guards are allowed.
- Feature projects, not layer projects (AV1578).
- `ConfigureAwait(false)` in module code.
- `var` only when the type is obvious; never for `string` / `bool` / `int` (AV1520).
- Braces on new lines (AV2400).
- Types `internal sealed` unless they must cross an assembly (AV1501).
- Role-based interfaces (`IRetrieveInvoices`, not `IInvoiceRepository`).
- Records for DTOs; classes when there is behavior.
- Inject `TimeProvider`; do not call `DateTime.UtcNow` in use cases.
- Strings and collections are never null; return empty instead.
- US English identifiers. UI strings may be Swedish.
- Tests: Arrange–Act–Assert, one concept per test.
- No `#region`, no commented-out code.

## What not to do

- Do not put `PageData` / Optimizely types in the invoices module.
- Do not add Kubernetes/Docker as required infrastructure (Docker was only meriting on the job spec).
- Do not add Optimizely NuGet packages unless the user asks (private feed; repo must stay compilable).
- Do not create extra README/scaffold files without need.
- Do not weaken tests to make a change pass.

## How to verify

```bash
dotnet test Company.Portal.sln
dotnet build src/Company.Portal.Web/Company.Portal.Web.csproj
```

Dev run: `dotnet run --project src/Company.Portal.Web` then http://localhost:5088

## Production swaps (when asked)

- `StaticInvoicesPageCopy` → Optimizely `ILoadInvoicesPageCopy`
- EF InMemory → Azure SQL via `ConnectionStrings:Invoices`
- Memory cache → Azure Cache for Redis
- `DevelopmentAuthenticationHandler` → JWT Bearer / Entra / BankID broker (OIDC)
