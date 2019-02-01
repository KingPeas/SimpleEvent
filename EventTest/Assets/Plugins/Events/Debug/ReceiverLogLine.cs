using System;
using UnityEngine;
using System.Collections;
using KingDOM.Event.Debug;

[Serializable]
public class ReceiverLogLine  {

    public Type Type = null;
    public UnityEngine.Object Object = null;
    public string Name = "";
    public string Tag = "";
    public LayerMask Mask;
    public  EventsCheckStage Stage = EventsCheckStage.None;
    public int Priority = 0;
    public float time = 0;
    public float time2 = 0;

    public ReceiverLogLine(EventCheckArgs args)
    {
        object target = args.Receiver;
        Type = target != null ? target.GetType() : null;
        Object = target as UnityEngine.Object;
        Name = Object != null ? Object.name : "";
        Component component = target as Component;
        Tag = component != null ? component.tag : "";
        Mask = component != null ? component.gameObject.layer : -1;
        Stage = args.Stage;
        Priority = args.Priority;
        time = Time.time;
        time2 = 0;
    }

    public void Update(EventCheckArgs args)
    {
        if (args.Receiver != Object)
        {
            Debug.LogError("Попытка обновить запись чужого получателя события.");
            return;
        }
        Stage = Stage | args.Stage;
        time2 = Time.time;
    }

}
