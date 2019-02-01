using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    [Multiline(5)]
    public string Text = "";
    public uint MaxWidth = 0;
    public Text Output = null;
    public float TimeDisplay = 0.0f;
    private bool isUI = false;
    // Use this for initialization
	void Start () {
        
        isUI = this.transform is RectTransform;
        if (isUI)
        {
            EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
            trigger.triggers = new List<EventTrigger.Entry>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback = new EventTrigger.TriggerEvent();
            UnityAction<BaseEventData> callback = ToolTipController.Instance.OnEnter;
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback = new EventTrigger.TriggerEvent();
            callback = ToolTipController.Instance.OnExit;
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   
}
