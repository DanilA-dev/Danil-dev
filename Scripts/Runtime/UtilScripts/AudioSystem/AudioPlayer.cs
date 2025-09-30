using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.AudioSystem
{
    public class AudioPlayer : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<AudioConfig> _audioConfigs = new();

        private AudioSource _audioSource;
        private AudioConfig _lastAudioConfig;
            
        #endregion

        #region Monobehaviour

        private void Awake()
        {
            GetOrCreateAudioSource();
            TryStartAwakeAudio();
        }
      
        #endregion

        #region Public

        public void Play(StringScriptableVariable configName)
        {
            var configByName = _audioConfigs.FirstOrDefault(a => a.AudioConfigName == configName);
            if(configByName == null)
                return;
            
            Play(configByName);
        }
        
        public void PlayOneShot(StringScriptableVariable configName)
        {
            var configByName = _audioConfigs.FirstOrDefault(a => a.AudioConfigName == configName);
            if(configByName == null)
                return;
            
            PlayOneShot(configByName);
        }

        public void PlayOneShotWithDelay(StringScriptableVariable configName)
        {
            var configByName = _audioConfigs.FirstOrDefault(a => a.AudioConfigName == configName);
            if(configByName == null)
                return;
            
            PlayOneShotWithDelay(configByName);
        }
        
        public void PlayWithFade(StringScriptableVariable configName)
        {
            var configByName = _audioConfigs.FirstOrDefault(a => a.AudioConfigName == configName);
            if(configByName == null)
                return;
            
            PlayWithFade(configByName);
        }
        

        public void Play(int configIndex)
        {
            if(_audioConfigs[configIndex] == null)
                return;
            
            Play(_audioConfigs[configIndex]);
        }
        
        public void PlayOneShot(int configIndex)
        {
            if(_audioConfigs[configIndex] == null)
                return;
            
            PlayOneShot(_audioConfigs[configIndex]);
        }

        public void PlayOneShotWithDelay(int configIndex)
        {
            if(_audioConfigs[configIndex] == null)
                return;
            
            PlayOneShotWithDelay(_audioConfigs[configIndex]);
        }
        
        public void PlayWithFade(int configIndex)
        {
            if(_audioConfigs[configIndex] == null)
                return;
            
            PlayWithFade(_audioConfigs[configIndex]);
        }
        
        public void Play(AudioConfig audioConfig)
        {
            if(audioConfig == null)
                return;
            
            audioConfig.SetAudioSource(ref _audioSource);
            _lastAudioConfig = audioConfig;
            switch (audioConfig.DelayType)
            {
                case DelayType.None:
                    _audioSource.Play();
                    break;
                case DelayType.SimpleDelay:
                    _audioSource.PlayDelayed(audioConfig.Delay);
                    break;
                case DelayType.ScheduledDelay:
                    _audioSource.PlayScheduled(audioConfig.ScheduledTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void PlayOneShot(AudioConfig audioConfig)
        {
            if(audioConfig == null)
                return;
            
            audioConfig.SetAudioSource(ref _audioSource);
            _lastAudioConfig = audioConfig;
            _audioSource.PlayOneShot(audioConfig.GetClip());
        }

        public void PlayOneShotWithDelay(AudioConfig audioConfig)
        {
            if(audioConfig == null)
                return;

            StartCoroutine(OneShotAudioDelayed(audioConfig));
        }

        public void PlayWithFade(AudioConfig audioConfig)
        {
            if(audioConfig == null)
                return;
            
            audioConfig.SetAudioSource(ref _audioSource);
            if (_audioSource.isPlaying && _lastAudioConfig != null)
                StartCoroutine(FadePlay(audioConfig));
            else
                Play(audioConfig);
        }
        #endregion
        
        #region Private

        private void TryStartAwakeAudio()
        {
           if(_audioConfigs.Count <= 0)
               return;

           var firstAwakeAudio = _audioConfigs.FirstOrDefault(a => a.PlayOnAwake);
           if(firstAwakeAudio == null)
               return;
           
           Play(firstAwakeAudio);
        }
        
        private void GetOrCreateAudioSource()
        {
            if(!TryGetComponent(out _audioSource))
                _audioSource = gameObject.AddComponent<AudioSource>();
        }

        #endregion

        #region Coroutines

        private IEnumerator FadePlay(AudioConfig audioConfig)
        {
            if (_audioSource.isPlaying)
            {
                for (float i = 0; i < _lastAudioConfig.FadeTime; i += Time.deltaTime)
                {
                    _audioSource.volume = (_lastAudioConfig.Volume - (i / _lastAudioConfig.FadeTime));
                    yield return null;
                }
            }
            _audioSource.Stop();
            audioConfig.SetAudioSource(ref _audioSource);
            _audioSource.Play();

            for (float i = 0; i < audioConfig.FadeTime; i += Time.deltaTime)
            {
                _audioSource.volume = ((i / audioConfig.FadeTime) * 1);
                yield return null;
            }
        }
        
        private IEnumerator OneShotAudioDelayed(AudioConfig audioConfig)
        {
            yield return new WaitForSeconds(audioConfig.Delay);
            PlayOneShot(audioConfig);
        }

        #endregion
    }
}
