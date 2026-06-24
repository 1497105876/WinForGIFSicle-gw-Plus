# WinForGIFSicle

基于 [gifsicle](https://www.lcdf.org/gifsicle/) 的 Windows GUI 压缩工具。

## 功能

- 拖拽 / 文件选择批量添加 GIF
- 实时预览压缩效果（原图 vs 压缩后对比）
- 压缩选项：缩放比例、Lossy 有损压缩、色板限制、去除注释/扩展、隔行扫描
- 高级选项：优化级别、循环次数、帧延迟、帧清除模式
- 多文件切换浏览（左右同步切换）
- 点击图片放大预览（滚轮缩放、拖动平移、动画播放）
- 保存选项：原目录 / 桌面 / 自选目录
- 文件命名：原文件名 / 添加时间戳 / 自定义名称（完全改名 or 增加名字）
- 压缩结果列表（文件名、大小对比、压缩率、状态）

## 技术栈

- .NET Framework 4.0 WinForms
- C#
- gifsicle (命令行工具)

## 使用

1. 确保 `gifsicle.exe` 在程序同目录或 PATH 中
2. 运行 `WinForGIFSicle.exe`
3. 拖入或选择 GIF 文件，调整压缩参数，实时预览
4. 点击「开始压缩」

## 致谢

- [gifsicle](https://www.lcdf.org/gifsicle/) — Eddie Kohler 的 GIF 处理命令行工具
- [WinForGIFSicle](https://github.com/OriginalWinForGIFSicle) — 原版 GUI 项目参考
