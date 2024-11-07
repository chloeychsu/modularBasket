using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;

namespace Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull, IRequest<TResponse>
     where TResponse : notnull
{
     public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
     {
          logger.LogInformation($"[START] Handle request={typeof(TRequest).Name} - Response={typeof(TResponse).Name} - RequestData={request}");
          // 紀時
          var timer = new Stopwatch();
          timer.Start();
          var response = await next();
          timer.Stop();
          var timeTake = timer.Elapsed;
          if (timeTake.Seconds > 3)
               logger.LogWarning($"[PERFORMANCE] The request {typeof(TRequest).Name} took {timeTake.Seconds} seconds.");

          logger.LogInformation($"[END] Handled {typeof(TRequest).Name} with {typeof(TResponse).Name}");

          return await next();
     }
}
