using System;
using System.Collections.Generic;
using KingDOM.Event;
using KingDOM.Event.Debug;
using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEditorInternal;

namespace KingDOM.Event.Debug
{
    public class EventsExplorer : EditorWindow
    {

        // Регистрируем пункт меню и функцию, выполняющую открытие окна
        [MenuItem("Window/KingDOM/Events Explorer")]
        public static void CreateWindow()
        {
            Sender.StartLog();
            EventsExplorer window = GetWindow<EventsExplorer>();
            window.title = "Events Explorer Window";
            //EventsChecker.SetExplorer(window);
        }

        public bool EventToPause = false;
        public string EventName = "";
        //[SceneName]

        public EventsCheckStage Stage = EventsCheckStage.Before;
        public UnityEngine.Object Caller = null;
        public UnityEngine.Object Receiver = null;
        private Vector2 scrollPosition = Vector2.zero;
        private int selectIdx = 0;
        private ReorderableList listEvent;
        private ReorderableList listReciever;
        private EventLogLine lastEvent = null;
        //public EventsCheckSetup[] setups = new EventsCheckSetup[0];
        // Функция отрисовки окна
        public void OnGUI()
        {
            if (Sender.Log == null) Sender.StartLog();
            GUILayout.BeginVertical();
            SerializedObject so = new SerializedObject(this);

            EventToPause = EditorGUILayout.Toggle(new GUIContent("Event Pause"), EventToPause);
            EventName = EditorGUILayout.TextField("Event Name", EventName);
            Stage = (EventsCheckStage)Enum.ToObject(typeof(EventsCheckStage), EditorGUILayout.EnumPopup("Execute", Stage));
            Caller = EditorGUILayout.ObjectField(new GUIContent("Caller"), Caller, typeof(UnityEngine.Object));
            Receiver = EditorGUILayout.ObjectField(new GUIContent("Receiver"), Receiver, typeof(UnityEngine.Object));
            
                if (GUILayout.Button("Clear"))
            {
                Sender.StopLog();
                Sender.StartLog();
            }

            //GUILayout.Label(new GUIContent("Список событий", "Список всех обработанных событий."));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            listEvent.DoLayoutList();
            if (lastEvent != null && listReciever != null)
            {
                listReciever.DoLayoutList();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            so.ApplyModifiedProperties();
            Repaint();
        }

        void OnEnable()
        {
            if (Sender.Log == null) Sender.StartLog();
            listEvent = new ReorderableList(Sender.Log.events, typeof(EventLogLine), false, true, false, false);
            listEvent.drawElementCallback += drawEvent;
            listEvent.drawHeaderCallback += drawHeaderEvent;
            listEvent.onSelectCallback += selectEvent;
            clearListReceiver();
        }
        private void OnDisable()
        {
            // Make sure we don't get memory leaks etc.
            listEvent.drawHeaderCallback -= drawHeaderEvent;
            listEvent.drawElementCallback -= drawEvent;
            listEvent.onSelectCallback -= selectEvent;
            clearListReceiver();
        }

        void clearListReceiver()
        {
            if (listReciever == null) return;
            listReciever.drawHeaderCallback -= drawHeaderReceiver;
            listReciever.drawElementCallback -= drawReceiver;

        }

        void OnDestroy()
        {
            Sender.StopLog();
        }

        void drawHeaderEvent(Rect rect)
        {
            GUI.Label(rect, new GUIContent("Список событий", "Список всех обработанных событий."));
        }

        void drawEvent(Rect rect, int index, bool active, bool focused)
        {
            EventLogLine line = Sender.Log.events[index];
            GUI.Label(getRect(ref rect, 90), new GUIContent(string.Format("{0:##.000}", line.time), "Время начала события"));
            GUI.Label(getRect(ref rect, 100), new GUIContent(line.EventName, "Название события"));
            GUI.Label(getRect(ref rect, 100), new GUIContent(line.SourceName, "Источник события"));
        }

        void selectEvent(ReorderableList list)
        {
            if (lastEvent != null && listReciever != null)
            {
                clearListReceiver();
            }
            lastEvent = Sender.Log.events[list.index];
            listReciever = new ReorderableList(lastEvent.receivers, typeof(ReceiverLogLine), false, true, false, false);
            listReciever.drawHeaderCallback += drawHeaderReceiver;
            listReciever.drawElementCallback += drawReceiver;
        }

        void drawHeaderReceiver(Rect rect)
        {
            GUI.Label(rect, new GUIContent("Список получателей", "Список всех переданных по назначению событий."));
        }

        void drawReceiver(Rect rect, int index, bool active, bool focused)
        {
            ReceiverLogLine line = lastEvent.receivers[index];
            GUI.Label(getRect(ref rect, 90), new GUIContent(string.Format("{0:##.000}", line.time), "Время приема события"));
            GUI.Label(getRect(ref rect, 90), new GUIContent(string.Format("{0:##.000}", line.time), "Время завершения обработки события"));
            GUI.Label(getRect(ref rect, 100), new GUIContent(line.Name, "Получатель события"));
            GUI.Label(getRect(ref rect, 20), new GUIContent(string.Format("{0}", line.Priority), "Приоритет события"));
            GUI.Label(getRect(ref rect, 100), new GUIContent(string.Format("{0}", line.Stage), "Состояние"));
        }

        private Rect getRect(ref Rect rect, float width)
        {
            Rect r = new Rect(rect.x, rect.y, width, rect.height);
            rect.x = rect.x + width + 5;
            rect.width = rect.width - width + 5;
            return r;
        }

        /*public bool Check(EventsCheckStage stage, SimpleEvent evnt, UnityEngine.Object receiver)
        {
            bool checkOut = true;
            EventsCheckSetup setupLoc = ToSetup();
            setups = new EventsCheckSetup[1];
            setups[0] = setupLoc;
            foreach (EventsCheckSetup setup in setups)
            {
                checkOut = setup.Check(stage, evnt, receiver) && checkOut;
            }

            return checkOut;
        }


        EventsCheckSetup ToSetup()
        {
            EventsCheckSetup setup = new EventsCheckSetup();
            setup.EventName = EventName;
            setup.Stage = Stage;
            setup.Caller.Target = Caller;
            setup.Receiver.Target = Receiver;
            return setup;
        }

        void formSetup(EventsCheckSetup setup)
        {
            EventName = setup.EventName;
            Stage = setup.Stage;
            Caller = setup.Caller.Target;
            Receiver = setup.Receiver.Target;
        } */

    }

}

