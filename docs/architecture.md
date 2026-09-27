# Architecture

## Decision

A **full onion around the whole site does not fit**. Optimizely content types inherit `PageData`. Putting those in a domain core is cargo-cult layering.

Use:

- Modular monolith on one (or two) deployables
- CMS as an adapter that emits DTOs
- Clean / hexagonal **only** in business modules (invoices, profile, XML integrations)
- BFF in the web host that aggregates CMS chrome + business data
- Angular as its own app

## Projects

```mermaid
flowchart TB
    visitor[Visitor]
    angular[frontend Angular]
    web[Company.Portal.Web]
    cms[Company.Portal.Cms]
    invoices[Company.Portal.Invoices]
    tests[Company.Portal.Invoices.Tests]

    visitor --> angular
    angular -->|"JSON /api"| web
    web --> cms
    web --> invoices
    tests --> invoices
```

`Company.Portal.Web` is the composition root. It owns the BFF, auth, and static host. CMS and invoices stay feature assemblies. The Angular app shares the JSON contract, not C# types.

## Runtime map

```mermaid
flowchart TD
    editor[Editor] --> copySource["Optimizely in production, StaticInvoicesPageCopy in this repo"]
    visitor[Visitor] --> client[Angular or wwwroot]
    client --> api["GET /api/me/invoices-page"]
    api --> controller["InvoicesPageController reads OIDC sub"]
    controller --> composer[InvoicesPageComposer]
    composer --> pageCopy["IProvideInvoicesPageCopy"]
    composer --> overview[ICompileInvoiceOverview]
    pageCopy --> cache[CachedInvoicesPageCopy]
    cache --> loader[ILoadInvoicesPageCopy]
    loader --> copySource
    overview --> domain["Invoice.IsOverdue"]
    overview --> store[IRetrieveInvoices]
    store --> ef["EF InMemory or SQL"]
```

## Request example

`GET /api/me/invoices-page`

- Dev identity: header `X-User-Sub` or default `user-1`
- Language: `Accept-Language` (`en` prefix → English, else Swedish)
- 401 if no subject, 404 if CMS copy missing

```mermaid
sequenceDiagram
    participant Client as Angular or wwwroot
    participant Controller as InvoicesPageController
    participant Composer as InvoicesPageComposer
    participant Copy as CachedInvoicesPageCopy
    participant Overview as CompileInvoiceOverview
    participant Store as IRetrieveInvoices

    Client->>Controller: GET /api/me/invoices-page
    alt No OIDC sub
        Controller-->>Client: 401
    else Subject present
        par Page copy
            Composer->>Copy: Get language
            Copy-->>Composer: InvoicesPageCopy or null
        and Invoices
            Composer->>Overview: Get subject and today
            Overview->>Store: ListRecent
            Store-->>Overview: invoices
            Overview-->>Composer: overview items
        end
        Composer-->>Controller: page or null
        alt CMS copy missing
            Controller-->>Client: 404
        else Copy present
            Controller-->>Client: 200 InvoicesPageResponse
        end
    end
```

## Production Azure (not referenced so the repo compiles)

```mermaid
flowchart LR
    edge[Front Door or Gateway] --> web[App Service Web]
    edge --> cmsHost[App Service CMS]
    idp[Entra ID or BankID] --> web
    web --> sql[Azure SQL]
    cmsHost --> sql
    cmsHost --> blob[Blob media]
    web --> redis[Redis DTO cache]
    web --> vault[Key Vault]
    web --> insights[App Insights]
```

CMS on its own App Service is optional. This repo keeps the static copy loader so it compiles without Optimizely packages.

## Folder rule

Projects follow features and deployables (`Invoices`, `Cms`, `Web`), not layer names.
