using UnityEngine;
using System.Collections;

public class RandomColor : MonoBehaviour {

	public Color FromColor = new Color(0,0,0,0);
    public Color ToColor = new Color(1,1,1,1);
    private Material newMaterial;
    private bool isOk = true;
    // Use this for initialization
	void Start () {
        isOk = GetComponent<Renderer>() != null && GetComponent<Renderer>().sharedMaterial != null;
        if (isOk)
        {
            newMaterial = new Material(GetComponent<Renderer>().sharedMaterial);
            Color color = new Color(Random.value * (ToColor.r - FromColor.r) + FromColor.r,
                                    Random.value * (ToColor.g - FromColor.g) + FromColor.g,
                                    Random.value * (ToColor.b - FromColor.b) + FromColor.b,
                                    Random.value * (ToColor.a - FromColor.a) + FromColor.a);
            newMaterial.SetColor("_Color", color);
            GetComponent<Renderer>().sharedMaterial = newMaterial;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
