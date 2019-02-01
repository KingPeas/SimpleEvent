using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KingDOM.Event;
using KingDOM.Event.Debug;

[Serializable]
public class EventLogLine
{

    public List<ReceiverLogLine> receivers;
    public Type SourceType = null;
    public UnityEngine.Object Source = null;
    public string SourceName = "";
    public string SourceTag = "";
    public int SourceLayerMask = Physics.AllLayers | Physics2D.AllLayers;
    public string EventName = "";
    [NonSerialized]
    public SimpleEvent Event = null;

    public float time = 0;

    public EventLogLine(EventCheckArgs args)
    {
        object target = args.Event.target;
        SourceType = target != null ? target.GetType(): null;
        Source = target as UnityEngine.Object;
        SourceName = Source != null ? Source.name : "";
        Component component = target as Component;
        SourceTag = component != null ? component.tag : "";
        SourceLayerMask = component != null ? component.gameObject.layer: -1;
        EventName = args.Event.eventName;
        Event = args.Event;
        receivers = new List<ReceiverLogLine>();
        ReceiverLogLine receiver = new ReceiverLogLine(args);
        time = Time.time;
        receivers.Add(receiver);
    }

    public void Update(EventCheckArgs args)
    {
        if (Event != args.Event)
        {
            Debug.LogError("Попытка обновить запись чужого события.");
            return;
        }
        var receiver = receivers.Find(r => r.Object == args.Receiver && r.Priority == args.Priority && (r.Stage & (EventsCheckStage.Error | EventsCheckStage.Stop)) == 0);
        if (receiver != null)
        {
            receiver.Update(args);
        }
        else
        {
            receivers.Add(new ReceiverLogLine(args));
        }  
    }
    //public int TargetLayerMask2D = Physics2D.AllLayers;

    /*public bool Check(UnityEngine.Object target)
    {
        if (target == null)
            return false;

        bool ret = true;
        ret = ret && (TargeType == null || target.GetType().IsAssignableFrom(TargeType));
        ret = ret && (Target == null || target == Target);
        ret = ret && (string.IsNullOrEmpty(TargetName) || target.name == TargetName);
        ret = ret && (string.IsNullOrEmpty(TargetTag) || (target is Component && (target as Component).tag == TargetName));
        if (target is Component)
        {
            int layer = ((target as Component).gameObject.layer;
            ret = ret &&
                  (TargetLayerMask == Physics.AllLayers ||
                   (layer & TargetLayerMask) > 0);
            ret = ret &&
                  (TargetLayerMask2D == Physics2D.AllLayers ||
                   (layer & TargetLayerMask2D) > 0);
        }

        return ret;
    }*/
}
