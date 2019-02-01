using System;
using KingDOM.Event;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CenterController : MonoBehaviour
{

    //public int Priority = 0;
    // Information window
	public Text info = null;
    public Text help = null;
    public Text regime = null;
    public Toggle MusicSwitch = null;
    public Toggle SoundSwitch = null;
    public AudioSource MusicSource = null;
    [HideInInspector]
    public float timeRemaining = 0;
    private string startText;

    public bool SoundState
    {
        get { return Store.SoundOn; }
        set { Store.SoundOn = value; }
    }

    public bool MusicState
    {
        get { return Store.MusicOn; }
        set
        {
            Store.MusicOn = value;
            if (MusicSource != null) MusicSource.enabled = value;
        }
    }

    // Use this for initialization
	void Start ()
	{
	    // Set up regime
        Store.Regime = Store.TestRegime.Record;
        // It shows that information window will be displaed for 4 seconds after scene start
        timeRemaining = 4.0f;
        if (info != null) startText = info.text;
	    if (SoundSwitch != null) SoundState = SoundSwitch.isOn;
        if (MusicSwitch != null) MusicState = MusicSwitch.isOn;
	}

	void Update () {
        if (SoundSwitch != null && SoundSwitch.isOn != SoundState) SoundState = SoundSwitch.isOn;
        if (MusicSwitch != null && MusicSwitch.isOn != MusicState) MusicState = MusicSwitch.isOn;
        if (timeRemaining > 0)
	    {
	        //in play Regime
            if (Store.Regime == Store.TestRegime.Play)
            {
                timeRemaining -= Time.deltaTime;
                // we are in the waiting regime
                if (timeRemaining > 0)
                {
                    //update info
                    if (info != null) info.text = (int)timeRemaining > 0 ? "Waiting " + ((int)timeRemaining).ToString() : "Go!!!";
                }
			    else
			    {
                    timeRemaining = 0;
                    OnPlaying();
                    if (info != null) info.text = "";
			    }
            }
            //in record regime
            if (Store.Regime == Store.TestRegime.Record)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    if (info != null) info.text = "";
                }
            }
	    }
	    if (regime != null) regime.text = Store.Regime.ToString();
	}
    /// <summary>
    /// Execute after clicking Record
    /// </summary>
	public void OnRecord()
	{
		Fireworks.SwitchRecord();
        Bubble.SwitchRecord();
        Boom.SwitchRecord();
        Store.Regime = Store.TestRegime.Record;
	}
	/// <summary>
    /// Execute after clicking Play
	/// </summary>
	virtual public void OnPlay()
	{
	    Fireworks.SwitchPlay();
        Bubble.SwitchPlay();
        Boom.SwitchPlay();
        Store.Regime = Store.TestRegime.Play;
        timeRemaining = 4.0f;
	}
    /// <summary>
    /// Execute after clicking Stop
    /// </summary>
	public void OnStop()
	{
        Sender.SendEvent(EventName.E_CHANGE, this);
        if (info != null) info.text = startText;
	    timeRemaining = 4.0f;
	    OnRecord();
	}
    /// <summary>
    /// Execute when changing Event
    /// </summary>
    /// <param name="check">Selected firework</param>
    public void OnFireworks(bool check)
    {
        if (check) Store.TypeEvent = Store.TestEvent.Fireworks;
    }
    /// <summary>
    /// Execute when changing Event
    /// </summary>
    /// <param name="check">Selected Shield</param>
    public void OnShield(bool check)
    {
        if (check) Store.TypeEvent = Store.TestEvent.Shield;
    }
    /// <summary>
    /// Execute when changing Event
    /// </summary>
    /// <param name="check">Selected Explosion</param>
    public void OnExplosion(bool check)
    {
        if (check) Store.TypeEvent = Store.TestEvent.Explosion;
    }
    
    virtual protected void OnPlaying()
    {
        //***************************************************************************************
        // If time is up, initialize current Event and send it for all subscribers. 
        Sender.SendEvent(Planet.getEventName(), this);
        //***************************************************************************************
        
    }
}
