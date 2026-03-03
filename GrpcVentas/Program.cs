using GrpcVentas;
using GrpcVentas.Modelo;
using GrpcVentas.Notificaciones;
using GrpcVentas.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
//builder.Services.AddSignalR();
builder.Services.AddSignalR().AddAzureSignalR(options =>
{
    options.ConnectionString = builder.Configuration["Azure:SignalR:ConnectionString"];
});

builder.Services.AddSingleton(TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ApiKeyModdleware>();
});


var app = builder.Build();
app.MapHub<clsHub>("/cnn");

// Configure the HTTP request pipeline.
app.MapGrpcService<VentasService>().EnableGrpcWeb();
app.MapGrpcService<FacturaService>().EnableGrpcWeb();
app.MapGrpcService<InventarioService>().EnableGrpcWeb();
app.MapGrpcService<PedidoService>().EnableGrpcWeb();
app.MapGrpcService<OperacionService>().EnableGrpcWeb();
app.MapGrpcService<ProductoService>().EnableGrpcWeb();
app.MapGrpcService<KardexService>().EnableGrpcWeb();
app.MapGrpcService<UsuarioService>().EnableGrpcWeb();
app.MapGrpcService<SoporteService>().EnableGrpcWeb();
app.MapGrpcService<AuditoriaService>().EnableGrpcWeb();
app.MapGrpcService<ConfiguracionService>().EnableGrpcWeb();

app.MapGrpcReflectionService();


app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.MapGet("/", () => Results.Ok("Este servidor expone endpoints gRPC y SignalR."));

app.Run();
