# Architecture

## Decision

A **full onion around the whole site does not fit**. Optimizely content types inherit `PageData`. Putting those in a domain core is cargo-cult layering.

Use:

- Modular monolith on one (or two) deployables
- CMS as an adapter that emits DTOs
- Clean / hexagonal **only** in business modules (invoices, profile, XML integrations)
- BFF in the web host that aggregates CMS chrome + business data
- Angular as its own app

## Runtime map

```
Editor → Optimizely (production) / StaticInvoicesPageCopy (this repo)
Visitor → Angular or wwwroot
        → GET /api/me/invoices-page
           → InvoicesPageController (OIDC sub)
              → InvoicesPageComposer
                 → IProvideInvoicesPageCopy (cache + CMS)
                 → ICompileInvoiceOverview
                    → Invoice (domain)
                    → IRetrieveInvoices → EF / SQL
```

## Request example

`GET /api/me/invoices-page`

- Dev identity: header `X-User-Sub` or default `user-1`
- Language: `Accept-Language` (`en` prefix → English, else Swedish)
- 401 if no subject, 404 if CMS copy missing

## Production Azure (not referenced so the repo compiles)

- App Service for Web + (optionally) CMS
- Azure SQL for invoices and Optimizely
- Blob for media
- Redis for CMS DTO cache across instances
- Key Vault for secrets
- Front Door / Gateway at the edge
- App Insights
- Entra ID or BankID broker for OIDC

## Folder rule

Projects follow features and deployables (`Invoices`, `Cms`, `Web`), not layer names.
