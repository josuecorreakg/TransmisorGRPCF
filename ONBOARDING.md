# GrpcVentasMasivoCompleto — Project Onboarding

## What is this project?

A high-performance **ASP.NET Core 8.0 gRPC microservice** that handles bulk data ingestion for a pharmaceutical/pharmacy ERP system. It receives compressed protobuf payloads from clients, unpacks them, and persists them to per-client MySQL databases — all in a single transaction.

## Tech Stack

- **.NET 8** / ASP.NET Core gRPC (HTTP/1.1 + HTTP/2)
- **MySQL 8** via Entity Framework Core (Pomelo provider)
- **Azure SignalR** for real-time notifications
- **Protobuf 3** + GZip compression for all payloads
- API Key authentication via gRPC interceptor

## Project Structure

```
GrpcVentas/
├── Services/        # 11 gRPC service implementations
├── General/         # Business logic (clsVentas, clsFactura, clsGeneral, ...)
├── AccesoDato/      # Data access layer + clsRespuestaFactory
├── Modelo/
│   ├── DBCorporativo/   # CorporativoContext — routes to client DBs
│   └── DBVentas/        # VentasContext — all transactional entities (scaffolded)
├── Protos/          # 11 .proto service definitions
├── Notificaciones/  # SignalR hub (/cnn endpoint)
└── Program.cs       # Startup, DI, service mapping
```

## How to Run

```bash
# Build
dotnet build GrpcVentas/GrpcVentas.csproj

# Run locally (HTTP, port 5069)
dotnet run --project GrpcVentas --launch-profile http
```

Authentication: every gRPC call needs the `x-api-key` header. The key is in `appsettings.json → ApiKey`.

## Architecture in 60 seconds

1. **Request arrives** → `ApiKeyModdleware` validates the API key
2. **Service layer** decompresses the GZip payload and parses the protobuf
3. **`clsGeneralDatos.GetDatosCnn(nombrecorto)`** looks up the client's connection string in `corporativo_siif` DB
4. **`VentasContext`** is instantiated with that connection string (multi-tenant)
5. **`clsGeneral.ConvertirListaAEntidad<>()`** maps proto messages → EF entities via reflection
6. **`clsGeneral.ActualizarIdFran()`** patches franchise/location IDs
7. Bulk SQL insert (`INSERT IGNORE INTO ... VALUES (...)`) inside a transaction
8. **`clsRespuestaFactory`** returns a typed response with HTTP status code

## The 11 gRPC Services

| Service | Domain |
|---|---|
| VentasService | Sales transactions & history |
| FacturaService | Invoicing/billing |
| InventarioService | Inventory, counts, transfers |
| PedidoService | Purchase orders |
| ProductoService | Product catalog/pricing |
| KardexService | Warehouse ledger |
| UsuarioService | User accounts |
| OperacionService | Operational data |
| AuditoriaService | Audit trail |
| ConfiguracionService | System configuration |
| SoporteService | Support/help |

## Key Conventions

- All timestamps → Central Standard Time via `clsGeneral.ConvertirAZonaHoraria()`
- Proto field names **must match** EF entity property names (reflection-based mapping)
- EF models are **scaffolded** (not code-first) — edit schema externally, then re-scaffold
- No automated test suite — test via a gRPC client (e.g., Postman, grpcurl)

## Adding a New Service

1. Create `.proto` in `Protos/`
2. Add service class in `Services/` extending the generated base
3. Add business logic class in `General/`
4. Add data access class in `AccesoDato/`
5. Register in `Program.cs`: `app.MapGrpcService<YourService>()`

Protobuf codegen runs automatically on build.
