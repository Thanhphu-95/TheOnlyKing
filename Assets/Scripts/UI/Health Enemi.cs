using UnityEngine;
using UnityEngine.UI;

public class HealthEnemi : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }    
    public void SetHealth(int health)
    {
        slider.value = health;
    }    
}
