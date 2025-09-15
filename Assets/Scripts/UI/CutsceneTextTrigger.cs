using UnityEngine;
using TMPro;

public class CutsceneTextTrigger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cutsceneText;

    private void Start()
    {
        cutsceneText.gameObject.SetActive(false); // tắt sẵn
    }

    public void ShowText(string message)
    {
        cutsceneText.text = message;
        cutsceneText.gameObject.SetActive(true);
    }

    public void HideText()
    {
        cutsceneText.gameObject.SetActive(false);
    }
}
