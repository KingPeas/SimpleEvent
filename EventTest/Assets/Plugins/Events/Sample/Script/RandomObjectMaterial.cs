using UnityEngine;
using System.Collections;

public class RandomObjectMaterial : MonoBehaviour {

	public Texture[] mainTextures = new Texture[1];
	public Texture[] bumpTextures = new Texture[1];
	private bool isOk = true;
	private Material newMaterial;
	// Use this for initialization
	void Start () {
		isOk = mainTextures.Length == bumpTextures.Length && mainTextures.Length > 0;
		isOk = isOk && GetComponent<Renderer>() != null && GetComponent<Renderer>().sharedMaterial != null; 
		if (isOk){
			newMaterial = new Material(GetComponent<Renderer>().sharedMaterial);
			int idx = (int)Mathf.Round(Random.value * (mainTextures.Length - 1));
			newMaterial.SetTexture("_MainTex", mainTextures[idx]);
			newMaterial.SetTexture("_BumpMap", bumpTextures[idx]);
			GetComponent<Renderer>().sharedMaterial = newMaterial;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
