mergeInto(LibraryManager.library, {

   zappar_video_recorder_initialize: function(canvas, unityObject, onRecReadyFunc, onCompleteFunc, micAudio, fps, vidQuality, encSpeed) {
       var canva = document.querySelector(UTF8ToString(canvas));
       if(typeof canva === 'undefined' || canva === null) {
           window.recInitialized=false;
           console.log(UTF8ToString(canvas)+" not found in document");
           return false;
       }
       window.unityRecObjectListener = UTF8ToString(unityObject);
       window.unityRecReadyFunc = UTF8ToString(onRecReadyFunc);
       window.unityRecOnCompleteFunc = UTF8ToString(onCompleteFunc);
       window.recorder = null;
       window.recInitialized = false;
       window.recordingInProgress = false;
       if (typeof ZapparVideoRecorder === 'undefined' || ZapparVideoRecorder === null) {
            var scr = document.createElement("script");
            scr.src="https://libs.zappar.com/zappar-videorecorder/0.1.22/zappar-videorecorder.js";
            scr.addEventListener('load', function() {
                console.log("ZapparVideoRecorder version 0.1.22 created");
                ZapparVideoRecorder.createCanvasVideoRecorder(canva, {
                    autoFrameUpdate: true,
                    audio: micAudio,
                    maxFrameRate: fps,
                    quality: vidQuality,
                    speed: encSpeed,
                }).then((r)=>{
                    window.recorder = r;
                    window.recInitialized = true;
                    window.recordingInProgress = false;
                    window.uarGameInstance.SendMessage(window.unityRecObjectListener, window.unityRecReadyFunc, 'ready');
                });
            });
            document.body.appendChild(scr);
        }else{       
            ZapparVideoRecorder.createCanvasVideoRecorder(canva, {
                autoFrameUpdate: true,
                audio: micAudio,
                maxFrameRate: fps,
                quality: vidQuality,
                speed: encSpeed,
            }).then((r)=>{
                window.recorder = r;
                window.recInitialized = true;
                window.recordingInProgress = false;
                window.uarGameInstance.SendMessage(window.unityRecObjectListener, window.unityRecReadyFunc, 'ready');
            });
        }
        return true;
   },

   zappar_video_recorder_is_initialized : function() {
       if(typeof window.recInitialized === 'undefined') return false;
       return window.recInitialized;
   },

   zappar_video_recorder_recording_in_progress : function() {
       if(typeof window.recordingInProgress === 'undefined') return false;
       return window.recordingInProgress;
   },

   zappar_video_recorder_start : function(autoSavePrompt) {
       if(typeof window.recInitialized === 'undefined' || window.recInitialized === false) {
           console.log("video recorder not initialized");
           return false;
       }
       window.recorder.start();
       if(autoSavePrompt)
       {
            window.recorder.onComplete.bind(async res => {
                console.log("finished recording. Raising unity callback.");
                window.uarGameInstance.SendMessage(window.unityRecObjectListener, window.unityRecOnCompleteFunc, 'success');
                let a = document.createElement("a");
                a.href = await res.asDataURL();
                a.download = "UAR_Video.mp4";
                a.click();
            });
       }else{
           window.recorder.onComplete.bind(async res => {
                window.videoRecUrl = await res.asDataURL();
                window.uarGameInstance.SendMessage(window.unityRecObjectListener, window.unityRecOnCompleteFunc, 'success');
           });
       }
       this.recordingInProgress = true;
       return true;
   },

   zappar_video_recorder_stop : function() {
       if(typeof window.recordingInProgress === 'undefined' || window.recordingInProgress === false) {
           console.log("recorder not initialized or started");
           return false;
       }
       window.recorder.stop();
       window.recordingInProgress = false;
       return true;
   },

});