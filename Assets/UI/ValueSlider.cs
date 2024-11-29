using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueSlider : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Slider slider;
    public TMP_InputField inputField;

    private void OnValidate()
    {
        if (label == null)
        {
            label = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }

        if (inputField == null)
        {
            inputField = GetComponentInChildren<TMP_InputField>();
        }
    }

    public void Show(string labelText, float minValue, float maxValue, float value)
    {
        //clear listeners
        label.text = labelText;
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        Value = value;
    }
    
    public float Value
    {
        get => slider.value;
        set
        {
            slider.value = value;
            inputField.text = value.ToString();
        }
    }
    
    public void OnSliderValueChanged(float value)
    {
        inputField.text = value.ToString();
        OnValueChangedEvent(value);
    }
    
    public void OnInputFieldEndEdit(string value)
    {
        if (float.TryParse(value, out var floatValue))
        {
            slider.value = floatValue;
            OnValueChangedEvent(slider.value);
        }
        else
        {
            inputField.text = slider.value.ToString();
        }
    }
    
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out var floatValue))
        {
            slider.value = floatValue;
            OnValueChangedEvent(slider.value);
        }
    }
    
    private void OnEnable()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }
    
    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        inputField.onEndEdit.RemoveListener(OnInputFieldEndEdit);
        inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
    }
    
    //event on value changed
    public delegate void ValueChanged(float value);
    public event ValueChanged OnValueChanged;
    
    private void OnValueChangedEvent(float value)
    {
        OnValueChanged?.Invoke(value);
    }
}