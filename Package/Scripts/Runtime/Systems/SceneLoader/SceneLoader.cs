using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace D_Dev.SceneLoader
{
    public static class SceneLoader
    {
        #region Fields

        public static Dictionary<string, bool> Scenes;

        #endregion

        #region Public

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static UniTask InitializeScenesAsync()
        {
            Scenes = new();
            var path = "ProjectScenesConfig";
            var projectScenes = Resources.Load<ProjectScenesConfig>(path);
            if (projectScenes != null)
            {
                if(projectScenes.Scenes.Count <= 0)
                    return UniTask.CompletedTask;
                
                foreach (var scene in projectScenes.Scenes)
                {
                    Scenes.Add(scene.SceneName, scene.IsUnloadable);
                    if (scene.AddSceneOnStartup)
                        LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive).Forget();
                }
            }
            else
                Debug.LogError($"[SceneLoader] Failed to load project scenes config at path: Resources/{path}");
            return UniTask.CompletedTask;
        }
        
        public static async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single,
            Action OnStart = null, Action OnComplete = null, CancellationToken cancellationToken = default)
        {
            OnStart?.Invoke();
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            if (operation == null)
            {
                Debug.LogError($"[SceneLoader] Failed to start loading scene: {sceneName}");
                return;
            }

            operation.allowSceneActivation = false;
            await UniTask.WaitUntil(() => operation.progress >= 0.9f, cancellationToken: cancellationToken);
            operation.allowSceneActivation = true;
            await operation;
            Debug.Log($"[SceneLoader] Scene '{sceneName}' loaded successfully");
            OnComplete?.Invoke();
        }

        public static async UniTask UnloadUnloadableScenesExcept(string exceptScene, Action OnStart = null, Action OnComplete = null)
        {
            var unloadableScenes = new List<string>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (Scenes.TryGetValue(scene.name, out var isUnloadable) && isUnloadable && scene.name != exceptScene)
                    unloadableScenes.Add(scene.name);
            }

            OnStart?.Invoke();
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
            OnComplete?.Invoke();
        }

        #endregion
    }
}
