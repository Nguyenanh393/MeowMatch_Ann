using System;
using System.Collections.Generic;
using System.Diagnostics;
using MyUtils;
using UnityEditor;

namespace Observer.Runtime
{
    // ReSharper disable All
    public static class Messenger
    {
        // Lưu trữ các event thông thường
        private static Dictionary<EventKey, Delegate> s_eventTable = new Dictionary<EventKey, Delegate>();

        // Lưu trữ các event tồn tại tới hết scope
        private static Dictionary<EventKey, Delegate> s_permanentTable = new Dictionary<EventKey, Delegate>();

        public static IReadOnlyDictionary<EventKey, Delegate> EventTable => s_eventTable;
        public static IReadOnlyDictionary<EventKey, Delegate> PermanentTable => s_permanentTable;

#if UNITY_EDITOR
        private static event Action s_onEventTableChanged;

        // 0 : None
        // 1 << 0 : Broadcast
        // 1 << 1 : Adding
        // 1 << 2 : Removing
        private static event Action<int, EventKey, Delegate> s_onNewAction;

        public static event Action OnEventTableChanged
        {
            add => s_onEventTableChanged += value;
            remove => s_onEventTableChanged -= value;
        }

        public static event Action<int, EventKey, Delegate> OnNewAction
        {
            add => s_onNewAction += value;
            remove => s_onNewAction -= value;
        }

        // Sử dụng để reset giá trị static thủ công cho case load scene nhanh (giá trị static không bị reset)
        // => cần reset thủ công
        static Messenger()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                Cleanup(true);
            }
        }
#endif

        [Conditional("UNITY_EDITOR")]
        private static void NotifyEventTableChanged()
        {
#if UNITY_EDITOR
            s_onEventTableChanged?.Invoke();
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private static void NotifyNewAction(int logType, EventKey eventType, Delegate listener)
        {
#if UNITY_EDITOR
            s_onNewAction?.Invoke(logType, eventType, listener);
#endif
        }

        public static void Cleanup(bool includePermanent = false)
        {
            s_eventTable.Clear();

            if (includePermanent)
            {
                s_permanentTable.Clear();
            }

            NotifyEventTableChanged();
        }

        public static void OnListenerAdding(
            Dictionary<EventKey, Delegate> eventTable,
            EventKey eventType,
            Delegate listenerBeingAdded
        )
        {
            NotifyNewAction(1 << 1, eventType, listenerBeingAdded);

            eventTable.TryAdd(eventType, null);

            Delegate d = eventTable[eventType];

            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                throw new ListenerException(
                    $"Attempting to add listener with inconsistent signature for event type {eventType}. Current listeners have type {d.GetType().Name} and listener being added has type {listenerBeingAdded.GetType().Name}");
            }
        }

        public static void OnListenerRemoving(
            Dictionary<EventKey, Delegate> eventTable,
            EventKey eventType,
            Delegate listenerBeingRemoved
        )
        {
            NotifyNewAction(1 << 2, eventType, listenerBeingRemoved);

            if (eventTable.TryGetValue(eventType, out var d))
            {
                if (d == null)
                {
                    Common.LogWarning(
                        $"Attempting to remove listener with for event type \"{eventType}\" but current listener is null.");
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    Common.LogWarning(
                        $"Attempting to remove listener with inconsistent signature for event type {eventType}. Current listeners have type {d.GetType().Name} and listener being removed has type {listenerBeingRemoved.GetType().Name}");
                }
            }
            else
            {
                Common.LogWarning(
                    $"Attempting to remove listener for type \"{eventType}\" but Messenger doesn't know about this event type.");
            }
        }

        public static void OnListenerRemoved(Dictionary<EventKey, Delegate> eventTable, EventKey eventKey)
        {
            if (eventTable[eventKey] == null)
            {
                eventTable.Remove(eventKey);
            }

            NotifyEventTableChanged();
        }

        private static BroadcastException CreateBroadcastSignatureException(EventKey eventType)
        {
            return new BroadcastException(
                $"Broadcasting message \"{eventType}\" but listeners have a different signature than the broadcaster.");
        }

        private class BroadcastException : Exception
        {
            public BroadcastException(string msg) : base(msg) { }
        }

        private class ListenerException : Exception
        {
            public ListenerException(string msg) : base(msg) { }
        }


        // Các hàm add listener
        // No parameters
        public static void AddListener(EventKey eventType, Callback handler, bool permanent = false)
        {
            if (permanent)
            {
                OnListenerAdding(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback)s_permanentTable[eventType] + handler;
            }
            else
            {
                OnListenerAdding(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback)s_eventTable[eventType] + handler;
            }

            NotifyEventTableChanged();
        }

        // Single parameter
        public static void AddListener<T>(EventKey eventType, Callback<T> handler, bool permanent = false)
        {
            if (permanent)
            {
                OnListenerAdding(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T>)s_permanentTable[eventType] + handler;
            }
            else
            {
                OnListenerAdding(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T>)s_eventTable[eventType] + handler;
            }

            NotifyEventTableChanged();
        }

        // Two parameters
        public static void AddListener<T, U>(EventKey eventType, Callback<T, U> handler, bool permanent = false)
        {
            if (permanent)
            {
                OnListenerAdding(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T, U>)s_permanentTable[eventType] + handler;
            }
            else
            {
                OnListenerAdding(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T, U>)s_eventTable[eventType] + handler;
            }

            NotifyEventTableChanged();
        }

        // Three parameters
        public static void AddListener<T, U, V>(EventKey eventType, Callback<T, U, V> handler, bool permanent = false)
        {
            if (permanent)
            {
                OnListenerAdding(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T, U, V>)s_permanentTable[eventType] + handler;
            }
            else
            {
                OnListenerAdding(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T, U, V>)s_eventTable[eventType] + handler;
            }

            NotifyEventTableChanged();
        }

        // Four parameters
        public static void AddListener<T, U, V, N>(EventKey eventType, Callback<T, U, V, N> handler,
            bool permanent = false)
        {
            if (permanent)
            {
                OnListenerAdding(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T, U, V, N>)s_permanentTable[eventType] + handler;
            }
            else
            {
                OnListenerAdding(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T, U, V, N>)s_eventTable[eventType] + handler;
            }

            NotifyEventTableChanged();
        }

        // Five parameters
        public static void AddListener<T, U, V, N, M>(EventKey eventType, Callback<T, U, V, N, M> handler,
            bool permanent = false)
        {
            if (permanent)
            {
                OnListenerAdding(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T, U, V, N, M>)s_permanentTable[eventType] + handler;
            }
            else
            {
                OnListenerAdding(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T, U, V, N, M>)s_eventTable[eventType] + handler;
            }

            NotifyEventTableChanged();
        }

        // Các hàm remove listener
        // No parameters
        public static void RemoveListener(EventKey eventType, Callback handler, bool permanent = false)
        {
            if (permanent)
            {
                OnListenerRemoving(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback)s_permanentTable[eventType] - handler;
                OnListenerRemoved(s_permanentTable, eventType);
            }
            else
            {
                OnListenerRemoving(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback)s_eventTable[eventType] - handler;
                OnListenerRemoved(s_eventTable, eventType);
            }
        }

        // Single parameter
        public static void RemoveListener<T>(EventKey eventType, Callback<T> handler, bool permanent = false)
        {
            if (permanent)
            {
                OnListenerRemoving(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T>)s_permanentTable[eventType] - handler;
                OnListenerRemoved(s_permanentTable, eventType);
            }
            else
            {
                OnListenerRemoving(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T>)s_eventTable[eventType] - handler;
                OnListenerRemoved(s_eventTable, eventType);
            }
        }

        //Two parameters
        public static void RemoveListener<T, U>(EventKey eventType, Callback<T, U> handler, bool permanent = false)
        {
            if (permanent)
            {
                OnListenerRemoving(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T, U>)s_permanentTable[eventType] - handler;
                OnListenerRemoved(s_permanentTable, eventType);
            }
            else
            {
                OnListenerRemoving(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T, U>)s_eventTable[eventType] - handler;
                OnListenerRemoved(s_eventTable, eventType);
            }
        }

        //Three parameters
        public static void RemoveListener<T, U, V>(EventKey eventType, Callback<T, U, V> handler,
            bool permanent = false)
        {
            if (permanent)
            {
                OnListenerRemoving(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T, U, V>)s_permanentTable[eventType] - handler;
                OnListenerRemoved(s_permanentTable, eventType);
            }
            else
            {
                OnListenerRemoving(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T, U, V>)s_eventTable[eventType] - handler;
                OnListenerRemoved(s_eventTable, eventType);
            }
        }

        //Four parameters
        public static void RemoveListener<T, U, V, N>(EventKey eventType, Callback<T, U, V, N> handler,
            bool permanent = false)
        {
            if (permanent)
            {
                OnListenerRemoving(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T, U, V, N>)s_permanentTable[eventType] - handler;
                OnListenerRemoved(s_permanentTable, eventType);
            }
            else
            {
                OnListenerRemoving(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T, U, V, N>)s_eventTable[eventType] - handler;
                OnListenerRemoved(s_eventTable, eventType);
            }
        }

        //Five parameters
        public static void RemoveListener<T, U, V, N, M>(
            EventKey eventType,
            Callback<T, U, V, N, M> handler,
            bool permanent = false)
        {
            if (permanent)
            {
                OnListenerRemoving(s_permanentTable, eventType, handler);
                s_permanentTable[eventType] = (Callback<T, U, V, N, M>)s_permanentTable[eventType] - handler;
                OnListenerRemoved(s_permanentTable, eventType);
            }
            else
            {
                OnListenerRemoving(s_eventTable, eventType, handler);
                s_eventTable[eventType] = (Callback<T, U, V, N, M>)s_eventTable[eventType] - handler;
                OnListenerRemoved(s_eventTable, eventType);
            }
        }

        // Các hàm invoke callback
        private static void InvokeCallback(Dictionary<EventKey, Delegate> eventTable, EventKey eventType)
        {
            if (eventTable.TryGetValue(eventType, out var d))
            {
                if (d is Callback callback)
                {
                    callback();
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        private static void InvokeCallback<T>(
            Dictionary<EventKey, Delegate> eventTable,
            EventKey eventType,
            T arg1)
        {
            if (eventTable.TryGetValue(eventType, out var d))
            {
                if (d is Callback<T> callback)
                {
                    callback(arg1);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        private static void InvokeCallback<T, U>(
            Dictionary<EventKey, Delegate> eventTable,
            EventKey eventType,
            T arg1, U arg2)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d))
            {
                if (d is Callback<T, U> callback)
                {
                    callback(arg1, arg2);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        private static void InvokeCallback<T, U, V>(
            Dictionary<EventKey, Delegate> eventTable,
            EventKey eventType,
            T arg1, U arg2, V arg3)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d))
            {
                if (d is Callback<T, U, V> callback)
                {
                    callback(arg1, arg2, arg3);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        private static void InvokeCallback<T, U, V, N>(
            Dictionary<EventKey, Delegate> eventTable,
            EventKey eventType,
            T arg1, U arg2, V arg3, N arg4)
        {
            if (eventTable.TryGetValue(eventType, out var d))
            {
                if (d is Callback<T, U, V, N> callback)
                {
                    callback(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        private static void InvokeCallback<T, U, V, N, M>(
            Dictionary<EventKey, Delegate> eventTable,
            EventKey eventType,
            T arg1, U arg2, V arg3, N arg4, M arg5)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d))
            {
                if (d is Callback<T, U, V, N, M> callback)
                {
                    callback(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        // Các hàm broadcast
        // No parameters
        public static void Broadcast(EventKey eventType)
        {
            InvokeCallback(s_eventTable, eventType);
            InvokeCallback(s_permanentTable, eventType);
            NotifyNewAction(1 << 0, eventType, null);
        }

        // Single parameter
        public static void Broadcast<T>(EventKey eventType, T arg1)
        {
            InvokeCallback(s_eventTable, eventType, arg1);
            InvokeCallback(s_permanentTable, eventType, arg1);
            NotifyNewAction(1 << 0, eventType, null);
        }

        // Two parameters
        public static void Broadcast<T, U>(EventKey eventType, T arg1, U arg2)
        {
            InvokeCallback(s_eventTable, eventType, arg1, arg2);
            InvokeCallback(s_permanentTable, eventType, arg1, arg2);
            NotifyNewAction(1 << 0, eventType, null);
        }

        // Three parameters
        public static void Broadcast<T, U, V>(EventKey eventType, T arg1, U arg2, V arg3)
        {
            InvokeCallback(s_eventTable, eventType, arg1, arg2, arg3);
            InvokeCallback(s_permanentTable, eventType, arg1, arg2, arg3);
            NotifyNewAction(1 << 0, eventType, null);
        }

        // Four parameters
        public static void Broadcast<T, U, V, N>(EventKey eventType, T arg1, U arg2, V arg3, N arg4)
        {
            InvokeCallback(s_eventTable, eventType, arg1, arg2, arg3, arg4);
            InvokeCallback(s_permanentTable, eventType, arg1, arg2, arg3, arg4);
            NotifyNewAction(1 << 0, eventType, null);
        }

        // Five parameters
        public static void Broadcast<T, U, V, N, M>(EventKey eventType, T arg1, U arg2, V arg3, N arg4, M arg5)
        {
            InvokeCallback(s_eventTable, eventType, arg1, arg2, arg3, arg4, arg5);
            InvokeCallback(s_permanentTable, eventType, arg1, arg2, arg3, arg4, arg5);
            NotifyNewAction(1 << 0, eventType, null);
        }
    }
}
