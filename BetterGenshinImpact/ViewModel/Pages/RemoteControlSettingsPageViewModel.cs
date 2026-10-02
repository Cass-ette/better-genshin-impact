using System;
using System.Threading.Tasks;
using BetterGenshinImpact.Core.Config;
using BetterGenshinImpact.Service.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace BetterGenshinImpact.ViewModel.Pages;

public partial class RemoteControlSettingsPageViewModel : ObservableObject, IViewModel
{
    private static readonly ILogger<RemoteControlSettingsPageViewModel> Logger =
        App.GetLogger<RemoteControlSettingsPageViewModel>();

    public AllConfig Config { get; set; }

    [ObservableProperty] private string _testStatus = string.Empty;

    public RemoteControlSettingsPageViewModel(IConfigService configService)
    {
        Config = configService.Get();
    }

    [RelayCommand]
    public void OnRegenerateToken()
    {
        Config.RemoteControlConfig.ApiToken = Guid.NewGuid().ToString("N");
        Logger.LogInformation("[RemoteControl] API Token regenerated");
    }

    [RelayCommand]
    public async Task OnTestConnectionAsync()
    {
        try
        {
            TestStatus = "测试中...";
            Logger.LogInformation("[RemoteControl] Testing connection...");

            var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("X-API-Token", Config.RemoteControlConfig.ApiToken);

            var response = await client.GetAsync($"http://127.0.0.1:{Config.RemoteControlConfig.Port}/api/status");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TestStatus = $"✓ 连接成功: {content}";
                Logger.LogInformation($"[RemoteControl] Connection test successful: {content}");
            }
            else
            {
                TestStatus = $"✗ 连接失败: {response.StatusCode}";
                Logger.LogWarning($"[RemoteControl] Connection test failed: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            TestStatus = $"✗ 错误: {ex.Message}";
            Logger.LogError(ex, "[RemoteControl] Connection test error");
        }
    }

    [RelayCommand]
    public void OnCopyToken()
    {
        System.Windows.Clipboard.SetText(Config.RemoteControlConfig.ApiToken);
        TestStatus = "✓ API Token 已复制到剪贴板";
        Logger.LogInformation("[RemoteControl] API Token copied to clipboard");
    }

    [RelayCommand]
    public void OnOpenDocumentation()
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/babalae/better-genshin-impact/blob/main/docs/REMOTE_CONTROL_PLAN.md",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "[RemoteControl] Failed to open documentation");
        }
    }
}
