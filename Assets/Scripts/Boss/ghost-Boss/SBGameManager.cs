using UnityEngine;
using UnityEngine.SceneManagement;

public class SBGameManager : MonoBehaviour
{
    [SerializeField]
    private float delayLoadScene = 1f;
    [SerializeField]
    private string sceneName = "Home";

    public void ReloadScene()
    {
        Invoke(nameof(DelayLoadScene), delayLoadScene);
    }

    private void DelayLoadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
