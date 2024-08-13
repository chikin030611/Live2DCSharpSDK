# Live2DCSharpSDK

[中文版README](https://github.com/chikin030611/Live2DCSharpSDK/blob/master/README_ZH.md)

The SDK is designed to implement Live2D models in .NET application.

![demo](https://github.com/chikin030611/Live2DCSharpSDK/blob/master/image/demo.png)

The project was forked from [Live2DSharpSDK](https://github.com/Coloryr/Live2DCSharpSDK). `LAppWavFileHandler` and sample are added.

## Getting Started

### Prerequisites

Download the zip file of Cubism SDK from the [website](https://www.live2d.com/en/sdk/download/native/). 

### Set Up The Repo

    # Clone this repository
    git clone https://github.com/chikin030611/Live2DCSharpSDK.git

    # Go into the repository
    cd Live2DCSharpSDK.Avalonia
    
    # Run the app first, so \bin and \obj can be created
    dotnet run
    
1. Extract the Cubism SDK zip file.
2. Copy `Live2DCubismCore.dll` from `\CubismSdkForNative-5-r.1\Core\dll\windows\x86_64`.
3. Paste the `Live2DCubismCore.dll` in `\Live2DCSharpSDK.[Avalonia OR OpenTK OR WPF]\bin\Debug\net8.0`.

### Running on Terminal

    # Run the app
    dotnet run

### Running on Visual Studio

1. Open `Live2DCSharpSDK.sln` in root directory.
2. Configure startup project as **Live2DCSharp.[Avalonia OR OpenTK OR WPF]**
3. Start the project

## Modules

- **Live2DCSharpSDK.Framework**: Live2D Cubism Framework
- **Live2DCSharpSDK.App**: Live2D model renderer
- **Live2DCSharpSDK.Avalonia**: GUI built with [AvaloniaUI](https://avaloniaui.net/)
- **Live2DCSharpSDK.OpenTK**: GUI built with [OpenTK](https://opentk.net/)
- **Live2DCSharpSDK.WPF**: GUI built with **WPF** and [OpenTK](https://opentk.net/)

**Live2DCSharpSDK.Avalonia** is the only project that can be started and used on my computer. For **Live2DCSharpSDK.OpenTK**, the app crashes when launched. For **Live2DCSharpSDK.WPF**, only black screen is shown.

Only **Live2DCSharpSDK.App** AND **Live2DCSharpSDK.Framework** are required for implementation on other projects.

**Live2DCSharpSDK.App** can be modified as needed. **Live2DCSharpSDK.Framework** is not recommended to be modified unless necessary.

## How It Works

### Main Components

- `Live2DCSharpSDK.Avalonia.Avatar.UI.Live2dControl.axaml`: Defines the UI layout and elements for the **Live2dControl** user control.
- `Live2DCSharpSDK.Avalonia.Avatar.UI.Live2dControl.axaml.cs`: Contains the code-behind logic for the `Live2dControl.axaml`, managing interactions, rendering, and data binding. It also initializes the `Live2dRender.cs` instance, which is responsible for rendering the Live2D model.
- `Live2DCSharpSDK.Avalonia.Avatar.Live2dRender.cs`: Responsible for the overall rendering process within the Avalonia application.
- `Live2DCSharpSDK.App.LAppDelegate.cs`: Handles the detailed management of Live2D models, including initialization, rendering, and resource management. `Live2dRender.cs` delegates specific tasks to `LAppDelegate.cs`, making it a central component for managing Live2D interactions and rendering.
- `Live2DCSharpSDK.Avalonia.Avatar.QnaAudioManager.cs`: Manages the playback of audio files associated with Q&A objects. All questions, answers, and the names of WAVE files of the corresponding answers are stored here.

### Workflow

In `Live2dControl.axaml.cs`, new `Live2dRender.cs` instance is initialized. When the button in the layout is clicked, `StartSpeaking()` is called.

In `Live2dRender.cs`, `StartSpeaking()` calls a function in `QnaAudioManager.cs` for audio playback (model speaking). `Live2dRender.cs` also interacts with the `LAppDelegate.cs` to synchronize the mouth movements of the Live2D model with the audio playback. This synchronization is achieved by analyzing the audio sound waves and adjusting the mouth size accordingly.

Note that the model is rendered by OpenGL and the audio is played by **System.Media.SoundPlayer**. Thus, the audio player is not attached to Live2D model, resulting audio still plays when the model is disabled.

### Resources

Live2D models files and audio files of answers are stored in `\resources\models` and `\resources\audio` respectively.

## Known Issues

1. Some models may not open the mouth wide enough when speaking. `weight` can be configured to adjust the mouth movement in `LAppModel.cs:423`.
2. The mouth movement of the model is delayed the first time the audio is played. However, the mouth movement is in sync with the audio afterwards.

## Recommended Extension for Visual Studio

[Avalonia for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaVS): Previewer and templates for Avalonia applications and libraries.
 
## Related

- [Live2D-dotnet](https://github.com/chikin030611/Live2D-dotnet): Similar project with more features(e.g. saving config with no hardcoded path and better UI)
- [ColorMC](https://github.com/Coloryr/ColorMC/tree/master): the original repository of Live2D-dotnet

