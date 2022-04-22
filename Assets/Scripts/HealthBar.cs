using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region Variables
    public Slider slider;
	#endregion

    public void SetMaxHealth(int maxHealth)
    {
        if (slider)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        } 
        else SetSliderVariable(maxHealth);
    }

    public void SetCurrentHealth(int currentHealth)
    {
        if (slider)
            slider.value = currentHealth;
    }

    private void SetSliderVariable(int maxHealth)
    {
        Debug.Log($"Slider was not found at {gameObject.name} looking for possible attachement");
        slider = GetComponent<Slider>();
        if (slider == null) slider = GetComponentInParent<Slider>();
        if (slider)
        {
            Debug.Log("Slider was found and attach to the variable.");
            SetMaxHealth(maxHealth);
        }
    }
}
