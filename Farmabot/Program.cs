using Farmabot.Components;
using Microsoft.EntityFrameworkCore;
using Farmabot.Data;
using Farmabot.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ConexionBD>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FarmaciaDB")));

builder.Services.AddScoped<MedicamentoService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<MovimientoStockService>();
builder.Services.AddScoped<LoteMedicamentoService>();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();