# BetterGI - 韩语版 (Korean Version)

> 这是 [BetterGI](https://github.com/babalae/better-genshin-impact) 的 Fork 版本，添加了完整的韩语界面支持。

## ✨ 新增功能

- ✅ **完整韩语翻译**：1531 条韩语翻译，使用原神官方术语
- ✅ **韩语 UI 支持**：在设置中可以选择韩语界面
- ✅ **自动同步官方更新**：每天自动获取官方最新功能

## 📥 下载

访问 [Releases](https://github.com/Cass-ette/better-genshin-impact/releases) 下载最新的韩语版。

## 🔧 从源码编译

### 前置要求
- .NET 8.0 SDK (x64)
- Windows 10/11 (x64)

### 编译步骤

```bash
# 克隆仓库
git clone https://github.com/Cass-ette/better-genshin-impact.git
cd better-genshin-impact

# 切换到韩语功能分支
git checkout feature/add-korean-language

# 还原依赖
dotnet restore

# 编译发布版本
dotnet build -c Release

# 编译输出位置
# BetterGenshinImpact\bin\x64\Release\net8.0-windows10.0.22621.0\BetterGI.exe
```

## 🔄 维护策略

### 自动同步（推荐）
- GitHub Actions 每天自动检查官方更新
- 自动合并并编译新版本
- 遇到冲突时会通知，需要手动解决

### 手动同步
当自动同步失败或需要立即同步时：

```bash
# 获取官方更新
git fetch upstream
git checkout main
git merge upstream/main

# 如果有冲突，解决后提交
git add .
git commit -m "Merge upstream changes"
git push origin main

# 合并到韩语分支
git checkout feature/add-korean-language
git merge main

# 再次解决冲突（如果有）
git add .
git commit -m "Merge main into Korean branch"
git push origin feature/add-korean-language

# 重新编译
dotnet build -c Release
```

### 冲突处理
**可能产生冲突的文件：**
1. `BetterGenshinImpact/View/Converters/CultureInfoNameToKVPConverter.cs`
2. `BetterGenshinImpact/ViewModel/Pages/CommonSettingsPageViewModel.cs`

**解决方法：**
- 保留韩语 `"ko" => "한국어"` 的代码
- 合并官方新增的其他语言
- 确保语言列表中包含 `"ko"`

## 📝 韩语翻译

所有翻译遵循原神韩服官方术语：
- 元素战技 → 원소전투 스킬
- 元素爆发 → 원소폭발
- 圣遗物 → 성유물
- 树脂 → 수지
- 深境螺旋 → 심연의 나선

翻译文件位置：`BetterGenshinImpact/User/I18n/ko.json`

## 🤝 贡献

欢迎帮助改进韩语翻译！

1. Fork 此仓库
2. 创建你的功能分支
3. 修改 `ko.json` 文件
4. 提交 Pull Request

## 📜 许可证

继承原项目的许可证。查看 [LICENSE](LICENSE) 了解详情。

## 🔗 相关链接

- [官方 BetterGI 仓库](https://github.com/babalae/better-genshin-impact)
- [官方文档](https://bgi.huiyadan.com/)
- [问题反馈](https://github.com/Cass-ette/better-genshin-impact/issues)

---

## 📌 更新日志

### v1.0.0-korean (2026-10-02)
- ✨ 添加完整韩语界面支持
- ✨ 1531 条韩语翻译
- ✨ 基于官方 BetterGI 最新版本
- ✨ 自动同步工作流

---

**Made with ❤️ for Korean Genshin Impact players**
