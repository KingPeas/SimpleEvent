using UnityEngine;
using System.Collections;

public class CenterController4 : CenterController
{
    public void OnSliderChange(float priority)
    {
        // First, the event will pass from parent to source (priority> 0), then from the source to the parent (priority <=0)
        Store.Priority = (int)(priority * 10 - 5); 
    }

    public override void OnPlay()
    {
        base.OnPlay();
        timeRemaining = 0f;
    }

    protected override void OnPlaying()
    {
        //base.OnPlaying();  //The event will be generated when clicking the planet
    }
}
