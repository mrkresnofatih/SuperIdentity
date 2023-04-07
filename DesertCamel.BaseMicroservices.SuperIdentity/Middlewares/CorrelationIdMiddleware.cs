using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Middlewares
{
    public class CorrelationIdMiddleware : IMiddleware
    {
        private readonly ICorrelationIdUtility _correlationIdUtility;
        private const string _CorrelationIdHeader = "X-Correlation-Id";

        public CorrelationIdMiddleware(
            ICorrelationIdUtility correlationIdUtility)
        {
            _correlationIdUtility = correlationIdUtility;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var correlationId = GetCorrelationId(context);
            AddCorrelationIdHeaderToResponse(context, correlationId);

            await next(context);
        }

        private string GetCorrelationId(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(_CorrelationIdHeader, out var correlationId))
            {
                _correlationIdUtility.Set(correlationId.ToString());
                return correlationId;
            }
            else
            {
                return _correlationIdUtility.Get();
            }
        }

        private void AddCorrelationIdHeaderToResponse(HttpContext context, string correlationId)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(_CorrelationIdHeader, new[] { correlationId });
                return Task.CompletedTask;
            });
        }
    }
}
