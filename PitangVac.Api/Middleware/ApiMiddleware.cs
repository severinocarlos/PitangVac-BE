
using log4net;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using PitangVac.Utilities.Reponses;
using PitangVac.Repository.Interface;
using Microsoft.AspNetCore.Http.Features;
using PitangVac.Utilities.Attributes;

namespace PitangVac.Api.Middleware
{
    public class ApiMiddleware : IMiddleware
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ApiMiddleware));
        private readonly ITransactionManagement _transactionManagement;

        public ApiMiddleware(ITransactionManagement transactionManagement)
        {
            _transactionManagement = transactionManagement;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            var requiredTransaction = context.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata.GetMetadata<TransactionalAttribute>();

            try
            {
                if (requiredTransaction != null)
                {
                    await _transactionManagement.BeginTransactionAsync(requiredTransaction.IsolationLevel);

                    await next.Invoke(context);

                    await _transactionManagement.CommitTransactionsAsync();
                }

                await next.Invoke(context);
                stopwatch.Stop();
                _log.InfoFormat("Serviço executado com sucesso: {0} {1} [{2} ms]", context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                if (requiredTransaction != null)
                    await _transactionManagement.RollbackTransactionsAsync();

                stopwatch.Stop();
                _log.Error($"Erro no serviço: {context.Request.Path} / Mensagem: {ex.Message} [{stopwatch.ElapsedMilliseconds}]", ex);
                await HandleException(context, ex);
            }
        }

        private static async Task HandleException(HttpContext context, Exception exception)
        {
            var response = context.Response;

            response.ContentType = "application/json";

            await response.WriteAsync(JsonConvert.SerializeObject(new DefaultResponse(HttpStatusCode.InternalServerError, GetMessages(exception))));
        }

        private static List<string> GetMessages(Exception exception)
        {
            // TODO: Implementar gerenciamento de exceções com suas mensagens respectivas
            var messages = new List<string>();
            return messages;
        }
    }
}
