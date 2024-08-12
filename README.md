# Live2DCSharpSDK

The SDK is designed to implement Live2D models in .NET application.

![demo](https://github.com/chikin030611/Live2DCSharpSDK/blob/master/image/demo.png)

The project was forked from [Live2DSharpSDK](https://github.com/Coloryr/Live2DCSharpSDK). ```LAppWavFileHandler``` and sample are added.

## Getting Started

### Prerequisites

- [Live2D Cubism Core](https://www.live2d.com/en/sdk/download/native/)

1. Download Cubism SDK from the website.
2. Extract the zip file.
3. Copy ```Live2DCubismCore.dll``` from ```\CubismSdkForNative-5-r.1\Core\dll\windows\x86_64```.
4. Paste the ```Live2DCubismCore.dll``` in ```\Live2DCSharpSDK.[Avalonia OR OpenTK OR WPF]\bin\Debug\net8.0```.

\* ```\bin\Debug\net8.0\``` should appear after running the application at least once.

### Using terminal to start the project
    # Clone this repository
    git clone https://github.com/chikin030611/Live2DCSharpSDK.git

    # Go into the repository
    cd Live2DCSharpSDK.Avalonia
    
    # Run the app
    dotnet run

### Using Visual Studio to start the project

1. Open ```Live2DCSharpSDK.sln``` in root directory.
2. Configure startup project as **Live2DCSharp.[Avalonia OR OpenTK OR WPF]**
3. Start the project

## How it works

### Main Components

- ```Live2dControl.axaml``` in **Live2DCSharpSDK.Avalonia**: Defines the UI layout and elements for the **Live2dControl** user control.
- ```Live2dControl.axaml.cs``` in **Live2DCSharpSDK.Avalonia**: Contains the code-behind logic for the ```Live2dControl.axaml```, managing interactions, rendering, and data binding. It also initializes the ```Live2dRender.cs``` instance, which is responsible for rendering the Live2D model.
- ```Live2dRender.cs``` in **Live2DCSharpSDK.Avalonia**: Responsible for the overall rendering process within the Avalonia application.
- ```LAppDelegate.cs``` in **Live2DCSharpSDK.App**: Handles the detailed management of Live2D models, including initialization, rendering, and resource management. ```Live2dRender.cs``` delegates specific tasks to ```LAppDelegate.cs```, making it a central component for managing Live2D interactions and rendering.
- ```QnaAudioManager.cs``` in **Live2DCSharpSDK.Avalonia**: Manages the playback of audio files associated with Q&A objects. All questions, answers, and the names of WAVE files of the corresponding answers are stored here.

### Data and Code Flow

In ```Live2dRender.cs```, ```StartSpeaking()``` calls a function in ```QnaAudioManager.cs``` for audio playback (model speaking). Note that the model is rendered by OpenGL and the audio is played by **System.Media.SoundPlayer**. Thus, the audio player is not attached to Live2D model, resulting audio still plays when the model is disabled.

### Resources

Live2D models and audio of answers are stored in ```/resources/models``` and ```/resources/audio``` respectively.

## Modules

- **Live2DCSharpSDK.Framework**: Live2D Cubism Framework
- **Live2DCSharpSDK.App**: Live2D model renderer
- **Live2DCSharpSDK.Avalonia**: GUI built with [AvaloniaUI](https://avaloniaui.net/)
- **Live2DCSharpSDK.OpenTK**: GUI built with [OpenTK](https://opentk.net/)
- **Live2DCSharpSDK.WPF**: GUI built with **WPF** and [OpenTK](https://opentk.net/)

**Live2DCSharpSDK.Avalonia** is the only project that can be started and used on my computer. For **Live2DCSharpSDK.OpenTK**, the app crashes when launched. For **Live2DCSharpSDK.WPF**, only black screen is shown.

Only the GUI projects should be modified for implementation on other projects. **Live2DCSharpSDK.App** is not recommended to be modified unless necessary.

## Known Issues

1. Some models may not open the mouth wide enough when speaking. ```weight``` can be configured to adjust the mouth movement in ```LAppModel.cs:423```.
2. The mouth movement of the model is delayed the first time the audio is played. However, the mouth movement is in sync with the audio afterwards.

## Recommended Extension for Visual Studio

[Avalonia for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaVS): Previewer and templates for Avalonia applications and libraries.
 
## Related

- [Live2D-dotnet](https://github.com/chikin030611/Live2D-dotnet): Similar project with more features(e.g. saving config with no hardcoded path and better UI)
- [ColorMC](https://github.com/Coloryr/ColorMC/tree/master): the original repo of Live2D-dotnet

