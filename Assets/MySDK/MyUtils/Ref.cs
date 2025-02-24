using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyUtils
{
    public static class Ref
    {
        private static Dictionary<Type, MonoBehaviour> s_references = new Dictionary<Type, MonoBehaviour>();

#if UNITY_EDITOR

        // Sử dụng để reset giá trị static thủ công cho case load scene nhanh (giá trị static không bị reset)
        // => cần reset thủ công
        static Ref()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                Clear();
            }
        }

#endif

        public static void Add<T>(T obj) where T : MonoBehaviour
        {
            if (obj == null)
            {
                Common.LogWarning(obj, "You have tried to add a null reference");
                return;
            }

            var type = typeof(T);

            if(s_references.TryGetValue(type, out var reference))
            {
                Common.Warning(reference != null, obj, $"Reference for type {type} already exists");
                return;
            }

            s_references.Add(type, obj);
        }

        public static T Get<T>() where T : MonoBehaviour
        {
            var type = typeof(T);

            // Nếu tìm thấy Ref thì trả về
            if (s_references.TryGetValue(type, out var reference))
            {
                return reference as T;
            }

            // Nếu không tìm thấy Ref thì sẽ tìm nó trong scene
            T obj = Object.FindObjectOfType<T>();

            if (obj != null)
            {
                s_references.Add(type, obj);
            }
            else
            {
                // Kiểm tra xem nó đã bị destroy chưa -> nếu đã bị tức là app close -> không cần tạo mới
                if (s_references.ContainsKey(type))
                {
                    Common.LogWarning($"Reference type {type} destroyed in scene when app close so can't auto create new one");
                    return obj;
                }

                var gameObject = new GameObject($"Service - {type}");
                obj = gameObject.AddComponent<T>();
                Common.LogWarning($"Reference type {type} not found in scene, new one created");
            }

            return obj;
        }

        public static void Remove<T>(T obj) where T : MonoBehaviour
        {
            var type = typeof(T);

            if (!s_references.ContainsKey(type))
            {
                Common.LogWarning(obj, $"Reference for type {type} not found");
                return;
            }

            s_references.Remove(type);
        }

        public static void Clear()
        {
            s_references.Clear();
        }
    }
}
