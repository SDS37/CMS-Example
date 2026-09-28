using Company.Portal.Cms;
using Company.Portal.Invoices;
using Company.Portal.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddInvoiceModule(builder.Configuration.GetConnectionString("Invoices"));
builder.Services.AddCmsModule();
builder.Services.AddScoped<IComposeInvoicesPage, InvoicesPageComposer>();
builder.Services.AddBffAuthentication(builder.Environment, builder.Configuration);
builder.Services.AddAuthorization();

WebApplication application = builder.Build();

using (IServiceScope scope = application.Services.CreateScope())
{
    InvoicesDbContext database = scope.ServiceProvider.GetRequiredService<InvoicesDbContext>();
    await DemoInvoiceCatalog.Seed(database, CancellationToken.None);
}

application.UseHttpsRedirection();
application.UseMiddleware<RejectCrossOriginRequests>();
application.UseDefaultFiles();
application.UseStaticFiles();
application.UseAuthentication();
application.UseAuthorization();
application.MapControllers();
application.MapFallbackToFile("index.html");
application.Run();
