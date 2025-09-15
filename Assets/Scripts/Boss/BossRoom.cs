using Unity.Cinemachine;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField] private GameObject theBoss;
    [SerializeField] private Collider2D Wall01;
    [SerializeField] private Collider2D Wall02;
    [SerializeField] private GameObject bossCam;
    bool dead = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ddeens noi");
        if (collision.CompareTag("Player") && !dead)
        {
            Debug.Log("bossRoom");
            theBoss.SetActive(true);
            this.gameObject.SetActive(false);
            AudioManager.Instance.PlayBossBGM();

            if (UIManager.HasInstance)
            {
                UIManager.Instance.gamePanel.ActiveBossHealth(true);
            }

            Wall01.isTrigger = false;
            Wall02.isTrigger = false;

            bossCam.SetActive(true);
            
            
        }
    }

    public void OpenWall()
    {
        Wall01.isTrigger = true;
        Wall02.isTrigger = true;
        dead = true;


        bossCam.SetActive(false);

    }

}
