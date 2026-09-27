import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InvoicesPage } from './invoices.models';

@Injectable({ providedIn: 'root' })
export class InvoicesService {
  private readonly http = inject(HttpClient);

  getPage(): Observable<InvoicesPage> {
    return this.http.get<InvoicesPage>('/api/me/invoices-page');
  }
}
