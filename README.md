# WebGL Video Recorder unity package

This package allows you to record MP4 videos of HTML canvas elements, including those with WebGL contexts. It aims to support a wide range of real-world devices at interactive frame-rates.

## Importing VideoRecorder package into Unity

If you're using [Universal AR (UAR) Unity SDK](https://github.com/zappar-xr/universal-ar-unity) version 3.1.0 or above, you can directly add this package from Zappar menu option `Zappar > Additional Packages > Add\Update WebGL Video Recorder`. Otherwise, you can import the package from the Unity editor by following these steps:
1. Opening the `Package Manager` from `Window > Package Manager` from Editor
2. Locate the `+` button on the top left corner and select `Add package from git URL...`
3. Enter the following URL: `https://github.com/zappar-xr/unity-webgl-video-recorder.git`

This will automatically fetch the latest version of the package from Github.

Another option is to add the `WebGL Video Recorder` package in your projects' `manifest.json` file located under `Root_Directory>Packages`.

```
{
  "dependencies": {
    "com.zappar.videorecorder": "https://github.com/zappar-xr/unity-webgl-video-recorder.git"
  }
}
```

Note that you can modify the source Github URL to define any particular `tag`, `branch` or `commit hash`, by adding a suffix: `#ID` to the git URL. You can read more about it here: https://docs.unity3d.com/Manual/upm-git.html

## Platform support

This library is currently supported only on Unity WebGL with the Unity version of `2021.x LTS` and above. There are placeholder APIs for editor mode development, but you would always need to build the project for testing any functionality.


## Usage

`ZVideoRecorder.cs` is the main script that exposes all the required calls and callbacks you need to use the package. The typical flow of operation is as follows:
1. Call the `ZVideoRecorder.Initialize()` at the start of the scene. Normally you would call this from MonoBehaviours' `Awake` or `Start` method which will set up the library for use.
2. Next you would want to subscribe for OnRecordingFinished event from the native plugin. The `ZVideoRecorder.RegisterOnRecordingFinished(...)` allows you to register your callback methods. Depending upon the unity canvas resolution and duration it might take some time for the plugin to process the video output after you've called to stop the video recording.
3. Call `ZVideoRecorder.DeregisterOnRecordingFinished(...)` to unsubscribe from the plugin event at the end.
4. Call `ZVideoRecorder.StartRecording()` and `ZVideoRecorder.StartRecording()` to start/stop the recording process.

The package includes two example scenes to demonstrate the usage of the plugin. The `VideoRecorderSample.scene` uses a direct browser prompt to save the video recording at the end of processing, While the `VideoRecorderPromptSample.scene` uses the [com.zappar.sns](https://github.com/zappar-xr/unity-webgl-sns) package to open web prompt to check the recording and then either save or share it. Look for the `ZVidTest.cs` and `ZVidPromptTest.cs` scripts to understand the flow.


> **Note**: Make sure to uncomment all the lines containing `Zappar.Additional.SNS.ZSaveNShare` in `ZVidPromptTest.cs` when using VideoRecording package along with the SNS package, to allow calling the SNS APIs.


## Patents and Licenses

This project includes encoders for AVC/H264 and AAC-LC. There are patent licensing implications for the usage and deployment of this software - please seek your own legal advice and obtain licenses as appropriate.

This project is built from multiple components with various licenses. They are included here:
```
Copyright (c) 2020 Trevor Sundberg, MIT License
```
```
fdk-aac: Copyright (c) 1995 - 2018 Fraunhofer-Gesellschaft zur Förderung der angewandten Forschung e.V. All rights reserved., Software License for The Fraunhofer FDK AAC Codec Library for Android
```
```
libmp4v2: libmp4v2 Contributors, Mozilla Public License version 1.1
```