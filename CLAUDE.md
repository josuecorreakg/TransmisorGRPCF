# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**GrpcVentasMasivoCompleto** is an ASP.NET Core 8.0 gRPC service for bulk data processing in a pharmaceutical/pharmacy ERP system. It exposes 11 gRPC service endpoints with SignalR real-time notifications and Azure SignalR integration.

- gRPC microservice with HTTP/1.1 and HTTP/2 support
- Multi-tenant architecture using corporate domains and franchises
- MySQL backend via Entity Framework Core (Pomelo provider)
- GZip compression for bulk request/response payloads
- API Key authentication via gRPC interceptor

## Build & Run

```bash
# Build
dotnet build GrpcVentas/GrpcVentas.csproj

# Run (HTTP only, port 5069)
dotnet run --project GrpcVentas --launch-profile http

# Run (HTTP + HTTPS)
dotnet run --project GrpcVentas --launch-profile https

# Publish release
dotnet publish GrpcVentas -c Release -o ./publish

# Publish for Linux container
dotnet publish GrpcVentas -c Release -r linux-x64 -o ./publish-linux
```

There are no automated tests in this project.

## Architecture

### Three-Layer Pattern

**Services Layer** (`GrpcVentas/Services/`) — 11 gRPC service classes extending protobuf-generated bases. Each validates corporate identity, decompresses the payload, calls business logic, and returns a typed response.

**Business Logic Layer** (`GrpcVentas/General/`) — Domain classes (`clsVentas`, `clsFactura`, etc.) plus `clsGeneral` for cross-cutting utilities: GZip compression/decompression, timezone normalization, and reflection-based proto-to-entity mapping.

**Data Access Layer** (`GrpcVentas/AccesoDato/`) — One class per domain, each using EF Core with `VentasContext` for transactional data and `clsGeneralDatos` for corporate metadata lookups. `clsRespuestaFactory` produces standardized success/error responses.

### Multi-Tenancy

Each request carries a `nombrecorto` (e.g., `"Alpacel"`). `clsGeneralDatos.GetDatosCnn(nombrecorto)` fetches a `DatosCorporativo` record from `CorporativoContext` (the `corporativo_siif` database), which holds the client-specific connection string and franchise mappings. `VentasContext` is then instantiated dynamically with that connection string.

### Bulk Data Flow

```
Client → gRPC request (GZip + protobuf bytes)
       → clsGeneral.DecompressData()
       → ProtoType.Parser.ParseFrom()
       → clsGeneral.ConvertirListaAEntidad<Proto, Entity>() via reflection
       → clsGeneral.ActualizarIdFran() patches franchise IDs
       → BEGIN TRANSACTION
       → manual bulk SQL via StringBuilder (INSERT IGNORE INTO ... VALUES ...)
       → COMMIT
       → clsRespuestaFactory → response with HTTP status code
```

All mutations are wrapped in a database transaction to prevent partial inserts.

### Database Contexts

| Context | Database | Purpose |
|---|---|---|
| `CorporativoContext` | `corporativo_siif` | Corporate metadata, connection routing |
| `VentasContext` | Per-client (dynamic) | All transactional entities |

Both target MySQL 8.0.28. Connection strings are hardcoded in `OnConfiguring()` with commented-out Azure variants. Models in `Modelo/DBVentas/` are scaffolded — regenerate after schema changes with:

```bash
dotnet ef dbcontext scaffold "ConnectionString" Pomelo.EntityFrameworkCore.MySql -o GrpcVentas/Modelo/DBVentas -f
```

### Authentication

All gRPC requests require the `x-api-key` header matching `ApiKey` in `appsettings.json`. Validated by `ApiKeyModdleware` interceptor; failures raise `RpcException(StatusCode.Unauthenticated)`.

### SignalR Hub

Hub mounted at `/cnn`. Clients join via `JoinConnection(cliente, clave, nombre, operacion)`. Active connections tracked in `ConnectionTracker.ConexionesActivas`. Azure SignalR distributes across instances.

## gRPC Services (11 total)

All protos in `GrpcVentas/Protos/` follow the same pattern: requests carry `bytes compressdata` (GZip protobuf), responses carry `MensajeRespuesta`, `EstatusCodigo`, and `MensajeError`.

| Proto | Domain |
|---|---|
| `protorespuestaventas.proto` | Sales transactions, historical data, costs |
| `protorespuestafacturas.proto` | Invoicing/billing |
| `protorespuestainventario.proto` | Inventory, physical counts, transfers |
| `protorespuestapedido.proto` | Purchase orders |
| `protorespuestaoperacion.proto` | Operational data |
| `protorespuestaproducto.proto` | Product catalog/pricing |
| `protorespuestakardex.proto` | Warehouse ledger entries |
| `protorespuestausuario.proto` | User accounts and credentials |
| `protorespuestacorporativo.proto` | Corporate configuration |
| `protorespuestaauditoria.proto` | Audit trail logging |
| `protorespuestaconfiguracion.proto` | System configuration |

Protobuf code generation runs automatically at build time.

## Adding a New gRPC Service

1. Add `.proto` file to `Protos/`
2. Create service class in `Services/` extending the generated base
3. Create business logic class in `General/`
4. Create data access class in `AccesoDato/`
5. Register in `Program.cs`: `app.MapGrpcService<YourService>()`

## Key Notes

- **Timezone**: All timestamps normalized to Central Standard Time via `clsGeneral.ConvertirAZonaHoraria()` before persistence.
- **Reflection mapping**: `clsGeneral.ConvertirListaAEntidad<T, U>()` maps proto fields to EF entities by name — field names in protos must match entity property names.
- **Bulk insert strategy**: Uses raw `StringBuilder` SQL (`INSERT IGNORE INTO ... VALUES (...)`) rather than EF `AddRange` for performance on large payloads.
- **No migrations**: Schema is managed externally; EF models are scaffolded, not code-first.
