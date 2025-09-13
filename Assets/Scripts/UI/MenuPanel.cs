using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private CutsceneStarter cutsceneStarter;
    public void OnclickStarGame()
    {
        Debug.Log("ko bấm được");
        if (TheOnlyKingManager.HasInstance)
        {
            Debug.Log("đã bấm");
            TheOnlyKingManager.Instance.StartGame();
            cutsceneStarter.PlayCutscene();
        }
    }
    public void OnClickQuitGame()
    {
        if (TheOnlyKingManager.HasInstance)
        {
            TheOnlyKingManager.Instance.QuitGame();
        }
    }

}
