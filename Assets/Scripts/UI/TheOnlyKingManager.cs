using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class TheOnlyKingManager : BaseManager<TheOnlyKingManager>
{
    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        
    }

    public void StartGame()
    {
        Debug.Log("ko vao duoc");
        Time.timeScale = 1;
        if (UIManager.HasInstance)
        {
            Debug.Log("vao game");
            UIManager.Instance.gamePanel.gameObject.SetActive(true);
            UIManager.Instance.menuPanel.gameObject.SetActive(false);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Home");

        if (UIManager.HasInstance)
        {
            UIManager.Instance.loosePanel.gameObject.SetActive(false);
        }
    }

    public void LosseGame()
    {
        Time.timeScale = 0;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.loosePanel.gameObject.SetActive(true);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
