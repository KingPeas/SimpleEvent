using UnityEngine;
using System.Collections;
using KingDOM.Event;
using UnityEngine.UI;

public class CenterController2 : CenterController
{

    public Slider sliderPower = null;
    protected override void OnPlaying()
    {
        //***************************************************************************************
        // If time is up, initialize current Event and send it with its parameter for all subscribers. 
        float power = 1.0f;
        if (sliderPower != null) power = sliderPower.value;
        Sender.SendEvent(Planet.getEventName(), this, EventParm.V_POWER, power);
        //***************************************************************************************
    }
}
