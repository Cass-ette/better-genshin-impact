// BetterGI HTTP API 设计草案

// 需要添加的功能：

1. HTTP Server (使用 ASP.NET Core 或简单的 HttpListener)
   - 端口：本地 localhost:8080
   - 只监听本地，安全性更高

2. API 端点：

   GET  /api/status          - 获取 BetterGI 运行状态
   POST /api/task/start      - 启动任务
   POST /api/task/stop       - 停止任务
   GET  /api/task/list       - 获取任务列表
   POST /api/config/set      - 修改配置
   GET  /api/screenshot      - 获取游戏截图

3. 认证：
   - API Token (在配置文件中设置)
   - 每个请求需要 Bearer token

4. 实现位置：
   - 新建 BetterGenshinImpact/Service/Remote/
   - RemoteControlService.cs
   - ApiController.cs
   - Models/

5. 配置项 (config.json)：
   {
     "remoteControlEnabled": true,
     "remoteControlPort": 8080,
     "remoteControlToken": "your-secret-token",
     "remoteControlAllowedIPs": ["127.0.0.1"]
   }
