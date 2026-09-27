# Use this folder as a Cursor agent project

## Add to Cursor

1. Unzip `Company.Portal-cursor-agent.zip` (or copy this folder).
2. Cursor → **File → Open Folder** → select `Company.Portal`.
3. Start Agent / Composer in that workspace.

Cursor will pick up:

- `AGENTS.md` — standing orders
- `.cursor/rules/portal.mdc` — always-apply rule

## Good first agent prompts

- "Explain GET /api/me/invoices-page through every project."
- "Add a Cypress-style journey description for Mina fakturor."
- "Replace StaticInvoicesPageCopy with an Optimizely ILoadInvoicesPageCopy sketch that still compiles."
- "Add an invoice XML import command in the Invoices module, following AGENTS.md."

## Do not

Point the agent at a parent folder that contains other repos without this `AGENTS.md`.
