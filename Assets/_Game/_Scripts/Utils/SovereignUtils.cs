using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
