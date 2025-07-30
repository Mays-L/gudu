using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    /// <summary>
    /// Sound efffects handler
    /// </summary>
    class AudioManager : Singleton<AudioManager>
    {
        #region Fields
        public AudioSource _AudioSource;
        private readonly string AudioFolder = "General/Audios/";

        #region Clips
        public AudioClip TrueAnswerSound;
        public AudioClip FalseAnswerSound;
        public AudioClip MainClip;
        public AudioClip StopClip;
        public AudioClip TimeSound;
        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Unity awake method
        /// </summary>
        public void Start()
        {
            #region Add Listeners
            EventManager.Instance.AddListeners("TrueAnswerEvent", PlayTrueAnswer);
            EventManager.Instance.AddListeners("FalseAnswerEvent", PlayFalseAnswer);
            EventManager.Instance.AddListeners("LastSecondsEvent", PlayTimeClip);
            EventManager.Instance.AddListeners("FinishedGameEvent", PlayStopClip);
            EventManager.Instance.AddListeners("StartGameEvent", PlayMainClip);
            EventManager.Instance.AddListeners("RestartGameEvent", PlayStopClip);
            #endregion

            //TrueAnswerSound = (AudioClip)Resources.Load(AudioFolder + "true");
            //FalseAnswerSound = (AudioClip)Resources.Load(AudioFolder + "false");
            //MainClip = (AudioClip)Resources.Load(AudioFolder + "main");
            //StopClip = (AudioClip)Resources.Load(AudioFolder + "stop");
            //TimeSound = (AudioClip)Resources.Load(AudioFolder + "finishtime");
        }

        /// <summary>
        /// Play true answer clip
        /// </summary>
        public void PlayTrueAnswer()
        {
            _AudioSource.PlayOneShot(TrueAnswerSound);
        }

        /// <summary>
        /// Play false answer clip
        /// </summary>
        public void PlayFalseAnswer()
        {
            _AudioSource.PlayOneShot(FalseAnswerSound);
        }
        

        /// <summary>
        /// Play main clip
        /// </summary>
        public void PlayMainClip()
        {
            _AudioSource.clip = MainClip;
            _AudioSource.Play();
        }

        /// <summary>
        /// Is playing main clip
        /// </summary>
        /// <returns>Boolean represent is playing main cleap or not</returns>
        public bool IsPlayingMainClip()
        {
            if (_AudioSource.clip == MainClip)
                return true;
            return false;
        }

        /// <summary>
        /// Play stop clip
        /// </summary>
        public void PlayStopClip()
        {
        if (_AudioSource.clip != null)
        {
            _AudioSource.clip = null;
            _AudioSource.PlayOneShot(StopClip);
        }
        }

        /// <summary>
        /// Disable all sounds
        /// </summary>
        internal void DisableSound()
        {
            _AudioSource.Stop();
        }

        /// <summary>
        /// Play time clip
        /// </summary>
        internal void PlayTimeClip()
        {
            _AudioSource.clip = TimeSound;
            _AudioSource.Play();
        }
        
    public bool ChangeMute()
    {
        _AudioSource.mute = !_AudioSource.mute;
        return _AudioSource.mute;
    }
        #endregion
    }

