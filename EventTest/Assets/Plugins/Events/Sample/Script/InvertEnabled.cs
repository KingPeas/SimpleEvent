using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InvertEnabled : MonoBehaviour
{

    private Image image = null;

    public bool InverseEnabled
    {
        get { return image != null ? image.enabled : false; }
        set { if (image != null) image.enabled = !value; }
    }
    // Use this for initialization
	void Start ()
	{
	    image = GetComponent<Image>();
	    if (image != null) InverseEnabled = image.enabled;
	}
	
}
