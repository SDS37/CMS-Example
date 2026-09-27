# Coding standards

Sources:

- https://csharpcodingguidelines.com/ (Aviva Solutions / Dennis Doomen, through C# 14)
- https://github.com/thangchung/clean-code-dotnet (Clean Code adapted for .NET)

## Combined bar

- One purpose per type and method
- Composition over inheritance except true is-a (Optimizely `PageData`)
- KISS / YAGNI; DRY only inside a module
- Interfaces named by role
- Primary constructors when they only capture dependencies
- Records for DTOs and snapshots
- Domain value types (`Money`, `CustomerSubject`, `InvoiceId`)
- Inject `TimeProvider`
- `IReadOnlyList<T>` for collections leaving a boundary
- Encapsulate conditionals behind named methods
- Same vocabulary for the same action (`Get`, `List`)
- Fail fast on invariants; never `throw ex`
- No magic numbers; named constants
- US English identifiers

## Conflicts — locked choices

| Topic | Choice |
|---|---|
| `_repository` fields | Forbidden |
| Parameter count | Maximum 3 |
| `FooAsync` | Only if `Foo` also exists |
| Early return | Guards yes |
| Project layout | Feature assemblies |
| `var` | Only when type is evident |
| Braces | New line |

## Tests

xUnit. One concept per test. Arrange–Act–Assert. `InternalsVisibleTo` for domain types.
