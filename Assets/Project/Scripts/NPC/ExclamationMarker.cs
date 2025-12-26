using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExclamationMarker : MonoBehaviour
{
    public Image exclamationImage;
    public float showDuration = 2f;

    public void ShowOnce()
    {
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        exclamationImage.enabled = true;
        yield return new WaitForSeconds(showDuration);
        exclamationImage.enabled = false;
    }
}
