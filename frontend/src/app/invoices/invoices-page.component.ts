import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { catchError, map, of, startWith } from 'rxjs';
import { InvoicesService } from './invoices.service';

@Component({
  standalone: true,
  selector: 'app-invoices-page',
  imports: [CommonModule],
  templateUrl: './invoices-page.component.html',
  styleUrl: './invoices-page.component.css',
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
