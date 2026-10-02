# BetterGI Remote Control - 完整方案

## 架构图

```
[手机浏览器]
    ↓ HTTPS
[公网服务器 - Go Backend]
    ↓ SSH Tunnel / 内网穿透
[本地 PC - BetterGI + HTTP API]
```

## 组件

### 1. BetterGI HTTP API (需要开发)
- **语言**: C# (.NET)
- **位置**: BetterGI 源码中添加
- **监听**: localhost:8080
- **功能**:
  - 启动/停止任务
  - 查询状态
  - 修改配置
  - 获取截图

### 2. 公网服务器 (复用 MAA Remoter)
- **语言**: Go
- **仓库**: `Cass-ette/ArknightsMaaRemoter-`
- **功能**:
  - 接收手机请求
  - 转发到本地 BetterGI
  - WebSocket 实时推送
  - 多设备管理

### 3. 连接方式选择

#### 选项 A: SSH 隧道 (推荐)
```bash
# 在本地 PC 执行
ssh -R 8080:localhost:8080 user@your-server.com

# 优点：
- 安全（SSH 加密）
- 简单（不需要修改防火墙）
- 稳定

# 缺点：
- 需要保持 SSH 连接
- 断线需要重连
```

#### 选项 B: frp 内网穿透
```ini
# frpc.ini
[bettergi-api]
type = http
local_ip = 127.0.0.1
local_port = 8080
custom_domains = bettergi.your-domain.com

# 优点：
- 自动重连
- 支持多协议
- 配置灵活

# 缺点：
- 需要额外配置 frp
```

#### 选项 C: Cloudflare Tunnel
```bash
cloudflared tunnel --url http://localhost:8080

# 优点：
- 免费
- 自动 HTTPS
- 不需要公网 IP

# 缺点：
- 依赖 Cloudflare
- 可能有延迟
```

## 实施步骤

### Phase 1: BetterGI HTTP API 开发
1. 在 BetterGI 中添加 HTTP Server
2. 实现基本 API 端点
3. 本地测试 (curl/Postman)

### Phase 2: Go 后端扩展
1. Fork 你的 MAA Remoter
2. 添加 BetterGI 适配器
3. 支持多游戏切换

### Phase 3: Web 界面复用
1. 修改你的 MAA Web 界面
2. 添加 BetterGI 控制面板
3. 统一的多游戏管理

### Phase 4: 部署测试
1. 本地测试完整链路
2. 部署到公网服务器
3. 手机端测试

## 技术栈

- **BetterGI API**: C# + ASP.NET Core Minimal API
- **后端**: Go + Gin/Echo
- **前端**: 复用现有 MAA Web 界面
- **连接**: SSH Tunnel / frp / Cloudflare
- **认证**: JWT Token

## API 示例

### 启动任务
```bash
POST http://your-server/api/bettergi/task/start
Authorization: Bearer your-token
Content-Type: application/json

{
  "device_id": "pc-home",
  "task_type": "domain",
  "config": {
    "domain_name": "忘却之峡",
    "runs": 5
  }
}
```

### 查询状态
```bash
GET http://your-server/api/bettergi/status?device_id=pc-home
Authorization: Bearer your-token

Response:
{
  "status": "running",
  "current_task": "auto_domain",
  "progress": "3/5",
  "screenshot": "base64..."
}
```

## 安全考虑

1. **认证**: JWT Token + API Key
2. **加密**: HTTPS/SSH 隧道
3. **权限**: 每个 token 绑定设备
4. **审计**: 记录所有远程操作
5. **限流**: 防止 API 滥用

## 预估工作量

- BetterGI HTTP API: 2-3 天
- Go 后端扩展: 1-2 天
- Web 界面修改: 1 天
- 测试部署: 1 天

**总计**: 约 5-7 天开发时间

## 后续扩展

- 多台 PC 同时管理
- 定时任务调度
- 远程配置管理
- 日志实时查看
- 性能监控
