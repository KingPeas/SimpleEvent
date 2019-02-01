using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadSimply()
    {
        Application.LoadLevel("SimpleEvent");
    }
    public void LoadParameter()
    {
        Application.LoadLevel("ParmEvent");
    }
    public void LoadPriority()
    {
        Application.LoadLevel("PriorityEvent");
    }
    public void LoadHierarchy()
    {
        Application.LoadLevel("HierarchicalEvent");
    }
}
