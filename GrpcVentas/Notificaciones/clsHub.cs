using GrpcVentas.AccesoDato;
using GrpcVentas.General;
using GrpcVentas.Modelo;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GrpcVentas.Notificaciones
{
    public class clsHub:Hub
    {
        private static readonly Dictionary<string, string> ConnectedClients = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            //return Task.CompletedTask;
            var connectionId = Context.ConnectionId;
            ConnectionTracker.ConexionesActivas[connectionId] = DateTime.UtcNow;

            Console.WriteLine($"🟢 Cliente conectado: {connectionId} ({DateTime.UtcNow})");
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.ConnectionId;
            string? nombreCliente = null;

            lock (ConnectedClients)
            {
                if (ConnectedClients.TryGetValue(connectionId, out nombreCliente))
                {
                    ConnectedClients.Remove(connectionId);
                    ConnectionData.ExitConnection(Context.ConnectionId, nombreCliente);
                }
            }

            //Se agregaLinea
            ConnectionTracker.ConexionesActivas.TryRemove(connectionId, out _);
            return base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("JoinConnection")]
        public async Task JoinConnection(string cliente, string clave, string nombre, int operacion)
        {
            try
            {
                string connectionId = Context.ConnectionId;
                string? nombreCliente = cliente;
                if (!string.IsNullOrEmpty(nombreCliente))
                {
                    lock (ConnectedClients) // Evita problemas de concurrencia
                    {
                        ConnectedClients[connectionId] = nombreCliente;
                    }
                }

                ConnectionData.JoinConnection(Context.ConnectionId, cliente, clave, nombre, operacion);
                await Clients.Client(Context.ConnectionId).SendAsync("AwaitConnection", "Se conecto al servidor mediante SignalR");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            
        }

        public async Task SendNotificaciones(string cliente, string connectionId, int operacion , string FechaInicio, string FechaFin)
        {
            var notificacion = new Notificacion(operacion, FechaInicio, FechaFin);
            await Clients.Client(connectionId).SendAsync("SendNotificaciones", notificacion);
        }

        public static class ConnectionTracker
        {
            // Guardamos los connectionId activos
            public static ConcurrentDictionary<string, DateTime> ConexionesActivas = new();
        }
    }
}
