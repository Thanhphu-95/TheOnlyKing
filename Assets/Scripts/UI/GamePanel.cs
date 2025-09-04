using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Slider playerHealthSlider;

    private int playerMaxHealth;

    public void SetMaxHealth(int maxHealth)
    {
        playerMaxHealth = maxHealth;
        playerHealthSlider.maxValue = maxHealth;
    }
    public void UpdateHealth(int currentHealth)
    {
        playerHealthSlider.value = currentHealth;
    }

    public void ResetHealth()
    {
        UpdateHealth(playerMaxHealth);
    }


    public void ActiveBossHealth(int maxHealth)
    {

    }
}
