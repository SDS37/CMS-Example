export interface UserInvoice {
  id: string;
  amount: number;
  currency: string;
  dueDate: string;
  status: string;
  isOverdue: boolean;
}

export interface InvoicesPage {
  heading: string;
  introductionHtml: string;
  helpHtml: string;
  invoices: UserInvoice[];
}
