using UnityEngine;
using Zappar.Additional.VideoRecorder;

public class ZVidTest : MonoBehaviour
{
    private void Start()
    {
        ZVideoRecorder.Initialize();
    }

    public void StartRecording()
    {
        ZVideoRecorder.RegisterOnRecordingFinished(OnRecordingFinished);
        ZVideoRecorder.StartRecording();
    }

    public void StopRecording()
    {
        ZVideoRecorder.StopRecording();
    }

    public void OnRecordingFinished(string message)
    {
        Debug.Log("Finished video recording!");
        ZVideoRecorder.DeregisterOnRecordingFinished(OnRecordingFinished);
    }
}
