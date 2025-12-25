using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class SortingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text accuracyLabel;

    public void Refresh(int correct, int wrong, int total)
    {
        if (scoreLabel)
            scoreLabel.text = $"Correct: {correct}/{total} | Wrong: {wrong}";

        if (accuracyLabel)
        {
            float attempts = correct + wrong;
            float acc = attempts > 0 ? (correct / attempts) * 100f : 0f;
            accuracyLabel.text = $"Accuracy: {acc:0}%";
        }
    }
}
