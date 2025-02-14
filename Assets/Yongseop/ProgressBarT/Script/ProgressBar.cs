using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Range(0, 100)]
    public float fillValue = 0;
    public Image barFillImage;
    public RectTransform handlerEdgeImage;
    public RectTransform fillHandler;
    public Text percentageText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        printPercentage((int)fillValue);
        fillBarValue(fillValue);
    }

    void printPercentage(int value)
    {
        percentageText.text = value.ToString() + " %";
    }
    void fillBarValue(float value)
    {
        value = (value <= 2f) ? 2f : value;
        value = (value >= 98.5f) ? 98.5f : value;
        float fillAmount = (value / 100f);
        barFillImage.fillAmount = fillAmount;
        float width = barFillImage.rectTransform.rect.width;
        fillHandler.localPosition = new Vector3(barFillImage.fillAmount * width - 13.5f, 0, 0);
        handlerEdgeImage.localPosition = new Vector3(-(barFillImage.fillAmount * width) + width / 2 - 3f, 0, 0);
    }
}
