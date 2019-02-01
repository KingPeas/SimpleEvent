using System;
using UnityEngine;
using System.Collections;
using KingDOM.Event;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SphereCollider))]
public class Planet : MonoBehaviour {

    public float Population = 0;
    public float Radius = 50.0f;

    public string Tip
    {
        get
        {
            return string.Format("{0} \n\n Radius : {1} KM\n Population : {2} M PPL", name, (int) (Radius*1000),
                                 (int) Population);
        }
    }

    #region public field
    //public Status status;
    public GameObject IndicatorFireworks;
    public GameObject IndicatorShield;
    public GameObject IndicatorExplosion;
    public GameObject FireworkObject;
    public GameObject ShieldObject;
    public GameObject ExplosiveObject;
    public int maxIndicator = 12;
    public Sputnik[] indicators = new Sputnik[12];
    #endregion
    // Use this for initialization
	void Awake ()
	{
        for (int i = 0; i < maxIndicator; i++ )
        {
            indicators[i] = null;
        }
	    Population = Random.value*5000;
	    Radius = Random.value*40 + 15;
        Sender.AddEvent(EventName.E_CHANGE, hnChangeStatus);
	}

    void OnDestroy()
    {
        Sender.RemoveEvent(EventName.E_CHANGE, hnChangeStatus);
    }

    public virtual void OnMouseDown()
    {
        if (Store.Regime == Store.TestRegime.Record)
        {
            int idx = getFreeIndicators();
            if (idx < 0) return;
            Sender.AddEvent(getEventName(), getEventHandler());
            // create sattelite
            GameObject go = Instantiate(getIndicator()) as GameObject;
            go.transform.position = transform.position;
            Sputnik i = go.GetComponentInChildren<Sputnik>();
            if (i == null) i = go.AddComponent<Sputnik>();
            go.transform.parent = this.gameObject.transform;
            i.Init(getEventName(), getEventHandler(), 0);
            i.MyPlanet = this;
			CreateEventReaction();
        }
    }
	
	// Update is called once per frame
	virtual public void Update ()
	{
	    Population += (Random.value - 0.5f)*100;
	    Population = Population > 0 ? Population : 0;
	}

    protected void CreateFirework()
    {
        if (FireworkObject == null) return;
        GameObject go = Instantiate(FireworkObject) as GameObject;
        Fireworks fireworks = go.GetComponent<Fireworks>();
        if (fireworks == null)
        {
            DestroyImmediate(go);
            return;
        }
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
    }
    protected void CreateShield()
    {
        if (ShieldObject == null) return;
        GameObject go = Instantiate(ShieldObject) as GameObject;
        Bubble shield = go.GetComponent<Bubble>();
        if (shield == null)
        {
            DestroyImmediate(go);
            return;
        }
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
    }
    protected void CreateExplosion()
    {
        if (ExplosiveObject == null) return;
        GameObject go = Instantiate(ExplosiveObject) as GameObject;
        Boom boom = go.GetComponent<Boom>();
        if (boom == null)
        {
            DestroyImmediate(go);
            return;
        }
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
    }

    public int getFreeIndicators()
    {
        for (int i = 0; i < maxIndicator; i++)
        {
            if (indicators[i] == null) return i;
        }
        return -1;
    }

    void hnChangeStatus(SimpleEvent evnt)
    {
        for (int i = 0; i < maxIndicator; i++ )
        {
            if (indicators[i] is Sputnik)
                (indicators[i] as Sputnik).destroyIndicator();
        }
    }
    virtual public void hnFireworks(SimpleEvent evnt)
    {
        CreateFirework();
    }
    virtual public void hnShield(SimpleEvent evnt)
    {
        CreateShield();
    }
    virtual public void hnExplosion(SimpleEvent evnt)
    {
        CreateExplosion();
    }

    protected void CreateEventReaction()
    {
        switch (Store.TypeEvent)
        {
            case Store.TestEvent.Fireworks:
                CreateFirework();
                break;
            case Store.TestEvent.Shield:
                CreateShield();
                break;
            case Store.TestEvent.Explosion:
                CreateExplosion();
                break;
        }

    }

    protected Action<SimpleEvent> getEventHandler()
    {
        switch (Store.TypeEvent)
        {
            case Store.TestEvent.Fireworks:
                return hnFireworks;
                break;
            case Store.TestEvent.Shield:
                return hnShield;
                break;
            case Store.TestEvent.Explosion:
                return hnExplosion;
                break;
        }

        return hnFireworks;
    }

    public static string getEventName()
    {
        switch (Store.TypeEvent)
        {
            case Store.TestEvent.Fireworks:
                return EventName.E_FIREWORKS;
                break;
            case Store.TestEvent.Shield:
                return EventName.E_SHIELD;
                break;
            case Store.TestEvent.Explosion:
                return EventName.E_EXPLOSION;
                break;
        }

        return EventName.E_FIREWORKS;
    }

    protected GameObject getIndicator()
    {
        switch (Store.TypeEvent)
        {
            case Store.TestEvent.Fireworks:
                return IndicatorFireworks;
                break;
            case Store.TestEvent.Shield:
                return IndicatorShield;
                break;
            case Store.TestEvent.Explosion:
                return IndicatorExplosion;
                break;
        }

        return IndicatorFireworks;
    }
}
