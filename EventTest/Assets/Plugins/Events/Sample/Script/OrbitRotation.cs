using UnityEngine;
using System.Collections;

public class OrbitRotation : MonoBehaviour {

	public float MinVelocity = 0.0f;
	public float MaxVelocity = 0.0f;
	public Vector3 StartAngleRotation = Vector3.zero;
	private float velocity = 0.0f;
	private Vector3 angleRotation = Vector3.zero;
	// Use this for initialization
	void Start () {
		if (MaxVelocity - MinVelocity > 0.0f) velocity = Random.value * (MaxVelocity - MinVelocity) + MinVelocity;
		angleRotation = StartAngleRotation * velocity;
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion q = Quaternion.Euler(angleRotation);
		transform.localRotation = transform.localRotation * q;
	}
}
