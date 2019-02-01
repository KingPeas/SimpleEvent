using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderInfo : MonoBehaviour
{

    public Text sliderInfo = null;
    public string format = "{0}";
    public float scalar = 1.0f;
    public float from = 0.0f;

    void Start()
    {
        if (sliderInfo == null) sliderInfo = GetComponent<Text>();
    }

    public void OnSliderChange(float value)
    {
        if (sliderInfo != null) sliderInfo.text = string.Format(format, value * scalar + from);
    }
}
