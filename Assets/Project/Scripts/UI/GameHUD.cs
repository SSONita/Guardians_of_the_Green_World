using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text trashCounterText;
    public TMP_Text timerText;
    public TMP_Text phaseText;

    [Header("Gameplay Settings")]
    public int totalTrash = 0;
    private int collectedTrash = 0;
    public float gameDuration = 180f;
    private float timeRemaining;

    [Header("Polluter State")]
    public bool polluterConfronted = false;

    private bool gameEnded = false;

    // Singleton for easy access
    public static GameHUD Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        timeRemaining = gameDuration;
        UpdateTrashCounter();
        UpdateTimer();
        // Phase will be set by PhaseController via event
    }

    void Update()
    {
        if (gameEnded) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimer();
            CheckWinCondition();
        }
        else
        {
            LoseGame();
        }
    }

    // Called when polluter spawns trash
    public void RegisterNewTrash()
    {
        totalTrash++;
        Debug.Log($"[HUD] New trash spawned. Total = {totalTrash}");
        UpdateTrashCounter();
    }

    // Called when player collects trash
    public void AddTrash()
    {
        collectedTrash++;
        Debug.Log($"[HUD] Trash collected. {collectedTrash}/{totalTrash}");
        UpdateTrashCounter();
        CheckWinCondition();
    }

    // Called if trash despawns/destroyed without pickup
    public void RemoveTrash()
    {
        if (totalTrash > 0)
        {
            totalTrash--;
            Debug.Log($"[HUD] Trash removed. Total = {totalTrash}, Collected = {collectedTrash}");
            UpdateTrashCounter();
            CheckWinCondition();
        }
    }

    void UpdateTrashCounter()
    {
        if (trashCounterText != null)
            trashCounterText.text = $"Collected: {collectedTrash}/{totalTrash}";
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(Mathf.Max(timeRemaining, 0f) / 60f);
        int seconds = Mathf.FloorToInt(Mathf.Max(timeRemaining, 0f) % 60f);

        if (timerText != null)
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
    }

    // Called externally by PhaseController
    public void UpdatePhaseFromController(PhaseType phase)
    {
        if (phaseText == null) return;

        switch (phase)
        {
            case PhaseType.Safe:
                phaseText.text = "Phase: Safe";
                phaseText.color = Color.green;
                break;

            case PhaseType.Danger:
                phaseText.text = "Phase: Danger";
                phaseText.color = Color.yellow;
                break;

            case PhaseType.Disaster:
                phaseText.text = "Phase: Disaster";
                phaseText.color = Color.red;
                break;
        }

        Debug.Log("[HUD] Phase updated by controller: " + phase);
    }

    void CheckWinCondition()
    {
        if (gameEnded) return;

        if (collectedTrash >= totalTrash && polluterConfronted)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameEnded = true;
        Debug.Log("[HUD] Win! All trash collected and polluter confronted.");
        SceneManager.LoadScene("SortingScene");
    }

    void LoseGame()
    {
        gameEnded = true;
        Debug.Log("[HUD] Lose! Timer ran out.");
        SceneManager.LoadScene("ResultScene");
    }
}
