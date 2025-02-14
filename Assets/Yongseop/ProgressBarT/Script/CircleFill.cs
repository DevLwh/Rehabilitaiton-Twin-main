using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFill : MonoBehaviour
{
    public int? value = null;
    public int? maxValue = null;
    [Range(0, 1)] public float fillValue = 0;
    public Image circleFillImage;
    public RectTransform handlerEdgeImage;
    public RectTransform fillHandler;

    public Text valueText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (float.IsNaN(fillValue))
            fillValue = 0;
        fillCircleValue(fillValue);
        if (value != null)
            valueText.text = value.ToString();
        //if (maxValue != null)
        //    fillCircleValue((float)(value / maxValue) * 100);
    }

    void fillCircleValue(float value = 180f)
    {
        float fillAmount = value;

        circleFillImage.fillAmount = fillAmount;
        
        float angle = - fillAmount * 360;
        fillHandler.localEulerAngles = new Vector3(0, 0, -angle);
        Vector3 vector3 = new Vector3(0, 0, angle);
        handlerEdgeImage.localEulerAngles = vector3;
    }
}
