using UnityEngine;

public class LoosePanel : MonoBehaviour
{
    public void OnClickQuitGame()
    {
        if (TheOnlyKingManager.HasInstance)
        {
            TheOnlyKingManager.Instance.QuitGame();
        }
    }

    public void OnClickRestart()
    {
        if (TheOnlyKingManager.HasInstance)
        {
            TheOnlyKingManager.Instance.RestartGame();
        }
    }
}
