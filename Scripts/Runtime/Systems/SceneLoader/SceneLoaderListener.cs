using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using D_Dev.CustomEventManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace D_Dev.SceneLoader
{
    public class SceneLoaderListener : MonoBehaviour
    {
        #region Fields

        [SerializeField] private UnityEvent _onSceneLoadStart;
        [SerializeField] private UnityEvent _onSceneLoadEnd;
        
        private CancellationTokenSource _tokenSource;

        #endregion

        #region Lifecycle

        private void Awake()
        {
            _tokenSource = new CancellationTokenSource();

            EventManager.AddListener<string>(EventNameConstants.SceneLoad.ToString(), OnSceneLoad);
            EventManager.AddListener(EventNameConstants.SceneReload.ToString(), OnSceneReload);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<string>(EventNameConstants.SceneLoad.ToString(), OnSceneLoad);
            EventManager.RemoveListener(EventNameConstants.SceneReload.ToString(), OnSceneReload);
            _tokenSource?.Cancel();
        }

        #endregion

        #region Private

        private void SetActiveScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid() && scene.isLoaded)
            {
                SceneManager.SetActiveScene(scene);
                Debug.Log($"[SceneLoader] Set active scene to '{sceneName}'");
                _onSceneLoadEnd?.Invoke();
            }
        }

        private string GetLastActiveUnloadableScene()
        {
            List<string> unloadableScenes = new List<string>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (SceneLoader.Scenes.TryGetValue(scene.name, out var isUnloadable) && isUnloadable)
                    unloadableScenes.Add(scene.name);
            }
            return unloadableScenes[^1];
        }

        #endregion

        #region Listeners

        private void OnSceneLoad(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Scene name is null or empty");
                return;
            }

            if (SceneLoader.Scenes.ContainsKey(sceneName))
            {
                if (SceneLoader.Scenes[sceneName])
                {
                    SceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive,
                        () => _onSceneLoadStart?.Invoke(),
                        () => SetActiveScene(sceneName),
                        _tokenSource.Token).Forget();
                    SceneLoader.UnloadUnloadableScenesExcept(sceneName).Forget();
                }
                else
                {
                    SceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive, cancellationToken: _tokenSource.Token).Forget();
                }
            }
            else
            {
                Debug.LogError($"[SceneLoader] Scene '{sceneName}' not found in config.");
            }
        }

        private void OnSceneReload()
        {
            var currentScene = GetLastActiveUnloadableScene();
            if (SceneLoader.Scenes.TryGetValue(currentScene, out var isUnloadable) && isUnloadable)
            {
                SceneLoader.UnloadUnloadableScenesExcept(string.Empty,
                    OnStart: () => _onSceneLoadStart?.Invoke()).Forget();
                SceneLoader.LoadSceneAsync(currentScene, LoadSceneMode.Additive,
                    OnComplete: () => SetActiveScene(currentScene),
                    cancellationToken: _tokenSource.Token).Forget();
            }
            else
            {
                SceneLoader.LoadSceneAsync(currentScene, LoadSceneMode.Additive, cancellationToken: _tokenSource.Token).Forget();
            }
        }
      
        #endregion
    }
}
