using UnityEngine;
using System.Runtime.InteropServices;

namespace Zappar.Additional.VideoRecorder
{
    public class ZVideoRecordListener : ZWebListener
    {
        public delegate void RecorderMessages(string message);
        public event RecorderMessages OnRecorderReady;
        public event RecorderMessages OnRecordingFinished;

        public void RecorderReadyCallback(string msg)
        {
            //Debug.Log("Zappar Video Recorder is ready. Message: " + msg);
            OnRecorderReady?.Invoke(msg);
        }

        public void RecordingFinishedCallback(string result)
        {
            //Debug.Log("Finished Video Recording. Result: " + result);
            OnRecordingFinished?.Invoke(result);
        }

        public override void MessageCallback(string message)
        {
            Debug.Log("[ZWebGL]: " + message);
        }
    }
    
    [System.Serializable]
    public class ZVideoConfig
    {
        [Tooltip("Whether or not to record audio from the device microphone as part of the video")]
        public bool MicAudio;
        [Tooltip("Maximum number of frames-per-second that will be captured and encoded into the video. The actual frame-rate of the video may be lower than this value depending on the performance of the device")]
        public int MaxFrameRate;
        [Range(0f,1f), Tooltip("Controls the trade-off between video quality and file size between 0 (lowest video quality) and 1 (best video quality)")]
        public float Quality;
        [Range(0f,1f), Tooltip("Controls the trade-off between encoding time and quality of video between 0 (slowest) and 1 (fastest)")]
        public float Speed;
        public ZVideoConfig()
        {
            MicAudio = false; MaxFrameRate = 24; Quality = 0.7f; Speed = 0.7f;
        }
    };

    public class ZVideoRecorder
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        public const string PluginName = "__Internal";
#else
        public const string PluginName = "";
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_initialize(string canvas, string unityObjectName, string onRecReadyFunc, string onCompleteFunc, bool micAudio, int fps, int vidQuality, int encSpeed);
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_is_initialized();
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_recording_in_progress();
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_start(bool autoSavePrompt);
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_stop();
#else
        private static bool zappar_video_recorder_initialize(string canvas, string unityObjectName, string onRecReadyFunc, string onCompleteFunc, bool micAudio, int fps, int vidQuality, int encSpeed) { return false; }
        private static bool zappar_video_recorder_is_initialized() { return false; }
        private static bool zappar_video_recorder_recording_in_progress() { return false; }
        private static bool zappar_video_recorder_start(bool autoSavePrompt) { return false; }
        private static bool zappar_video_recorder_stop() { return false; }
#endif

        private static ZVideoRecordListener m_recordListener = null;
        
        private static void SetupListener()
        {
            if (m_recordListener == null)
            {
                GameObject go = (GameObject.FindObjectOfType<ZWebListener>()?.gameObject) ?? new GameObject(ZWebListener.UnityObjectName);
                m_recordListener = go.GetComponent<ZVideoRecordListener>() ?? go.AddComponent<ZVideoRecordListener>();
            }
        }

        public static void Initialize(ZVideoConfig config)
        {
            SetupListener();

#if UNITY_2020_1_OR_NEWER
            if (!zappar_video_recorder_initialize("#unity-canvas", ZWebListener.UnityObjectName, nameof(ZVideoRecordListener.RecorderReadyCallback), nameof(ZVideoRecordListener.RecordingFinishedCallback), config.MicAudio, config.MaxFrameRate, (int)(-41f * config.Quality + 51f), (int)(config.Speed * 10)))
#else
            if (!zappar_video_recorder_initialize("#canvas", ZWebListener.UnityObjectName, nameof(ZVideoRecordListener.RecorderReadyCallback), nameof(ZVideoRecordListener.RecordingFinishedCallback), config.MicAudio, config.MaxFrameRate, (int)(-41f * config.Quality + 51f), (int)(config.Speed * 10)))
#endif
            {
#if UNITY_EDITOR
                Debug.Log("ZVideoRecorder is not supported in editor mode.");
#else
                Debug.Log("Failed to initialize video recorder");
#endif
                return;
            }
        }

        public static bool IsRecordingInProgress()
        {
            return zappar_video_recorder_recording_in_progress();
        }

        /// <summary>
        /// </summary>
        /// <param name="autoSavePrompt">Raise prompt to save video recording automatically once the processing is finished.
        /// Set this to false if using Zappar Save and Save (SNS) package `OpenSNSVideoRecordingPrompt` method.</param>
        /// <returns></returns>
        public static bool StartRecording(bool autoSavePrompt=true)
        {
            if (!zappar_video_recorder_is_initialized()) { Debug.Log("Recording not started; Video recorder not initialized."); return false; }

            if (!zappar_video_recorder_start(autoSavePrompt))
            {
                Debug.Log("Failed to start video recording");
                return false;
            }
            return true;
        }

        public static bool StopRecording()
        {
            if (!zappar_video_recorder_is_initialized() || !zappar_video_recorder_recording_in_progress()) return false; 

            if (!zappar_video_recorder_stop())
            {
                Debug.Log("Failed to stop video recording");
                return false;
            }
            return true;
        }

        public static void RegisterVideoRecorderCallbacks(ZVideoRecordListener.RecorderMessages recorderReady = null, ZVideoRecordListener.RecorderMessages recorderFinished = null)
        {
            SetupListener();
            if(recorderReady != null) m_recordListener.OnRecorderReady += recorderReady;
            if(recorderFinished != null) m_recordListener.OnRecordingFinished += recorderFinished;
        }

        public static void DeregisterVideoRecorderCallbacks(ZVideoRecordListener.RecorderMessages recorderReady = null, ZVideoRecordListener.RecorderMessages recorderFinished = null)
        {
            SetupListener();
            if(recorderReady != null) m_recordListener.OnRecorderReady -= recorderReady;
            if(recorderFinished != null) m_recordListener.OnRecordingFinished -= recorderFinished;
        }
    }
}