using Company.Portal.Cms;
using Company.Portal.Invoices;
using Company.Portal.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddInvoiceModule(builder.Configuration.GetConnectionString("Invoices"));
builder.Services.AddCmsModule();
builder.Services.AddScoped<IComposeInvoicesPage, InvoicesPageComposer>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddAuthentication(DevelopmentAuthenticationHandler.SchemeName)
        .AddScheme<AuthenticationSchemeOptions, DevelopmentAuthenticationHandler>(
            DevelopmentAuthenticationHandler.SchemeName,
            _ => { });
}
else
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["Auth:Authority"];
            options.Audience = builder.Configuration["Auth:Audience"];
            options.MapInboundClaims = false;
        });
}

builder.Services.AddAuthorization();

WebApplication application = builder.Build();

using (IServiceScope scope = application.Services.CreateScope())
{
    InvoicesDbContext database = scope.ServiceProvider.GetRequiredService<InvoicesDbContext>();
    await DemoInvoiceCatalog.Seed(database, CancellationToken.None);
}

application.UseHttpsRedirection();
application.UseDefaultFiles();
application.UseStaticFiles();
application.UseAuthentication();
application.UseAuthorization();
application.MapControllers();
application.MapFallbackToFile("index.html");
application.Run();
