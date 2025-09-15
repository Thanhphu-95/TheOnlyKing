using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class BossHealthController : MonoBehaviour
{
    [SerializeField] private BossRoom bossRoom;
    private int currenHealth;

    public int CurrenHealth => currenHealth;
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;


    [Header("End Scene Settings")]
    [SerializeField] private PlayableDirector endSceneDirector; // gắn endscene Timeline ở Inspector
    [SerializeField] private bool loadSceneAfterCutscene = false;
    [SerializeField] private string nextSceneName; // nếu muốn load scene sau cutscene

    void Start()
    {
        currenHealth = maxHealth;

        if (UIManager.HasInstance)
        {
            UIManager.Instance.gamePanel.SetBossMaxHealth(maxHealth);
            UIManager.Instance.gamePanel.UpdateBossHealth(currenHealth);
        }

        if (endSceneDirector != null)
        {
            endSceneDirector.stopped += OnCutsceneStopped;
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

            
            if (endSceneDirector != null)
            {
                
                endSceneDirector.Play();

                if (loadSceneAfterCutscene)
                {
                    endSceneDirector.Play();
                }
            }
            else
            {
                Debug.Log("ko vao endscend");
            }


        }
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        if (loadSceneAfterCutscene && !string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }

        // Sau khi cutscene xong mới destroy boss
        Destroy(this.gameObject);
    }

}
