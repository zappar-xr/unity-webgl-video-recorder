using UnityEngine;
using Zappar.Additional.VideoRecorder;

public class ZVidPromptTest : MonoBehaviour
{
    public ZVideoConfig VidConfig;
    [Tooltip("Use zappar save and share package to raise the prompt for video recording")]
    public bool UseSNSPrompt = true;
    public GameObject VideoControls;

    private void Start()
    {
        VideoControls?.SetActive(false);
        Debug.Log("Uncomment all lines in this script containing references to ZSaveNShare to raise the save and share prompt");
        ZVideoRecorder.RegisterVideoRecorderCallbacks(OnRecorderReady);
        ZVideoRecorder.Initialize(VidConfig);
        //Zappar.Additional.SNS.ZSaveNShare.Initialize();
    }

    public void StartRecording()
    {
        ZVideoRecorder.RegisterVideoRecorderCallbacks(null, OnRecordingFinished);
        //Zappar.Additional.SNS.ZSaveNShare.RegisterSNSCallbacks(OnSaved, OnShared, OnPromptClosed);
        ZVideoRecorder.StartRecording(!UseSNSPrompt);
    }

    public void StopRecording()
    {
        ZVideoRecorder.StopRecording();
    }

    public void OnRecorderReady(string message)
    {
        Debug.Log("Video recorder is ready");
        VideoControls?.SetActive(true);
        ZVideoRecorder.DeregisterVideoRecorderCallbacks(OnRecorderReady);
    }

    public void OnRecordingFinished(string message)
    {
        Debug.Log("Finished video recording! Calling SNS prompt");
        ZVideoRecorder.DeregisterVideoRecorderCallbacks(null, OnRecordingFinished);
        if (UseSNSPrompt)
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
