using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using D_Dev.CustomEventManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace D_Dev.SceneLoader
{
    public static class SceneLoader
    {
        #region Fields

         private static CancellationTokenSource _tokenSource;

        #endregion

        #region DomainReload

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            EventManager.RemoveListener<string>(EventNameConstants.SceneLoad.ToString(), OnSceneLoad);
            EventManager.RemoveListener(EventNameConstants.SceneReload.ToString(), OnSceneReload);
            Application.quitting -= OnAppClose;
            
            EventManager.AddListener<string>(EventNameConstants.SceneLoad.ToString(), OnSceneLoad);
            EventManager.AddListener(EventNameConstants.SceneReload.ToString(), OnSceneReload);
            Application.quitting += OnAppClose;

            _tokenSource = new CancellationTokenSource();
        }

        #endregion

        #region Private

        private static async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
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
            LoadSceneAsync(sceneName).Forget();
        }

        private static void OnSceneReload()
        {
            var currentScene = SceneManager.GetActiveScene();
            LoadSceneAsync(currentScene.name).Forget();
        }

        private static void OnAppClose()
        {
            _tokenSource?.Cancel();
        }
        
        #endregion
    }
}
