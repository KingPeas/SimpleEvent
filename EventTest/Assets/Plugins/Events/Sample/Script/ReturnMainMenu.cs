using UnityEngine;
using System.Collections;

public class ReturnMainMenu : MonoBehaviour {

	// Use this for initialization
	public void OnClickBtn () {
	    Store.Regime = Store.TestRegime.Record;
        Store.TypeEvent = Store.TestEvent.Fireworks;
        Application.LoadLevel("MainMenu");
	}
	
}
