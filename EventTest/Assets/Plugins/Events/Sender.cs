#if !SIMPLE_EVENT_DEBUG
#define SIMPLE_EVENT_DEBUG
#endif
#if SIMPLE_EVENT_DEBUG && UNITY_EDITOR
using KingDOM.Event.Debug;
#endif
using UnityEngine;
using System;
using System.Collections.Generic;

namespace KingDOM.Event
{
    /// <summary>
    /// The class is responsible for registering subscribers to the event, the removal of subscribers on-demand and event generation. It is a basic element in the system event notifications.
    /// </summary>
    public class Sender
    {

        private static Dictionary<string, SortedDictionary<int, List<Action<SimpleEvent>>>> ht;

        static Sender()
        {
            ht = new Dictionary<string, SortedDictionary<int, List<Action<SimpleEvent>>>>();
        }

#if SIMPLE_EVENT_DEBUG && UNITY_EDITOR
        public static event EventHandler<EventCheckArgs> startEvent;
        public static event EventHandler<EventCheckArgs> endEvent;
        public static event EventHandler<EventCheckArgs> stopEvent;
        public static event EventHandler<EventCheckArgs> errorEvent;

        public static EventsLog Log;

        public static void StartLog()
        {
            Log = new EventsLog();
            Log.Init();
        }

        public static void StopLog()
        {
            if (Log != null)
                Log.Destroy();
        }

        private static void OnStartEvent(SimpleEvent simpleEvent, object receiver, int priority = 0)
        {
            OnEvent(startEvent, EventsCheckStage.Before, simpleEvent, receiver, priority);
        }

        private static void OnEndEvent(SimpleEvent simpleEvent, object receiver, int priority = 0)
        {
            OnEvent(endEvent, EventsCheckStage.After, simpleEvent, receiver, priority);
        }

        private static void OnStopEvent(SimpleEvent simpleEvent, object receiver, int priority = 0)
        {
            OnEvent(endEvent, EventsCheckStage.After, simpleEvent, receiver, priority);
        }

        private static void OnErrorEvent(SimpleEvent simpleEvent, object receiver, int priority = 0)
        {
            OnEvent(endEvent, EventsCheckStage.After, simpleEvent, receiver, priority);
        }

        private static void OnEvent(EventHandler<EventCheckArgs> handler, EventsCheckStage stage, SimpleEvent simpleEvent, object receiver, int priority)
        {
            if (handler != null)
            {
                EventCheckArgs args = new EventCheckArgs();
                args.Stage = stage;
                args.Event = simpleEvent;
                args.Receiver = receiver;
                args.Priority = priority;

                handler(null, args);
            }
        }
#endif


        /// <summary>
        /// Subscribe the event.
        /// </summary>
        /// <param name="nameEvent">Event name is also a unique key by which the event links with handlers subscribing.</param>
        /// <param name="priority">Event priority. The higher the value, the earlier subscribed method in the queue handlers will be launched. Handlers with the same priority will be executed in any order.</param>
        /// <param name="function">Handler method  receives a reference to the event at the time of its occurrence.</param>
        /// <returns>True if subscribtion to the event is successful.</returns>
        public static bool AddEvent(string nameEvent, int priority, Action<SimpleEvent> function)
        {
            SortedDictionary<int, List<Action<SimpleEvent>>> priorityAction;
            if (!ht.ContainsKey(nameEvent))
            {
                priorityAction = new SortedDictionary<int, List<Action<SimpleEvent>>>(new ReverseComparer<int>());
                ht.Add(nameEvent, priorityAction);
            }
            else {
                priorityAction = ht[nameEvent];
                if (priorityAction == null)
                    return false;
            }

            List<Action<SimpleEvent>> listAction;
            if (!priorityAction.ContainsKey(priority))
            {
                listAction = new List<Action<SimpleEvent>>();
                priorityAction.Add(priority, listAction);
            }
            else {
                listAction = priorityAction[priority];
                if (listAction == null)
                    return false;
            }
            listAction.Add(function);

            return true;
        }
        /// <summary>
        /// Subscribe the event with priority (default value 0). 
        /// </summary>
        /// <param name="nameEvent">Event name is also a unique key by which the event links with handlers subscribing.</param>
        /// <param name="function">Handler method  receives a reference to the event at the time of its occurrence.</param>
        /// <returns>True if subscribtion to the event is successful.</returns>
        public static bool AddEvent(string nameEvent, Action<SimpleEvent> function)
        {
            return AddEvent(nameEvent, 0, function);
        }
        /// <summary>
        /// Remove handler from event subscribtion.
        /// </summary>
        /// <param name="nameEvent">Event name is also a unique key by which the event links with handlers subscribing.</param>
        /// <param name="priority">Event priority. It must correspond to the value specified at the time of subscribing to an event.</param>
        /// <param name="function">Handler method</param>
        /// <returns>True if subscribtion to the event is removed successfully.</returns>
        public static bool RemoveEvent(string nameEvent, int priority, Action<SimpleEvent> function)
        {
            if (!ht.ContainsKey(nameEvent))
                return false;

            SortedDictionary<int, List<Action<SimpleEvent>>> priorityAction;
            priorityAction = ht[nameEvent];

            if (priorityAction == null || !priorityAction.ContainsKey(priority))
                return false;

            List<Action<SimpleEvent>> listAction;
            listAction = priorityAction[priority];

            if (listAction == null)
                return false;

            return listAction.Remove(function);
        }
        /// <summary>
        /// Remove handler from event subscribtion with priority 0.
        /// </summary>
        /// <param name="nameEvent">Event name is also a unique key by which the event links with handlers subscribing.</param>
        /// <param name="function">Handler method</param>
        /// <returns>True if subscribtion to the event is removed successfully.</returns>
        public static bool RemoveEvent(string nameEvent, Action<SimpleEvent> function)
        {
            return RemoveEvent(nameEvent, 0, function);
        }
        /// <summary>
        /// Send event to all subscribers.
        /// </summary>
        /// <param name="nameEvent">Event name is also a unique key by which the event links with handlers subscribing.</param>
        /// <param name="target">Event source</param>
        /// <param name="args">The list of parameters passed in the event.</param>
        /// <returns>True if all subscribers to the event have worked without error.</returns>
        public static bool SendEvent(string nameEvent, object target, params object[] args)
        {
            bool ret = true;
            if (!ht.ContainsKey(nameEvent))
                return false;

            SortedDictionary<int, List<Action<SimpleEvent>>> priorityAction;
            priorityAction = ht[nameEvent];
            if (priorityAction == null)
                return false;

            List<Action<SimpleEvent>> listAction;
            Action<SimpleEvent> function;
            SimpleEvent evnt = new SimpleEvent(nameEvent, target, args);
            foreach (KeyValuePair<int, List<Action<SimpleEvent>>> pair in priorityAction)
            {
                listAction = new List<Action<SimpleEvent>>(pair.Value);
                foreach (Action<SimpleEvent> action in listAction)
                {
                    function = action;

                    if (function != null)
                    {
#if SIMPLE_EVENT_DEBUG && UNITY_EDITOR
                        OnStartEvent(evnt, function.Target, pair.Key);
#endif
                        try
                        {
                            function(evnt);
                            if (evnt.stop)
                            {
#if SIMPLE_EVENT_DEBUG && UNITY_EDITOR
                                OnStopEvent(evnt, function.Target, pair.Key);
#endif
                                return ret;
                            }
#if SIMPLE_EVENT_DEBUG && UNITY_EDITOR
                            OnEndEvent(evnt, function.Target, pair.Key);
#endif
                        }
                        catch
                        {
                            ret = false;
#if SIMPLE_EVENT_DEBUG && UNITY_EDITOR
                            OnErrorEvent(evnt, function.Target, pair.Key);
#endif
                        }
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// Send event within the hierarchy of objects displayed on the scene. 
        /// handlers calling is done in three stages::
        /// 1 Tunneling  (from the parent to the source of the event).
        /// 2 Target (event processing at its source).
        /// 3 Bubbling (from the source to its parent).
        /// For this event the calling procedure is directly related to the position in the hierarchy of displaying, and to the event priority. Handler priority > 0 indicates the execution in the first stage, all other handlers are performed on stages 2 and 3 In accordance with the position in the hierarchy displaying. Inside one object the order of callings is due to a higher priority.
        /// Scene objects that are not their parents for the event source do not receive an event notification.
        /// </summary>
        /// <param name="nameEvent">Event name is also a unique key by which the event links with handlers subscribing.</param>
        /// <param name="target">Event source (can be GameObject or Component)</param>
        /// <param name="args">The list of parameters passed in the event.</param>
        /// <returns>True if all subscribers to the event have worked without error.</returns>
        public static bool SendEventHierarchy(string nameEvent, object target, params object[] args)
        {
            bool ret = true;
            if (!ht.ContainsKey(nameEvent))
                return false;

            SortedDictionary<int, List<Action<SimpleEvent>>> priorityAction;
            priorityAction = ht[nameEvent];
            if (priorityAction == null)
                return false;

            List<Action<SimpleEvent>> listAction;
            Action<SimpleEvent> function;

            //We create a list of objects caused by the hierarchy. First, we shall send  the event down from the parent to the source (event priority> 0), then we shall send it up (priority <= 0)
            Dictionary<object, int> level = new Dictionary<object, int>();
            // calling list for each level of the hierarchy already defined by the priority
            SortedDictionary<int, List<Action<SimpleEvent>>> hierarchyAction = new SortedDictionary<int, List<Action<SimpleEvent>>>(new ReverseComparer<int>());
            // calling list
            List<Action<SimpleEvent>> listHierarchyAction;

            int i = 0;
            GameObject targetEvent = getGameObject(target);
            if (targetEvent == null)
                return false;
            do
            {
                level.Add(targetEvent, i);
                i++;
                if (targetEvent.transform && targetEvent.transform.parent)
                {
                    targetEvent = targetEvent.transform.parent.gameObject;
                }
                else {
                    targetEvent = null;
                }
            } while (targetEvent != null);

            foreach (KeyValuePair<int, List<Action<SimpleEvent>>> pair in priorityAction)
            {
                listAction = new List<Action<SimpleEvent>>(pair.Value);
                foreach (Action<SimpleEvent> action in listAction)
                {
                    function = action;

                    if (function != null)
                    {
                        GameObject targetAction = getGameObject(function.Target);
                        if (level.ContainsKey(targetAction))
                        {
                            i = pair.Key > 0 ? level[targetAction] : -level[targetAction];
                            if (hierarchyAction.ContainsKey(i))
                                listHierarchyAction = hierarchyAction[i];
                            else {
                                listHierarchyAction = new List<Action<SimpleEvent>>();
                                hierarchyAction.Add(i, listHierarchyAction);
                            }

                            listHierarchyAction.Add(action);

                        }
                    }
                }
            }

            SimpleEvent evnt = new SimpleEvent(nameEvent, target, args);

            foreach (KeyValuePair<int, List<Action<SimpleEvent>>> pairHierarchy in hierarchyAction)
            {
                foreach (Action<SimpleEvent> action in pairHierarchy.Value)
                {
                    function = action;

                    if (function != null)
                    {
                        try
                        {
                            function(evnt);
                            if (evnt.stop)
                                return ret;
                        }
                        catch
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }

        private static GameObject getGameObject(object target)
        {
            if (target == null)
                return null;

            if (target is GameObject)
                return (GameObject)target;

            if (target is Component)
                return ((Component)target).gameObject;

            return null;
        }

    }
}