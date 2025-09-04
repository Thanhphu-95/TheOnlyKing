using TMPro;
using UnityEngine;

public class UnlockBoxMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TextMeshPro;

    public void SetBoxMessage(string message)
    {
        m_TextMeshPro.text = message;
    }
}
