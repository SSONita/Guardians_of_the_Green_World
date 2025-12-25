using UnityEngine;
using TMPro; // if using TextMeshPro

public class PopupTextManager : MonoBehaviour
{
    public static PopupTextManager Instance;

    public TextMeshProUGUI popupText;
    public float displayTime = 1.5f;

    private void Awake()
    {
        Instance = this;
        popupText.text = "";
    }

    public void ShowMessage(string message, Color color)
    {
        StopAllCoroutines();
        popupText.text = message;
        popupText.color = color;
        popupText.gameObject.SetActive(true);
        StartCoroutine(HideAfterDelay());
    }

    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        popupText.text = "";
        popupText.gameObject.SetActive(false);
    }
}
