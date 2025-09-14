using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private CutsceneStarter cutsceneStarter;


    [Header("UI")]
    [SerializeField] private CanvasGroup menuGroup;
    [SerializeField] private float fadeDuration = 0.3f;

    private void Awake()
    {
        if (cutsceneStarter != null)
        {
            cutsceneStarter.onStarted.RemoveListener(HideMenuOnCutsceneStart);
            cutsceneStarter.onStarted.AddListener(HideMenuOnCutsceneStart);
            cutsceneStarter.onCompleted.RemoveListener(StartGameAfterCutscene);
            cutsceneStarter.onCompleted.AddListener(StartGameAfterCutscene);
        }
    }
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

    private void HideMenuOnCutsceneStart()
    {
        // Fade-out menu rồi tắt
        if (menuGroup != null)
            StartCoroutine(FadeOutThenDisable());
        else
            gameObject.SetActive(false);
    }

    private IEnumerator FadeOutThenDisable()
    {
        float t = 0f;
        menuGroup.interactable = false;
        menuGroup.blocksRaycasts = false;

        float start = menuGroup.alpha;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(start, 0f, t / fadeDuration);
            menuGroup.alpha = a;
            yield return null;
        }
        menuGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void StartGameAfterCutscene()
    {
        if (TheOnlyKingManager.HasInstance)
            TheOnlyKingManager.Instance.StartGame();
    }

}
