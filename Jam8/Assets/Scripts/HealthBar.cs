using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;  
public class HealthBar : MonoBehaviour
{ 
    private float fillAmount;

    [SerializeField]
    private Image content;

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Color fullColor;

    [SerializeField]
    private Color lowColor;

    [SerializeField]
    bool lerpcolors;

    public float MaxValue
    {
        get;
        set;
    }
    public float Value
    {
        set
        {
            string[] temp = valueText.text.Split(':');
            valueText.text = temp[0] + ":" + value;
            fillAmount = Map(value, 0, MaxValue, 0, 1); 
        }
    }
    // Use this for initialization
    void Start()
    {
        if(lerpcolors)
        {
            content.color = fullColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HealthValue();
    }

    private void HealthValue()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
        if(lerpcolors)
        {
            content.color = Color.Lerp(lowColor, fullColor, fillAmount);
        }        
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
