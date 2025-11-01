using FacturadoBlazor;
using FacturadoBlazor.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Tu componente raíz (App.razor)
builder.RootComponents.Add<App>("#app");

// Si usás un HeadOutlet (para scripts o meta tags)
builder.RootComponents.Add<HeadOutlet>("head::after");

// 👉 HttpClient apuntando a tu API .NET
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7154/") // Cambiá por tu puerto real
});

// 👉 Registrás tus servicios
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<FacturaService>();

await builder.Build().RunAsync();

