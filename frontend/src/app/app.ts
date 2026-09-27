import { Component } from '@angular/core';
import { InvoicesPageComponent } from './invoices/invoices-page.component';

@Component({
  selector: 'app-root',
  imports: [InvoicesPageComponent],
  template: '<app-invoices-page />',
})
export class App {}
