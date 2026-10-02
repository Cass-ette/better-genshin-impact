using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BetterGenshinImpact.Core.Config;

/// <summary>
/// 远程控制配置
/// </summary>
[Serializable]
public partial class RemoteControlConfig : ObservableObject
{
    /// <summary>
    /// 是否启用远程控制
    /// </summary>
    [ObservableProperty]
    private bool _enabled = false;

    /// <summary>
    /// HTTP API 监听端口
    /// </summary>
    [ObservableProperty]
    private int _port = 8080;

    /// <summary>
    /// API 访问令牌（用于身份验证）
    /// </summary>
    [ObservableProperty]
    private string _apiToken = string.Empty;

    /// <summary>
    /// 是否允许远程启动
    /// </summary>
    [ObservableProperty]
    private bool _allowRemoteStart = true;

    /// <summary>
    /// 是否允许远程停止
    /// </summary>
    [ObservableProperty]
    private bool _allowRemoteStop = true;
}
