using UnityEngine;
using Zappar.Additional.VideoRecorder;

public class ZVidTest : MonoBehaviour
{
    public ZVideoConfig VidConfig;
    public GameObject VideoControls;

    private void Start()
    {
        VideoControls?.SetActive(false);
        ZVideoRecorder.RegisterVideoRecorderCallbacks(OnRecorderReady);
        ZVideoRecorder.Initialize(VidConfig);
    }

    public void StartRecording()
    {
        ZVideoRecorder.RegisterVideoRecorderCallbacks(null, OnRecordingFinished);
        ZVideoRecorder.StartRecording();
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
        Debug.Log("Finished video recording!");
        ZVideoRecorder.DeregisterVideoRecorderCallbacks(null, OnRecordingFinished);
    }
}
