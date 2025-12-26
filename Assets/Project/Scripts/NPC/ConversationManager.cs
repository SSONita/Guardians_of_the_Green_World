using UnityEngine;
using TMPro;

public class ConversationManager : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text text;

    public void ShowConversation(string message)
    {
        panel.SetActive(true);
        text.text = message;
    }

    public void HideConversation()
    {
        panel.SetActive(false);
    }
}
