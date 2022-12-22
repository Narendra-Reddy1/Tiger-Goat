using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SovereignStudios
{
    public enum LogType
    {
        Log,
        Warning,
        Error,
        Assertion
    }
    public class SovereignUtils
    {

#if UNITY_EDITOR
        public static void Log(object message, LogType logType = LogType.Log)
        {
            switch (logType)
            {
                case LogType.Log:
                    Debug.Log($"{message}");
                    break;
                case LogType.Warning:
                    Debug.LogWarning($"{message}");
                    break;
                case LogType.Error:
                    Debug.LogError($"{message}");
                    break;
                case LogType.Assertion:
                    Debug.LogAssertion($"{message}");
                    break;
            }
        }
#endif
        public static string GetJsonStringForTheObject<T>(T data)
        {
            return JsonUtility.ToJson(data);
        }
        public static T GetObjectFromJsonString<T>(string key)
        {
            return JsonUtility.FromJson<T>(key);
        }
        public static Vector3 GetMousePositionInWorldCordinates(Vector2 mousePosition, Camera camera)
        {
            if (camera == null) camera = Camera.main;
            return camera.ScreenToWorldPoint(mousePosition);
        }
        public static void LoadSceneAsync(int scene, bool makeActive = false, System.Action onComplete = null)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).completed += (handle) =>
            {
                if (makeActive)
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(scene));
                onComplete?.Invoke();
            };
        }
        public static void LoadSceneAsync(string scene, bool makeActive = false, System.Action onComplete = null)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).completed += (handle) =>
            {
                if (makeActive)
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
                onComplete?.Invoke();
            };
        }
        public static void UnloadSceneAsync(string scene, System.Action onComplete = null)
        {
            SceneManager.UnloadSceneAsync(scene).completed += (handle) =>
            {
                onComplete?.Invoke();
            };
        }
        public static void UnloadSceneAsync(int scene, System.Action onComplete = null)
        {
            SceneManager.UnloadSceneAsync(scene).completed += (handle) =>
            {
                onComplete?.Invoke();
            };
        }
    }
}
