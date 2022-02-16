using UnityEngine;
using System.Runtime.InteropServices;

namespace Zappar.Additional.VideoRecorder
{
    public class ZVideoRecordListener : ZWebListener
    {
        public delegate void RecordingFinished(string result);
        public event RecordingFinished OnRecordingFinished;

        public void RecordingFinishedCallback(string result)
        {
            //Debug.Log("Finished Video Recording. Result: " + message);
            OnRecordingFinished?.Invoke(result);
        }

        public override void MessageCallback(string message)
        {
            Debug.Log("[ZWebGL]: " + message);
        }
    }

    public class ZVideoRecorder
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        public const string PluginName = "__Internal";
#else
        public const string PluginName = "";
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_initialize(string canvas, string unityObjectName, string onCompleteFunc);
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_is_initialized();
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_recording_in_progress();
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_start(bool autoSavePrompt);
        [DllImport(PluginName)]
        private static extern bool zappar_video_recorder_stop();
#else
        private static bool zappar_video_recorder_initialize(string canvas, string unityObjectName, string onCompleteFunc) { return false; }
        private static bool zappar_video_recorder_is_initialized() { return false; }
        private static bool zappar_video_recorder_recording_in_progress() { return false; }
        private static bool zappar_video_recorder_start(bool autoSavePrompt) { return false; }
        private static bool zappar_video_recorder_stop() { return false; }
#endif

        private static ZVideoRecordListener m_recordListener = null;

        public static void Initialize()
        {
            if (m_recordListener == null)
            {
                GameObject go = (GameObject.FindObjectOfType<ZWebListener>()?.gameObject) ?? new GameObject(ZWebListener.UnityObjectName);
                m_recordListener = go.GetComponent<ZVideoRecordListener>() ?? go.AddComponent<ZVideoRecordListener>();
            }

#if UNITY_2020_1_OR_NEWER
            if (!zappar_video_recorder_initialize("#unity-canvas", ZWebListener.UnityObjectName, nameof(ZVideoRecordListener.RecordingFinishedCallback)))
#else
            if(!zappar_video_recorder_initialize("#canvas", ZWebListener.UnityObjectName, nameof(ZVideoRecordListener.RecordingFinishedCallback)))
#endif
            {
                Debug.Log("Failed to initialize video recorder");
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
            if (!zappar_video_recorder_is_initialized()) { Debug.Log("recording not started; not init"); return false; }

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

        public static void RegisterOnRecordingFinished(ZVideoRecordListener.RecordingFinished callback)
        {
            m_recordListener.OnRecordingFinished += callback;
        }

        public static void DeregisterOnRecordingFinished(ZVideoRecordListener.RecordingFinished callback)
        {
            m_recordListener.OnRecordingFinished -= callback;
        }
    }
}