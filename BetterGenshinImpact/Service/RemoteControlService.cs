using BetterGenshinImpact.Core.Config;
using BetterGenshinImpact.GameTask;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BetterGenshinImpact.Service;

public class RemoteControlService : IHostedService
{
    private readonly ILogger<RemoteControlService> _logger;
    private HttpListener? _listener;
    private CancellationTokenSource? _cts;

    public RemoteControlService(ILogger<RemoteControlService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var config = TaskContext.Instance().Config.RemoteControlConfig;
        if (!config.Enabled)
        {
            _logger.LogInformation("远程控制服务未启用");
            return Task.CompletedTask;
        }

        try
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://+:{config.Port}/");
            _listener.Start();

            _cts = new CancellationTokenSource();
            _ = Task.Run(() => ListenForRequests(_cts.Token), _cts.Token);

            _logger.LogInformation($"远程控制服务已启动，监听端口: {config.Port}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "启动远程控制服务失败");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel();
        _listener?.Stop();
        _listener?.Close();
        _logger.LogInformation("远程控制服务已停止");
        return Task.CompletedTask;
    }

    private async Task ListenForRequests(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && _listener != null)
        {
            try
            {
                var context = await _listener.GetContextAsync();
                _ = Task.Run(() => HandleRequest(context), cancellationToken);
            }
            catch (Exception ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "处理请求时发生错误");
                }
            }
        }
    }

    private async Task HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        try
        {
            // 验证 Token
            var config = TaskContext.Instance().Config.RemoteControlConfig;
            var token = request.Headers["X-API-Token"];
            if (string.IsNullOrEmpty(config.ApiToken) || token != config.ApiToken)
            {
                response.StatusCode = 401;
                await WriteResponse(response, "{\"error\":\"未授权\"}");
                return;
            }

            // CORS 支持
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-API-Token");

            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = 200;
                response.Close();
                return;
            }

            // 路由处理
            var path = request.Url?.AbsolutePath ?? "";
            var method = request.HttpMethod;

            response.ContentType = "application/json; charset=utf-8";

            if (path == "/api/status" && method == "GET")
            {
                await HandleGetStatus(response);
            }
            else if (path == "/api/start" && method == "POST")
            {
                await HandleStart(response);
            }
            else if (path == "/api/stop" && method == "POST")
            {
                await HandleStop(response);
            }
            else
            {
                response.StatusCode = 404;
                await WriteResponse(response, "{\"error\":\"未找到端点\"}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理请求时发生错误");
            response.StatusCode = 500;
            await WriteResponse(response, $"{{\"error\":\"{ex.Message}\"}}");
        }
        finally
        {
            response.Close();
        }
    }

    private async Task HandleGetStatus(HttpListenerResponse response)
    {
        var taskContext = TaskContext.Instance();
        var isRunning = taskContext.IsInitialized;
        var result = $"{{\"running\":{isRunning.ToString().ToLower()}}}";
        await WriteResponse(response, result);
    }

    private async Task HandleStart(HttpListenerResponse response)
    {
        try
        {
            TaskTriggerDispatcher.Instance().StartTimer();
            await WriteResponse(response, "{\"success\":true,\"message\":\"已启动\"}");
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            await WriteResponse(response, $"{{\"success\":false,\"error\":\"{ex.Message}\"}}");
        }
    }

    private async Task HandleStop(HttpListenerResponse response)
    {
        try
        {
            TaskTriggerDispatcher.Instance().StopTimer();
            await WriteResponse(response, "{\"success\":true,\"message\":\"已停止\"}");
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            await WriteResponse(response, $"{{\"success\":false,\"error\":\"{ex.Message}\"}}");
        }
    }

    private async Task WriteResponse(HttpListenerResponse response, string content)
    {
        var buffer = Encoding.UTF8.GetBytes(content);
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }

}
