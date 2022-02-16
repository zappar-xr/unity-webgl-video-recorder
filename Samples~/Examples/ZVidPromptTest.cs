using UnityEngine;
using Zappar.Additional.VideoRecorder;

public class ZVidPromptTest : MonoBehaviour
{
    [Tooltip("Use zappar save and share package to raise the prompt for video recording")]
    public bool UseSNSMethodToSaveVideo = false;

    private void Start()
    {
        Debug.Log("Uncomment all lines in this script containing references to ZSaveNShare to raise the save and share prompt");
        //Zappar.Additional.SNS.ZSaveNShare.Initialize();
        ZVideoRecorder.Initialize();
    }

    public void StartRecording()
    {
        ZVideoRecorder.RegisterOnRecordingFinished(OnRecordingFinished);
        //Zappar.Additional.SNS.ZSaveNShare.RegisterSNSCallbacks(OnSaved, OnShared, OnPromptClosed);
        ZVideoRecorder.StartRecording(!UseSNSMethodToSaveVideo);
    }

    public void StopRecording()
    {
        ZVideoRecorder.StopRecording();
    }

    public void OnRecordingFinished(string message)
    {
        Debug.Log("Finished video recording! Calling SNS prompt");
        ZVideoRecorder.DeregisterOnRecordingFinished(OnRecordingFinished);
        if (UseSNSMethodToSaveVideo)
        {
            // if (Zappar.Additional.SNS.ZSaveNShare.IsInitialized())
            //     Zappar.Additional.SNS.ZSaveNShare.OpenSNSVideoRecordingPrompt();
            // else
            // {
            //     Debug.Log("SNS library not initialized");
            // }
        }
    }

    public void OnSaved()
    {
        Debug.Log("Prompt saved");
    }

    public void OnShared()
    {
        Debug.Log("Prompt shared");
    }

    public void OnPromptClosed()
    {
        Debug.Log("Save and share prompt closed");
        //Zappar.Additional.SNS.ZSaveNShare.DeregisterSNSCallbacks(OnSaved, OnShared, OnPromptClosed);
    }
}
