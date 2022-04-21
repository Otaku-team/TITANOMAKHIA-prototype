using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    #region Variables
    public Slider slider;
    #endregion

    public void SetMaxStamina(int maxStamina)
    {
        if (slider)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }
        else SetSliderVariable(maxStamina);
    }

    public void SetCurrentStamina(int currentStamina)
    {
        if (slider)
            slider.value = currentStamina;
    }

    private void SetSliderVariable(int maxHealth)
    {
        Debug.Log($"Slider was not found at {gameObject.name} looking for possible attachement");
        slider = GetComponent<Slider>();
        if (slider == null) slider = GetComponentInParent<Slider>();
        if (slider)
        {
            Debug.Log("Slider was found and attach to the variable.");
            SetMaxStamina(maxHealth);
        }
    }
}
