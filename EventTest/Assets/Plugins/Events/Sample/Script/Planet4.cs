using KingDOM.Event;
using UnityEngine;
using System.Collections;

public class Planet4 : Planet
{
    private bool waitPlaying = false;
    private CenterController4 center= null;
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
        if (Store.Regime == Store.TestRegime.Play)
        {
            waitPlaying = true; // When Play is clicked, wait for 3 seconds
            if (center == null)
            {
                center = FindObjectOfType<CenterController4>();
            }
            if (center != null) center.timeRemaining = 4.0f;
        }
    }

    public override void Update()
    {
        base.Update();
        if (waitPlaying && center != null && center.timeRemaining <= 0)
        {
            waitPlaying = false;
            Sender.SendEventHierarchy(Planet.getEventName(), this);
        }
    }
}
