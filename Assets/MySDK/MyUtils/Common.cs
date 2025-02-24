#if UNITY_EDITOR
#define DEBUG
#endif
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MyUtils
{
    // Hỗ trợ log sử dụng được với hot reload, tự động loại bỏ trong bản build
    public static class Common
    {
        [Conditional("DEBUG")]
        public static void Log(object message, string color = "white")
        {
            Debug.Log($"<color={color}>{message}</color>");
        }

        [Conditional("DEBUG")]
        public static void Log(Object context, object message, string color = "white")
        {
            Debug.Log($"<color={color}>{message}</color>", context);
        }

        [Conditional("DEBUG")]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }

        [Conditional("DEBUG")]
        public static void LogWarning(Object context, object message)
        {
            Debug.LogWarning(message, context);
        }

        [Conditional("DEBUG")]
        public static void Warning(bool condition, object message)
        {
            if (condition) Debug.LogWarning(message);
        }

        [Conditional("DEBUG")]
        public static void Warning(bool condition, Object context, object message)
        {
            if (condition) Debug.LogWarning(message, context);
        }

        [Conditional("DEBUG")]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }

        [Conditional("DEBUG")]
        public static void LogError(Object context, object message)
        {
            Debug.LogError(message, context);
        }

        [Conditional("DEBUG")]
        public static void Error(bool condition, object message)
        {
            if (condition) Debug.LogError(message);
        }

        [Conditional("DEBUG")]
        public static void Error(bool condition, Object context, object message)
        {
            if (condition) Debug.LogError(message, context);
        }
    }
}
