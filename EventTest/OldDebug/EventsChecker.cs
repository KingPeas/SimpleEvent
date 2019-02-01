#if !SIMPLE_EVENT_DEBUG
    #define SIMPLE_EVENT_DEBUG
#endif

using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEditor;
using UnityEngine;
using System.Collections;

namespace KingDOM.Event.Debug
{
    public static class EventsChecker
    {
//#if UNITY_EDITOR
        private static EventsExplorer explorer = null;
//#endif        
        public static Dictionary<string, SortedDictionary<int, List<Action<SimpleEvent>>>> SubscribeList;

//#if UNITY_EDITOR
        public static void SetExplorer(EventsExplorer window)
        {
            explorer = window;
        }
//#endif
        //Проверка события что оно соотвествует условиям.
        public static void CheckEvent(EventsCheckStage stage, SimpleEvent evnt, UnityEngine.Object receiver)
        {
//#if UNITY_EDITOR
            explorer = EditorWindow.GetWindow<EventsExplorer>();
            if (explorer == null)
                return;
            bool result = explorer.EventToPause;
            if (result)
            {
                bool check = explorer.Check(stage, evnt, receiver);
                if (check)
                    EditorApplication.isPaused = true;
//#endif
            }
        }
    }
    
}


