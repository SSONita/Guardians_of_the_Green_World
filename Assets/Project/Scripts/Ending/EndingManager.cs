using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        int correct = PlayerPrefs.GetInt("CorrectlySorted", 0);
        int wrong = PlayerPrefs.GetInt("IncorrectlySorted", 0);
        int total = PlayerPrefs.GetInt("TotalItems", 0);
        bool win = PlayerPrefs.GetInt("Win", 0) == 1;

        // Prefer recomputing accuracy from counts to ensure correctness
        int attempts = correct + wrong;
        float accuracy = attempts > 0 ? (float)correct / attempts * 100f : 0f;

        // Optional: clamp and format
        accuracy = Mathf.Clamp(accuracy, 0f, 100f);

        resultText.text = win ? "You Win! The world is saved!" : "You Lose! Pollution took over...";
        scoreText.text = $"Accuracy: {accuracy:F1}%   Mistakes: {wrong}   Total Trash: {total}";
    }


    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay"); // replace with your gameplay scene name
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}


