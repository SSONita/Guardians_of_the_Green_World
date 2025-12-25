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
        // Retrieve stored results from SortingManager or GameManager
        int correct = PlayerPrefs.GetInt("CorrectlySorted", 0);
        int wrong = PlayerPrefs.GetInt("IncorrectlySorted", 0);
        int total = PlayerPrefs.GetInt("TotalItems", 0);
        bool win = PlayerPrefs.GetInt("Win", 0) == 1;

        // Update UI
        resultText.text = win ? "You Win! The world is saved!" : "You Lose! Pollution took over...";
        scoreText.text = $"Correct: {correct}\nWrong: {wrong}\nTotal: {total}";
    }

    public void RestartGame()
    {
        //SceneManager.LoadScene("MainGameplayScene"); // replace with your gameplay scene name
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}


