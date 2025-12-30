using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public int totalTrash = 10;       // Set total trash count in Inspector
    private int collectedTrash = 0;
    public float timeLimit = 120f;    // Example: 2 minutes
    private float timer;

    private bool polluterConfronted = false;
    private bool gameEnded = false;

    void Start()
    {
        timer = timeLimit;
    }

    void Update()
    {
        if (gameEnded) return;

        // Countdown timer
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            LoseGame();
        }
    }

    // Call this when trash is collected
    public void CollectTrash()
    {
        collectedTrash++;
    }

    // Call this when polluter is confronted
    public void ConfrontPolluter()
    {
        polluterConfronted = true;

        if (collectedTrash >= totalTrash)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    void WinGame()
    {
        gameEnded = true;
        SceneManager.LoadScene("SortScene"); // Replace with your SortScene name
    }

    void LoseGame()
    {
        gameEnded = true;
        SceneManager.LoadScene("ResultScene"); // Replace with your ResultScene name
    }
}
