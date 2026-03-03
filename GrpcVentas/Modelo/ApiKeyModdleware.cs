using Grpc.Core;
using Microsoft.AspNetCore.Authentication.OAuth;
using Grpc.Core.Interceptors;

namespace GrpcVentas.Modelo
{
    public class ApiKeyModdleware : Interceptor
    {
        private readonly IConfiguration _configuration;

        public ApiKeyModdleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var apiKey = context.RequestHeaders.Get("x-api-key")?.Value;
            var validApiKey = _configuration["ApiKey"];

            if (apiKey != validApiKey)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "API Key inválida"));
            }

            return await continuation(request, context);
        }
    }
}
