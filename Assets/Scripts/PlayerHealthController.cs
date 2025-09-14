using System.Collections;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth = 100;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private float flashTime = 0.1f;

    [Header("Components")]
    [SerializeField] private SpriteRenderer[] playerSprites;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject priest;

    private float invincibilityCounter = 0f;
    private float flashCounter = 0f;

    private int hurtParamHash = Animator.StringToHash("HurtPr");
    private int deadParamHash = Animator.StringToHash("Dead");

    // Property MaxHealth - thay đổi sẽ tự cập nhật UI
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;
            if (_currentHealth > _maxHealth) _currentHealth = _maxHealth; // giữ currentHealth không vượt maxHealth
            UpdateUI(); // cập nhật Slider và Text
        }
    }

    // Property CurrentHealth - thay đổi sẽ tự cập nhật UI
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth); // giới hạn trong [0, maxHealth]
            UpdateUI(); // cập nhật Slider và Text
        }
    }

    private void Start()
    {
        UpdateUI(); // khởi tạo UI
    }

    private void Update()
    {
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;

            if (flashCounter <= 0)
            {
                foreach (var sprite in playerSprites)
                    sprite.enabled = !sprite.enabled;
                flashCounter = flashTime;
            }

            if (invincibilityCounter <= 0)
            {
                foreach (var sprite in playerSprites)
                    sprite.enabled = true;
                flashCounter = 0f;
            }
        }
    }

    public void HealPlayer(int amount)
    {
        CurrentHealth += amount; // Dòng thay đổi: dùng property CurrentHealth để tự cập nhật UI
    }

    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount; // Dòng thay đổi: dùng property MaxHealth để tự cập nhật UI
    }

    public void DamagePlayer(int damageAmount)
    {
        if (invincibilityCounter <= 0)
        {
            CurrentHealth -= damageAmount; // Dòng thay đổi: dùng property CurrentHealth để tự cập nhật UI

            if (priest.activeSelf && !player.activeSelf)
                animator.SetTrigger(hurtParamHash);

            if (CurrentHealth <= 0)
            {
                animator.SetTrigger(deadParamHash);
                if (TheOnlyKingManager.HasInstance)
                    TheOnlyKingManager.Instance.LosseGame();
            }
            else
            {
                invincibilityCounter = invincibilityTime;
            }
        }
    }

    public IEnumerator HealOverTime(int totalHeal, float duration)
    {
        int healed = 0;
        float interval = 0.1f;
        int steps = Mathf.CeilToInt(duration / interval);
        int healPerStep = Mathf.CeilToInt(totalHeal / (float)steps);

        while (healed < totalHeal)
        {
            int healAmount = Mathf.Min(healPerStep, totalHeal - healed);
            HealPlayer(healAmount); // Dòng thay đổi: HealPlayer sẽ tự cập nhật UI
            healed += healAmount;
            yield return new WaitForSeconds(interval);
        }
    }

    private void UpdateUI()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.gamePanel.SetMaxHealth(_maxHealth); // Dòng thay đổi: cập nhật maxHealth UI
            UIManager.Instance.gamePanel.UpdateHealth(_currentHealth); // Dòng thay đổi: cập nhật currentHealth UI
        }
    }
}
