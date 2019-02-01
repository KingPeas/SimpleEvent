using UnityEngine;
using System.Collections;

public class Store  {

	public enum TestRegime
	{
	    Record,
        Play,
        Stop
	}

    public enum TestEvent
    {
        Fireworks,
        Shield,
        Explosion
    }

    public static TestRegime Regime = TestRegime.Record;
    public static TestEvent TypeEvent = TestEvent.Fireworks;
    public static bool MusicOn = true;
    public static bool SoundOn = true;
	public static int Priority = 0;
}
