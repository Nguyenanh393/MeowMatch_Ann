// using System;
// using System.Collections.Generic;
// using Observer.Runtime;
// using Sirenix.OdinInspector;
// using Sirenix.OdinInspector.Editor;
// using UnityEditor;
// using UnityEngine;
//
// // ReSharper disable All
// namespace Observer.Editor
// {
//     public class MessengerTrackerWindow : OdinEditorWindow
//     {
//         private class EventInfo
//         {
//             public EventKey eventKey;
//             public int count;
//
//             [TableList(AlwaysExpanded = true, HideToolbar = true, ShowIndexLabels = true)]
//             public DelegateInfo[] listeners;
//         }
//
//         private class LogInfo
//         {
//             [TableColumnWidth(100, Resizable = false)]
//             public LogType logType;
//
//             [TableColumnWidth(100)] public EventKey eventKey;
//
//             [TableColumnWidth(60)] public string method;
//
//             [TableColumnWidth(100)] public object target;
//
//             public LogInfo(LogType logType, EventKey eventKey, DelegateInfo listener)
//             {
//                 this.logType = logType;
//                 this.eventKey = eventKey;
//                 method = listener != null ? listener.method : string.Empty;
//                 target = listener != null ? listener.target : string.Empty;
//             }
//         }
//
//         private class DelegateInfo
//         {
//             [TableColumnWidth(60)] public string method;
//
//             [TableColumnWidth(100)] public object target;
//
//             public DelegateInfo(Delegate del)
//             {
//                 method = del.Method.Name;
//                 target = del.Target;
//             }
//         }
//
//         [Flags]
//         public enum LogType
//         {
//             NONE = 0,
//             BROARDCAST = 1 << 0,
//             ADDING = 1 << 1,
//             REMOVING = 1 << 2,
//         }
//
//         [SerializeField, TabGroup("Events"), OnValueChanged("RefreshEvents")]
//         private EventKey _eventFilter;
//
//         [SerializeField, TabGroup("Logs"), OnValueChanged("RefreshLogs")]
//         private LogType _logTypeFilter;
//
//         // Xem danh sách các event đã đăng kí tại bất kì thời điểm nào
//         [SerializeField, TabGroup("Events"), PropertySpace(10), ReadOnly]
//         private List<EventInfo> _events = new List<EventInfo>();
//
//         // Xem được log tính từ thời điểm mở cửa sổ
//         [SerializeField, TabGroup("Logs"), PropertySpace(10), ReadOnly,
//          TableList(AlwaysExpanded = true, HideToolbar = true, ShowIndexLabels = true)]
//         private List<LogInfo> _filterLogs = new List<LogInfo>();
//
//         private List<LogInfo> _logs = new List<LogInfo>();
//
//         [MenuItem("Window/Observer Tracker")]
//         public static void ShowWindow()
//         {
//             GetWindow<MessengerTrackerWindow>("Observer Tracker");
//         }
//
//         protected override void OnEnable()
//         {
//             base.OnEnable();
//
//             RefreshEvents();
//             Messenger.OnEventTableChanged += RefreshEvents;
//             Messenger.OnNewAction += AddNewLog;
//         }
//
//         protected override void OnDestroy()
//         {
//             base.OnDestroy();
//
//             ClearLogs();
//             Messenger.OnEventTableChanged -= RefreshEvents;
//             Messenger.OnNewAction -= AddNewLog;
//         }
//
//         private void AddNewLog(int logType, EventKey eventKey, Delegate listener)
//         {
//             _logs.Add(new LogInfo((LogType)logType, eventKey, new DelegateInfo(listener)));
//
//             RefreshLogs();
//         }
//
//         private void RefreshLogs()
//         {
//             _filterLogs.Clear();
//
//             foreach (var log in _logs)
//             {
//                 bool hideLog = (_logTypeFilter != LogType.NONE) && ((_logTypeFilter & log.logType) == 0);
//
//                 if (hideLog)
//                 {
//                     continue;
//                 }
//
//                 _filterLogs.Add(log);
//             }
//         }
//
//         private void ClearLogs()
//         {
//             _filterLogs.Clear();
//         }
//
//         private void RefreshEvents()
//         {
//             _events.Clear();
//             LoadEvents(Messenger.EventTable);
//             LoadEvents(Messenger.PermanentTable);
//         }
//
//         private void LoadEvents(IReadOnlyDictionary<EventKey, Delegate> eventTable)
//         {
//             foreach (var kvp in eventTable)
//             {
//                 bool hideEvent = _eventFilter != EventKey.NONE && kvp.Key != _eventFilter;
//
//                 if (hideEvent)
//                 {
//                     continue;
//                 }
//
//                 _events.Add(
//                     new EventInfo {
//                         eventKey = kvp.Key,
//                         count = kvp.Value?.GetInvocationList().Length ?? 0,
//                         listeners = kvp.Value?.GetInvocationList() != null
//                             ? Array.ConvertAll(kvp.Value.GetInvocationList(), x => new DelegateInfo(x))
//                             : Array.Empty<DelegateInfo>()
//                     }
//                 );
//             }
//         }
//     }
// }
