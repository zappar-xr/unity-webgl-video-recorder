mergeInto(LibraryManager.library, {

   zappar_video_recorder_initialize: function(canvas, unityObject, onCompleteFunc) {
       var canva = document.querySelector(UTF8ToString(canvas));
       if(typeof canva === 'undefined' || canva === null) {
           this.recInitialized=false;
           console.log(UTF8ToString(canvas)+" not found in document");
           return false;
       }
       this.unityRecObjectListener = UTF8ToString(unityObject);
       this.unityRecOnCompleteFunc = UTF8ToString(onCompleteFunc);
       this.recorder = null;
       this.recInitialized = true;
       this.recordingInProgress = false;
       if (typeof ZapparVideoRecorder === 'undefined' || ZapparVideoRecorder === null) {
            var scr = document.createElement("script");
            scr.src="https://libs.zappar.com/zappar-videorecorder/0.1.22/zappar-videorecorder.js";
            scr.addEventListener('load', function() {
                console.log("ZapparVideoRecorder version 0.1.22 created");
                ZapparVideoRecorder.createCanvasVideoRecorder(canva, {
                    autoFrameUpdate: true,
                    audio: false,
                    maxFrameRate: 20,
                    quality: 10,
                    speed: 10,
                }).then((r)=>{
                    window.recorder = r;
                    window.recInitialized = true;
                    window.recordingInProgress = false;
                });
            });
            document.body.appendChild(scr);
        }else{       
            ZapparVideoRecorder.createCanvasVideoRecorder(canva, {
                autoFrameUpdate: true,
                audio: false,
                maxFrameRate: 20,
                quality: 10,
                speed: 10,
            }).then((r)=>{
                window.recorder = r;
                window.recInitialized = true;
                window.recordingInProgress = false;
            });
        }
        return true;
   },

   zappar_video_recorder_is_initialized : function() {
       if(typeof this.recInitialized === 'undefined') return false;
       return this.recInitialized;
   },

   zappar_video_recorder_recording_in_progress : function() {
       if(typeof this.recordingInProgress === 'undefined') return false;
       return this.recordingInProgress;
   },

   zappar_video_recorder_start : function(autoSavePrompt) {
       if(typeof this.recInitialized === 'undefined' || this.recInitialized === false) {
           console.log("video recorder not initialized");
           return false;
       }
       this.recorder.start();
       if(autoSavePrompt)
       {
            this.recorder.onComplete.bind(async res => {
                console.log("finished recording. Raising unity callback.");
                window.uarGameInstance.SendMessage(this.unityRecObjectListener, this.unityRecOnCompleteFunc, 'success');
                let a = document.createElement("a");
                a.href = await res.asDataURL();
                a.download = "UAR_Video.mp4";
                a.click();
            });
       }else{
           this.recorder.onComplete.bind(async res => {
                window.videoRecUrl = await res.asDataURL();
                window.uarGameInstance.SendMessage(this.unityRecObjectListener, this.unityRecOnCompleteFunc, 'success');
           });
       }
       this.recordingInProgress = true;
       return true;
   },

   zappar_video_recorder_stop : function() {
       if(typeof this.recordingInProgress === 'undefined' || this.recordingInProgress === false) {
           console.log("recorder not initialized or started");
           return false;
       }
       this.recorder.stop();
       this.recordingInProgress = false;
       return true;
   },

});