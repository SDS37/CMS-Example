describe('Mina fakturor', () => {
  it('shows the invoices from the published test site', () => {
    cy.visit('/');

    cy.contains('h1', 'My invoices');
    cy.contains('Overdue');
    cy.contains('SEK');
  });
});
