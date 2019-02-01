using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class Boom : MonoBehaviour
{
    public AudioSource audioBoom = null;
    public int startEmitter = 30;
    public float lifeTime = 7.0f;
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
        //Debug.Log("Created boom");

        waitTime = OrderDelay;
        ripTime = waitTime + lifeTime;
        rateEmit = startEmitter;
        if (audioBoom == null) audioBoom = GetComponent<AudioSource>();        
        
        if (GetComponent<ParticleSystem>())
        {
            GetComponent<ParticleSystem>().startColor = new Color(Random.value, Random.value, Random.value);
            OrderDelay += OrderStep;
            GetComponent<ParticleSystem>().emissionRate = 0;

        }

    }

    // Update is called once per frame
    void Update()
    {
        waitTime -= Time.deltaTime;
        ripTime -= Time.deltaTime;

        if (waitTime <= 0 && !isStarted)
        {
            GetComponent<ParticleSystem>().Emit(rateEmit);
            isStarted = true;
			if (audioBoom != null)
	        {
	            if (Store.SoundOn) 
					audioBoom.Play();
	        }
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
            rateEmit = (int)(startEmitter * power);
            isStarted = false;
        }
        if (audioBoom != null) audioBoom.volume *= power;
    }
}
