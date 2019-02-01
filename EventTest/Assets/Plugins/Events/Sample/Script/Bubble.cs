using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour
{
    public AudioSource audioBubble = null;
    private static float OrderDelay = 0.0f;
    private static float OrderStep = 0.0f;
    private float timeDelayLeft;
    public float timeGrow = 0.2f;
    private float timeGrowLeft;
    private bool audioStart = false;
    public float GrowFromScale = 2f;
    public float GrowToScale = 4.5f;
    private float ToScale;

    public static void SwitchPlay()
    {
        OrderDelay = 0.0f;
        OrderStep = 0.2f;
    }
    public static void SwitchRecord()
    {
        OrderDelay = 0.0f;
        OrderStep = 0.0f;
    }
    // Use this for initialization
	void Awake () {
        timeDelayLeft = OrderDelay;
        OrderDelay += OrderStep;
	    timeGrowLeft = timeGrow;
        transform.localEulerAngles = new Vector3(Random.value * 360, Random.value*360);
	    ToScale = GrowToScale;
	    OrbitRotation r = GetComponent<OrbitRotation>();
		if (audioBubble == null) audioBubble = GetComponent<AudioSource>();     
        
        if (r != null)
        {
            r.StartAngleRotation = new Vector3(Random.value * 2 - 1, Random.value * 2 - 1);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (timeDelayLeft > 0)
        {
            timeDelayLeft -= Time.deltaTime;
            return;
        }
        if (audioBubble != null && !audioStart)
        {
            if (Store.SoundOn) {
				audioBubble.Play();
			}
            audioStart = true;
            return;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(ToScale, ToScale, ToScale),
	                                        (timeGrow - timeGrowLeft)/timeGrow);
        if (timeGrowLeft > 0)
        {
            timeGrowLeft -= Time.deltaTime;
            return;
        }
        DestroyImmediate(gameObject);
    }

    public void SetPower(float power)
    {
        power = Mathf.Clamp01(power);
        ToScale = Mathf.Lerp(GrowFromScale, GrowToScale, power);
        if (audioBubble != null) audioBubble.volume *= power;
    }

}
