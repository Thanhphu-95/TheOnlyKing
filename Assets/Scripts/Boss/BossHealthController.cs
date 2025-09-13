using UnityEngine;

public class BossHealthController : MonoBehaviour
{
    [SerializeField] private BossRoom bossRoom;
    private int currenHealth;

    public int CurrenHealth => currenHealth;
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;

    void Start()
    {
        currenHealth = maxHealth;

        if (UIManager.HasInstance)
        {
            UIManager.Instance.gamePanel.SetBossMaxHealth(maxHealth);
            UIManager.Instance.gamePanel.UpdateBossHealth(currenHealth);
        }
    }

    // Update is called once per frame
    public void TakeDamage(int damageAmount)
    {
        currenHealth -= damageAmount;

        if (UIManager.HasInstance)
        {
            UIManager.Instance.gamePanel.UpdateBossHealth(currenHealth);
        }

        if (currenHealth <= 0)
        {   Debug.Log("ko thay");
            //var bossRoom = FindAnyObjectByType<BossRoom>();
            //if (bossRoom != null)
            //{

            //    bossRoom.OpenWall();
            //}

            bossRoom.OpenWall();
            if (UIManager.HasInstance)
            {
                UIManager.Instance.gamePanel.ActiveBossHealth(false);
            }
            Destroy(this.gameObject);

        }
    }
}
