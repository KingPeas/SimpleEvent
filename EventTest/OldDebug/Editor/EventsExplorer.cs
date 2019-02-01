using System;
using KingDOM.Event;
using KingDOM.Event.Debug;
using UnityEditor;
using UnityEngine;
using System.Collections;

namespace KingDOM.Event.Debug
{
    public class EventsExplorer : EditorWindow
    {

        // Регистрируем пункт меню и функцию, выполняющую открытие окна
        [MenuItem("Window/KingDOM/Events Explorer")]
        public static void CreateWindow()
        {
            EventsExplorer window = GetWindow<EventsExplorer>();
            window.title = "Events Explorer Window";
            //EventsChecker.SetExplorer(window);
        }

        public bool EventToPause = false;
        [Range(0, 10)] public int z = 3;
        public string EventName = "";
        //[SceneName]
        
        public EventsCheckStage Stage = EventsCheckStage.Before;
        public UnityEngine.Object Caller = null;
        public UnityEngine.Object Receiver = null;
        public EventsCheckSetup[] setups = new EventsCheckSetup[0];
        // Функция отрисовки окна
        public void OnGUI()
        {
            GUILayout.BeginVertical();
            SerializedObject so = new SerializedObject(this);
            SerializedProperty sp = so.FindProperty("z");
            EditorGUILayout.PropertyField(sp, new GUIContent("шаг"));
            EventToPause = EditorGUILayout.Toggle(new GUIContent("Event Pause"), EventToPause);
            EventName = EditorGUILayout.TextField("Event Name", EventName);
            Stage = (EventsCheckStage)Enum.ToObject(typeof(EventsCheckStage), EditorGUILayout.EnumPopup("Execute", Stage));
            Caller = EditorGUILayout.ObjectField(new GUIContent("Caller"), Caller, typeof(UnityEngine.Object));
            Receiver = EditorGUILayout.ObjectField(new GUIContent("Receiver"), Receiver, typeof(UnityEngine.Object));
            GUILayout.EndVertical();
            so.ApplyModifiedProperties();
        }

        public bool Check(EventsCheckStage stage, SimpleEvent evnt, UnityEngine.Object receiver)
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

        void OnDestroy()
        {
            EventsChecker.SetExplorer(null);
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
        }
        
    }

}

