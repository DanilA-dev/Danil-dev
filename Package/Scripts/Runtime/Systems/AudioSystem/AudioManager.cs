using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D_Dev.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.AudioSystem
{
    public class AudioManager : BaseSingleton<AudioManager>
    {
        #region Classes

        private struct SoundRequest
        {
            public AudioClip Clip;
            public AudioConfig Config;
            public float Priority;
            public Vector3 WorldPos;
        }

        #endregion

        #region Fields

        [Title("Audio Mixer Groups")]
        [SerializeField] private AudioMixerGroupVolumeConfig[] _mixerGroupVolumeConfigs;

        [Title("Optimizations")]
        [SerializeField] private int maxSimultaneousSFX = 10;
        [SerializeField] private int poolSize = 14;
        [SerializeField] private float cullDistance = 30f;
        [SerializeField] private float throttleTime = 0.05f;

        private List<SoundRequest> _requests = new(64);
        private Dictionary<AudioClip, float> _lastPlayed = new();

        private Dictionary<AudioConfig, AudioSource> _activeSources = new();

        private AudioSource[] _pool;
        private Camera _mainCamera;
        private int _poolIdx;

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();
            InitPool();
        }

        private void Start() => _mainCamera = Camera.main;

        private void LateUpdate()
        {
            if (_requests.Count == 0)
                return;

            _requests.Sort((a, b) => b.Priority.CompareTo(a.Priority));

            float now = Time.time;
            int played = 0;

            for (int i = 0; i < _requests.Count && played < maxSimultaneousSFX; i++)
            {
                var req = _requests[i];

                if (_lastPlayed.TryGetValue(req.Clip, out float last) && now - last < throttleTime)
                    continue;

                _lastPlayed[req.Clip] = now;
                PlayFromPool(req.Config, req.WorldPos);
                played++;
            }

            _requests.Clear();
        }

        #endregion

        #region Public

        public void SetVolume(MixerGroupType type, float value)
        {
            var config = _mixerGroupVolumeConfigs.FirstOrDefault(x => x.Type == type);
            config?.SetVolume(value);
        }

        public AudioMixerGroupVolumeConfig GetMixerGroup(MixerGroupType type)
        {
            return _mixerGroupVolumeConfigs.FirstOrDefault(x => x.Type == type);
        }

        public void SetMusicVolume(float value)
        {
            var config = _mixerGroupVolumeConfigs.FirstOrDefault(x => x.Type == MixerGroupType.Music);
            config?.SetVolume(value);
        }

        public void SetSFXVolume(float value)
        {
            var config = _mixerGroupVolumeConfigs.FirstOrDefault(x => x.Type == MixerGroupType.SFX);
            config?.SetVolume(value);
        }

        public void SetAudioListenerVolume(float volume01) => AudioListener.volume = volume01;
        public float GetAudioListenerVolume() => AudioListener.volume;

        public void RequestSound(AudioConfig config, Vector3 worldPos)
        {
            if (config == null)
                return;

            AudioClip clip = config.GetClip();
            if (clip == null)
                return;

            float priority = config.Priority;

            if (_mainCamera != null && config.SpatialBlend > 0f)
            {
                float dist = Vector3.Distance(worldPos, _mainCamera.transform.position);
                if (dist > cullDistance) return;
                priority /= 1f + dist * 0.1f;
            }

            _requests.Add(new SoundRequest { Clip = clip, Config = config, Priority = priority, WorldPos = worldPos });
        }

        public void StopSound(AudioConfig config)
        {
            if (_activeSources.TryGetValue(config, out var src))
            {
                src.Stop();
                _activeSources.Remove(config);
            }
        }

        public void StopSoundWithFade(AudioConfig config)
        {
            if (_activeSources.TryGetValue(config, out var src))
                StartCoroutine(FadeStop(src, config));
        }

        #endregion

        #region Private

        private void PlayFromPool(AudioConfig config, Vector3 worldPos)
        {
            AudioSource src = null;
            for (int i = 0; i < _pool.Length; i++)
            {
                int j = (_poolIdx + i) % _pool.Length;
                if (!_pool[j].isPlaying)
                {
                    src = _pool[j];
                    break;
                }
            }

            src ??= _pool[_poolIdx % _pool.Length];
            _poolIdx = (_poolIdx + 1) % _pool.Length;

            config.SetAudioSource(ref src);
            src.transform.position = worldPos; 

            _activeSources[config] = src;

            src.Play();
        }

        private void InitPool()
        {
            _pool = new AudioSource[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                var go = new GameObject($"SFX_Pool_{i}");
                go.transform.SetParent(transform);
                var src = go.AddComponent<AudioSource>();
                src.priority = 128;
                src.playOnAwake = false;
                _pool[i] = src;
            }
        }

        private IEnumerator FadeStop(AudioSource src, AudioConfig config)
        {
            float startVolume = src.volume;

            for (float i = 0; i < config.FadeTime; i += Time.deltaTime)
            {
                src.volume = startVolume * (1 - i / config.FadeTime);
                yield return null;
            }

            src.Stop();
            src.volume = startVolume;
            _activeSources.Remove(config);
        }

        #endregion
    }
}