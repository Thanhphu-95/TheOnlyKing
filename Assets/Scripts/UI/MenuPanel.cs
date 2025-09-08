using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public void OnclickStarGame()
    {


        Debug.Log("ko bấm được");
        if (TheOnlyKingManager.HasInstance)
        {
            Debug.Log("đã bấm");
            TheOnlyKingManager.Instance.StartGame();
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
