using UnityEngine;
using System.Collections;
using System;
using KingDOM.Event;

//[RequireComponent(typeof(SphereCollider))]
public class Sputnik : MonoBehaviour
{

    public string EventName = "";
    public Action<SimpleEvent> EventHandler = null;
	public float OrbitRadius = 1.0f;
    public int EventPriority = 0;
    private Planet myPlanet = null;
    private int myIdx = -1;
    private Vector3 startScale;
    
    // Use this for initialization
	void Start ()
	{
        startScale = transform.localScale;
	}
	
    void OnMouseEnter()
    {
        gameObject.transform.localScale = startScale * 1.5f;
    }
    void OnMouseExit()
    {
        gameObject.transform.localScale = startScale;
    }
    void OnMouseDown()
    {
        if (myPlanet != null && Store.Regime == Store.TestRegime.Record)
			destroyIndicator();   
    }
    void OnDestroy()
    {
        Sender.RemoveEvent(EventName, EventPriority, EventHandler);
    }
    public void Init(string EventName, Action<SimpleEvent> EventHandler)
    {
        Init(EventName, EventHandler, 0);
    }
    public void Init(string EventName, Action<SimpleEvent> EventHandler, int EventPriority )
    {
        this.EventName = EventName;
        this.EventHandler = EventHandler;
        this.EventPriority = EventPriority;
    }
    public string Tip
    {
        get { return (string.IsNullOrEmpty(EventName) || EventHandler == null) ? "Not initialized." : String.Format("Target : {1} \n\n Sattelite : {4} \n Event : {0} \n Handler : {2} \n Priority : {3}", EventName, (EventHandler.Target as Component).name, EventHandler.Method.Name, EventPriority, myIdx + 1); }
    }

    public Planet MyPlanet
    {
        get { return myPlanet; }
        set
        {
            myPlanet = value;
            if (myPlanet != null)
            {
                myIdx = myPlanet.getFreeIndicators();
                if (myIdx < 0 || myIdx >= myPlanet.maxIndicator)
                {
                    destroyIndicator();
                    return;
                }
                myPlanet.indicators[myIdx] = this;
				transform.localPosition = Quaternion.AngleAxis(myIdx * 30, Vector3.back) * Vector3.up * OrbitRadius;
                startScale = transform.localScale;
            }
            else
            {
                destroyIndicator();
            }
        }
    }

    public void destroyIndicator()
    {
        if (myPlanet != null && myIdx >=0 && myIdx < myPlanet.maxIndicator)
        {
            myPlanet.indicators[myIdx] = null;
        }
        GameObject.DestroyObject(transform.parent.gameObject);
    }
}
