import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { catchError, map, of, startWith } from 'rxjs';
import { InvoicesService } from './invoices.service';

@Component({
  standalone: true,
  selector: 'app-invoices-page',
  imports: [CommonModule],
  styles: [
    `
      :host {
        display: block;
        font-family: sans-serif;
        margin: 2rem;
        max-width: 40rem;
      }
      .overdue {
        color: #b00020;
      }
      li {
        margin: 0.4rem 0;
      }
    `,
  ],
  template: `
    @if (view().status === 'loading') {
      <p>Laddar fakturor…</p>
    } @else if (view().status === 'error') {
      <p>Kunde inte hämta sidan. Försök igen.</p>
    } @else {
      <h1>{{ view().page?.heading }}</h1>
      <div [innerHTML]="view().page?.introductionHtml"></div>
      <ul>
        @for (invoice of view().page?.invoices ?? []; track invoice.id) {
          <li [class.overdue]="invoice.isOverdue">
            {{ invoice.amount | number: '1.2-2' }} {{ invoice.currency }}
            — due {{ invoice.dueDate }}
            @if (invoice.isOverdue) { <span>Overdue</span> }
          </li>
        }
      </ul>
      <aside [innerHTML]="view().page?.helpHtml"></aside>
    }
  `
})
export class InvoicesPageComponent {
  private readonly invoices = inject(InvoicesService);

  readonly view = toSignal(
    this.invoices.getPage().pipe(
      map((page) => ({ status: 'ok' as const, page })),
      catchError(() => of({ status: 'error' as const, page: undefined })),
      startWith({ status: 'loading' as const, page: undefined })
    ),
    { initialValue: { status: 'loading' as const, page: undefined } }
  );
}
