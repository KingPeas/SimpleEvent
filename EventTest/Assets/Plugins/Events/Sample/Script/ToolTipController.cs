using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipController : MonoBehaviour
{

    private enum TipState
    {
        None,
        Old,
        New
    }
    public GameObject LastObject = null;
    public float Delay = 0.0f;
    public Text Label = null;
    public Image Background = null;
    private ToolTip toolTip = null;
    private string toolTipText = "";
    private Camera[] cameras   ;
    private float timer = 0.0f;
    private Vector3 lastMousePosition = Vector3.zero;
    private GameObject LastObjectUI = null;
    private static ToolTipController instance;

    public static ToolTipController Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null)
            Debug.LogError("The scene has got more than one tooltip controller.");
        instance = this;
    }
    
    // Use this for initialization
	void Start ()
	{
        ResearchCameras();
        //Hide();
	    
	}


    // Update is called once per frame
	void Update ()
	{
	    if (lastMousePosition != Input.mousePosition && Delay > 0)
	    {
	        timer = 0.0f;
	        lastMousePosition = Input.mousePosition;
	        LastObject = null;
            Hide();
	        return;
	    }
	    else
	    {
	        if (timer <= Delay)
	        {
	            timer += Time.deltaTime;
	            return;
	        }
	    }
        TipState newState = TipState.None;
	    if (LastObjectUI != null)
	    {
	       newState = (TipState)Mathf.Max((int)newState, (int)GetTargetUI());
	    }
        else
        {
            foreach (Camera cam in cameras)
            {
                newState = (TipState)Mathf.Max((int)newState, (int)GetTargetCamera(cam));
            
	        }
        }
	    switch (newState)
	    {
	        case TipState.None:
                Hide();
	            LastObject = null;
                    break;
            case TipState.New:
                Show();
	            break;
	    }
	}

    public void ResearchCameras()
    {
        cameras = FindObjectsOfType(typeof (Camera)) as Camera[];
    }

    TipState GetTargetUI()
    {
        return GetTarget(LastObjectUI);
    }

    TipState GetTargetCamera(Camera cam)
    {
        // create the ray on the mouse position from camera
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
            return TipState.None;

        GameObject currentObject = hit.collider.gameObject;
        return GetTarget(currentObject);
    }

    TipState GetTarget(GameObject fromObject)
    {
        if (fromObject == null) return TipState.None;
        if (fromObject == LastObject) return TipState.Old;

        GameObject currentObject = fromObject;
        do
        {

            toolTip = currentObject.GetComponent<ToolTip>();

            if (toolTip != null)
            {
                Show();
                LastObject = fromObject;
                return TipState.New;
            }
            else
            {
                Component[] components = currentObject.GetComponents<Component>();
                foreach (Component component in components)
                {
                    Type type = component.GetType();
					FieldInfo fieldInfo = type.GetField("Tip");
					PropertyInfo propertyInfo;
                    toolTipText = fieldInfo != null ? fieldInfo.GetValue(component) as string: "";
                    if (string.IsNullOrEmpty(toolTipText))
					{
						fieldInfo = type.GetField("tip");
						toolTipText = fieldInfo != null ? fieldInfo.GetValue(component) as string: toolTipText;
					}
                    if (string.IsNullOrEmpty(toolTipText))
					{
                        propertyInfo = type.GetProperty("Tip");
						toolTipText = propertyInfo != null? propertyInfo.GetValue(component, null) as string: toolTipText;
					}
                    if (string.IsNullOrEmpty(toolTipText))
					{
                        propertyInfo = type.GetProperty("tip");
						toolTipText = propertyInfo != null ? propertyInfo.GetValue(component, null) as string: toolTipText;
					}
                    if (!string.IsNullOrEmpty(toolTipText))
                    {
                        Show();
                        LastObject = fromObject;
                        return TipState.New;
                    }
                }
            }
            if (currentObject.transform.parent != null) 
                currentObject = currentObject.transform.parent.gameObject;
            else 
                currentObject = null;
        } while (currentObject != null);
        return TipState.None;
    }
    void Show()
    {
        if (Background != null)
            Background.gameObject.SetActive(true);
        if (Label != null)
        {
            Label.text = toolTip != null ? toolTip.Text: toolTipText;
            Label.gameObject.SetActive(true);
        }

    }

    void Hide()
    {
        if (Background != null)
            Background.gameObject.SetActive(false);
        if (Label != null)
            Label.gameObject.SetActive(false);
    }

    public void OnEnter(BaseEventData data)
    {
        PointerEventData pData = data as PointerEventData;
        if (pData != null && pData.pointerCurrentRaycast.gameObject)
        {
            LastObjectUI = pData.pointerCurrentRaycast.gameObject;
        }
    }

    public void OnExit(BaseEventData data)
    {
        PointerEventData pData = data as PointerEventData;
        if (pData != null && pData.pointerEnter)
        {
            LastObjectUI = null;
            Hide();
        }
    }
}
