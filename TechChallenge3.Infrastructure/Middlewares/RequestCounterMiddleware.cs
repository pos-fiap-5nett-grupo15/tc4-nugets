using Microsoft.AspNetCore.Http;
using Prometheus;
using System.Diagnostics;
using System.Net;

namespace TechChallenge3.Infrastructure.Middlewares
{
    public class RequestCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Histogram RequestAnalyzerInfo = Metrics.CreateHistogram(
            "http_requests_info_total",
            "Informações customizadas sobre a requisição",
            new HistogramConfiguration
            {
                LabelNames = new[] { "path", "request_status" },
                Buckets = Histogram.ExponentialBuckets(0.001, 2, 10)
            }
        );

        public RequestCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestTime = Stopwatch.StartNew();
            var requestPath = context.Request.Path.ToString() ?? "unknown";

            await _next(context);

            if (!requestPath.Contains("metric", StringComparison.InvariantCultureIgnoreCase))
            {
                var responseStatus = ((HttpStatusCode)context.Response.StatusCode).ToString() ?? "unknown";

                requestTime.Stop();

                requestPath = requestPath.Split('/').First(path => !string.IsNullOrEmpty(path)) ?? requestPath;
                requestPath = $"{requestPath.Trim()}({context.Request.Method?.ToLower() ?? "unknown"})";

                RequestAnalyzerInfo
                    .WithLabels(requestPath.ToLower().Trim(),
                                responseStatus.ToLower().Trim())
                    .Observe(requestTime.Elapsed.TotalSeconds);
            }
        }
    }
}
