using System;
using KingDOM.Event;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KingDOM.Event.Debug;

[Serializable]
public class EventsLog {

    public List<EventLogLine> events = new List<EventLogLine>();
    private EventLogLine currentEvent = null;

    public void Init()
    {
        events = new List<EventLogLine>();
        currentEvent = null;
        Sender.startEvent += Add;
        Sender.endEvent += Add;
        Sender.stopEvent += Add;
        Sender.errorEvent += Add;

    }

    public void Destroy()
    {
        Sender.startEvent -= Add;
        Sender.endEvent -= Add;
        Sender.stopEvent -= Add;
        Sender.errorEvent -= Add; 
        events = new List<EventLogLine>();
    }
    public void Add(object target, EventCheckArgs args)
    {
        if (currentEvent == null || currentEvent.Event != args.Event)
        {
            if (currentEvent != null) currentEvent.Event = null;
            currentEvent = new EventLogLine(args);
            events.Add(currentEvent);
        }
        else
        {
            currentEvent.Update(args);
        }
        
    }
    public void Clear()
    {
        events = new List<EventLogLine>();
    }
}
