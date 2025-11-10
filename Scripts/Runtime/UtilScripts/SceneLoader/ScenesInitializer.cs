using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using D_Dev.CustomEventManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace D_Dev.SceneLoader
{
    public static class ScenesInitializer
    {
        #region Fields
        public static event Action<string> OnSceneFullyLoaded;
        private static CancellationTokenSource _tokenSource;
        private static Dictionary<string, bool> _scenes;

        #endregion

        #region DomainReload

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            EventManager.RemoveListener<string>(EventNameConstants.SceneLoad.ToString(), OnSceneLoad);
            EventManager.RemoveListener<string>(EventNameConstants.SceneAdd.ToString(), OnSceneAdd);
            EventManager.RemoveListener(EventNameConstants.SceneReload.ToString(), OnSceneReload);
            Application.quitting -= OnAppClose;
            
            EventManager.AddListener<string>(EventNameConstants.SceneLoad.ToString(), OnSceneLoad);
            EventManager.AddListener<string>(EventNameConstants.SceneAdd.ToString(), OnSceneAdd);
            EventManager.AddListener(EventNameConstants.SceneReload.ToString(), OnSceneReload);
            Application.quitting += OnAppClose;
 
            _tokenSource = new CancellationTokenSource();
            _scenes = new();

            InitializeScenesAsync().Forget();
        }

        #endregion

        #region Private

        private static async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single,
            Action OnComplete = null)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            if (operation == null)
            {
                Debug.LogError($"[SceneLoader] Failed to start loading scene: {sceneName}");
                return;
            }

            operation.allowSceneActivation = false;
            await UniTask.WaitUntil(() => operation.progress >= 0.9f, cancellationToken: _tokenSource.Token);
            operation.allowSceneActivation = true;
            await operation;
            Debug.Log($"[SceneLoader] Scene '{sceneName}' loaded successfully");
            OnSceneFullyLoaded?.Invoke(sceneName);
            OnComplete?.Invoke();
        }

        private static async UniTask UnloadUnloadableScenesExcept(string exceptScene)
        {
            var unloadableScenes = new List<string>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (_scenes.TryGetValue(scene.name, out var isUnloadable) && isUnloadable && scene.name!= exceptScene)
                    unloadableScenes.Add(scene.name);
            }

            foreach (var sceneName in unloadableScenes)
            {
                if (SceneManager.GetSceneByName(sceneName).isLoaded)
                {
                    var op = SceneManager.UnloadSceneAsync(sceneName);
                    if (op != null)
                    {
                        await op;
                        Debug.Log($"[SceneLoader] Unloaded scene '{sceneName}'");
                    }
                }
            }
        }

        private static void SetActiveScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid() && scene.isLoaded)
            {
                SceneManager.SetActiveScene(scene);
                Debug.Log($"[SceneLoader] Set active scene to '{sceneName}'");
            }
        }

        private static async UniTask InitializeScenesAsync()
        {
            var projectScenes = Resources.Load<ProjectUnloadableScenesConfig>("Project Unloadable Scenes Config");
            if (projectScenes != null)
            {
                foreach (var scene in projectScenes.Scenes)
                {
                    _scenes.Add(scene.SceneName, scene.IsUnloadable);
                    if(scene.AddSceneOnStartup)
                        await LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive);
                }
            }
        }

        #endregion

        #region Listeners

        private static void OnSceneLoad(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Scene name is null or empty");
                return;
            }

            if (_scenes.ContainsKey(sceneName))
            {
                LoadSceneAsync(sceneName, LoadSceneMode.Additive,
                    () => SetActiveScene(sceneName)).Forget();
                UnloadUnloadableScenesExcept(sceneName).Forget();
            }
            else if (_scenes.ContainsKey(sceneName))
            {
                LoadSceneAsync(sceneName, LoadSceneMode.Additive).Forget();
            }
            else
            {
                Debug.LogError($"[SceneLoader] Scene '{sceneName}' not found in config.");
            }
        }

        private static void OnSceneAdd(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Scene name is null or empty");
                return;
            }
            LoadSceneAsync(sceneName, LoadSceneMode.Additive).Forget();
        }

        private static async void OnSceneReload()
        {
            var currentScene = GetLastActiveUnloadableScene();
            if (_scenes.TryGetValue(currentScene, out var isUnloadable) && isUnloadable)
            {
                UnloadUnloadableScenesExcept(string.Empty).Forget();
                await LoadSceneAsync(currentScene, LoadSceneMode.Additive,
                    () => SetActiveScene(currentScene));
            }
            else
            {
                LoadSceneAsync(currentScene, LoadSceneMode.Additive).Forget();
            }
        }


        private static string GetLastActiveUnloadableScene()
        {
            List<string> unloadableScenes = new List<string>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (_scenes.TryGetValue(scene.name, out var isUnloadable) && isUnloadable)
                    unloadableScenes.Add(scene.name);
            }
            return unloadableScenes[^1];
        }
        private static void OnAppClose()
        {
            _tokenSource?.Cancel();
        }
        
        #endregion
    }
}
