# Live2DCSharpSDK

\* 此README使用了AI由英文翻譯為中文。如有任何句子不通順的問題，建議參考英文版。

該 SDK 旨在對 .NET 應用程式執行 Live2D 模型。

![demo](https://github.com/chikin030611/Live2DCSharpSDK/blob/master/image/demo.png)

此 Project 從 [Live2DSharpSDK](https://github.com/Coloryr/Live2DCSharpSDK) 複製。另外增加了 `LAppWavFileHandler` 和 Sample。

## 開始使用

### 前提條件

從[官網](https://www.live2d.com/en/sdk/download/native/)下載Cubism SDK的zip檔。請注意，不需要解壓縮 zip 檔案。

### 設定Repo

    # 複製此repo
    git clone https://github.com/chikin030611/Live2DCSharpSDK.git

    # 轉到repo
    cd Live2DCSharpSDK.Avalonia

    # 先執行應用程序，這樣就可以創建 \bin 和 \obj
    dotnet run

1. 解壓縮 Cubism SDK 壓縮檔。
2. 從`\CubismSdkForNative-5-r.1\Core\dll\windows\x86_64`複製`Live2DCubismCore.dll`。
3. 將`Live2DCubismCore.dll`貼到`\Live2DCSharpSDK.[Avalonia 或 OpenTK 或 WPF]\bin\Debug\net8.0`。

### 使用 Visual Studio 啟動項目

1. 在根目錄打開 `Live2DCSharpSDK.sln`。
2. 將啟動項目配置為 **Live2DCSharp.[Avalonia 或 OpenTK 或 WPF]**。
3. 啟動項目。

## 模組

- **Live2DCSharpSDK.Framework**：Live2D Cubism 框架
- **Live2DCSharpSDK.App**：Live2D 模型繪製器
- **Live2DCSharpSDK.Avalonia**：使用 [AvaloniaUI](https://avaloniaui.net/) 構建的GUI
- **Live2DCSharpSDK.OpenTK**：使用 [OpenTK](https://opentk.net/) 構建的GUI
- **Live2DCSharpSDK.WPF**：使用 WPF 和 [OpenTK](https://opentk.net/) 構建的GUI

**Live2DCSharpSDK.Avalonia** 是我電腦上唯一可以啟動並使用的 Project 。**Live2DCSharpSDK.OpenTK** 啟動時應用程式會當機。**Live2DCSharpSDK.WPF** 只顯示黑屏。

如需在其他 Projects 上使用 **Live2D**，只需複製 **Live2DCSharpSDK.App** 和 **Live2DCSharpSDK.Framework** 至其 Project 。

如有需要，可以自行修改 **Live2DCSharpSDK.App** 。如非必要，絕不建議修改 **Live2DCSharpSDK.Framework** 。

## 工作原理

### 主要元件

- `Live2DCSharpSDK.Avalonia.Avatar.UI.Live2dControl.axaml`：定義 **Live2dControl** 使用者控制項的UI佈局和元素。
- `Live2DCSharpSDK.Avalonia.Avatar.UI.Live2dControl.axaml.cs`：包含 `Live2dControl.axaml` 程式碼後端邏輯，管理交互、繪製和資料綁定。它也初始化 `Live2dRender.cs` ，負責呈現Live2D模型。
- `Live2DCSharpSDK.Avalonia.Avatar.Live2dRender.cs`：負責Avalonia應用程式整個繪製過程。
- `Live2DCSharpSDK.App.LAppDelegate.cs`：處理Live2D模型的詳細管理，包括初始化、繪製和資源管理。 `Live2dRender.cs` 將具體任務委派給 `LAppDelegate.cs` ，使其成為管理Live2D交互和繪製的中心元件。
- `Live2DCSharpSDK.Avalonia.Avatar.QnaAudioManager.cs`：管理與Q&A物件相關聯的音頻文件播放。所有問題、答案和相應答案WAVE文件的名稱都儲存在這裡。

### 程式碼流程

在 `Live2dControl.axaml.cs` 中，新的 `Live2dRender.cs` 實例被初始化。當您按一下佈局中的按鈕時，將呼叫 `StartSpeaking()` 。

在 `Live2dRender.cs` ， `StartSpeaking()` 調用 `QnaAudioManager.cs` 中的功能播放音頻(模型說話)。 `Live2dRender.cs` 也與 `LAppDelegate.cs` 互動，對音頻播放與Live2D模型嘴部動作進行同步。通過分析音頻聲波，就能調整嘴巴大小實現同步。

需要注意的是，模型通過OpenGL進行呈現，音頻則通過 **System.Media.SoundPlayer** 播放。因此音頻播放器並不附加於Live2D模型，在模型禁用時音頻仍能播放。

## 資源

Live2D模型和答案音頻檔案分別儲存在 `\resources\models` 和 `\resources\audio` 。

## 已知問題

1. 某些模型在說話時可能無法充分張開嘴巴。可以在 `LAppModel.cs:423` 調整 `weight` 以調整嘴部動作。
2. 首次播放音頻時，模型嘴部動作會有延遲。但之後嘴部動作會與音頻同步。

## Visual Studio 推薦擴充功能

[Avalonia for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaVS)：Avalonia應用庫的預覽器和模板。

## 相關

- [Live2D-dotnet](https://github.com/chikin030611/Live2D-dotnet)：具有更多功能(例如使用Hard Code路徑儲存配置和更好的UI)的類似Repo
- [ColorMC](https://github.com/Coloryr/ColorMC/tree/master)：Live2D-dotnet 原始Repo
