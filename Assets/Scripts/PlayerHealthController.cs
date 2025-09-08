using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int currentHealth; //mau hien tai

    [SerializeField] private int maxHealth; //mau toi da

    private float invicCouter; // luu va dem thoi gian bat tu.
    [SerializeField] private GameObject PlayerDeadEff;
    [SerializeField] private Animator Animator;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject priest;

    [SerializeField]
    private SpriteRenderer[] playerSprites;

    [Header("nhận sát thương")]
    [SerializeField]
    private float invicibilityTime;
    private float invicCounter;
    [SerializeField]
    private float flashTime;
    private float flashCounter;


    private int HurtParamPr = Animator.StringToHash("HurtPr");

    private void Start()
    {
        currentHealth = maxHealth;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.gamePanel.SetMaxHealth(maxHealth);
            UIManager.Instance.gamePanel.UpdateHealth(currentHealth);
        }
    }
    void Update()
    {
        if (invicCounter > 0)
        {
            invicCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;

            if (flashCounter <= 0)
            {
                foreach (SpriteRenderer sprite in playerSprites)
                {
                    sprite.enabled = !sprite.enabled;
                }

                flashCounter = flashTime;
            }

            if (invicCounter <= 0)
            {
                foreach (SpriteRenderer sprite in playerSprites)
                {
                    sprite.enabled = true;
                }
                flashCounter = 0;
            }
        }
    }


    public void HealPlayer(int healthAmount)//moi mau
    {
        currentHealth += healthAmount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (UIManager.HasInstance)
        {
            UIManager.Instance.gamePanel.UpdateHealth(currentHealth);
        }
    }

    public void IncreaseHeal(int Heal)//tang mau toi da
    {
        maxHealth += Heal;
    }

    public void DamagePlayer(int damageAmount)// nhan sat thuong
    {
        if (invicCounter <= 0)
        {
            Debug.Log($"sat thuong = {damageAmount}");
            currentHealth -= damageAmount;
            if (UIManager.HasInstance)
            {
                UIManager.Instance.gamePanel.UpdateHealth(currentHealth);
                if (priest.activeSelf && !player.activeSelf)
                {
                    Animator.SetTrigger(HurtParamPr);
                }


            }
            if (currentHealth <= 0)
            {
                Debug.Log("Die");
                if (TheOnlyKingManager.HasInstance)
                {
                    TheOnlyKingManager.Instance.LosseGame();
                }

            }
            else
            {
                invicCounter = invicibilityTime;
            }
        }
    }

    private void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }
}
