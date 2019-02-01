using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class Fireworks : MonoBehaviour
{
    public AudioSource audioFireworks = null;
    public int startEmitter = 30;
	public float lifeTime =7.0f;
	private float waitTime = 0.0f;
	private float ripTime = 0.0f;
	private bool isStarted = false;
	private int rateEmit = 30;
    private static float OrderDelay = 0.0f;
    private static float OrderStep = 0.0f;

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
    void Awake()
    {
        //Debug.Log("Created freworks");

        waitTime = OrderDelay;
		ripTime = waitTime + lifeTime;
		rateEmit = startEmitter;
        if (audioFireworks == null) audioFireworks = GetComponent<AudioSource>();
        
        if (GetComponent<ParticleSystem>())
        {
            GetComponent<ParticleSystem>().startColor = new Color(Random.value, Random.value, Random.value);
            OrderDelay +=OrderStep;
			GetComponent<ParticleSystem>().emissionRate = 0;
            
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	    waitTime -= Time.deltaTime;
		ripTime -= Time.deltaTime;
			
		if (waitTime <= 0 && !isStarted)
	    {
			GetComponent<ParticleSystem>().Emit(rateEmit);
			if (audioFireworks != null)
	        {
	            if (Store.SoundOn) 
					audioFireworks.Play();
	        } 
			isStarted = true;
		}
        if (ripTime <= 0)
		{
                DestroyImmediate(gameObject);
	    }
	}

    public void SetPower(float power)
    {
        power = Mathf.Clamp01(power);
        if (GetComponent<ParticleSystem>() != null)
        {
            GetComponent<ParticleSystem>().Stop();
			rateEmit = (int)(startEmitter*power);
			isStarted = false;
        }
        if (audioFireworks != null) audioFireworks.volume *= power;
    }
}
