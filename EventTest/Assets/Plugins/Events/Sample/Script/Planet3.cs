using KingDOM.Event;
using UnityEngine;
using System.Collections;

public class Planet3 : Planet {
    public override void OnMouseDown()
    {
        if (Store.Regime == Store.TestRegime.Record)
        {
            int idx = getFreeIndicators();
            if (idx < 0) return;
            Sender.AddEvent(getEventName(), Store.Priority, getEventHandler());
            GameObject go = Instantiate(getIndicator()) as GameObject;
            go.transform.position = transform.position;
            Sputnik i = go.GetComponentInChildren<Sputnik>();
            if (i == null) i = go.AddComponent<Sputnik>();
            go.transform.parent = this.gameObject.transform;
            i.Init(getEventName(), getEventHandler(), Store.Priority);
            i.MyPlanet = this;
            CreateEventReaction();
        }
    }
}
