using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneEndHandler : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector director;

    [Header("Next Scene")]
    public string gameplaySceneName = "Gameplay"; // Set your exact gameplay scene name

    private void OnEnable()
    {
        if (director != null)
            director.stopped += OnCutsceneEnd;
    }

    private void OnDisable()
    {
        if (director != null)
            director.stopped -= OnCutsceneEnd;
    }

    private void OnCutsceneEnd(PlayableDirector d)
    {
        // Optional: debounce in case of double-fire
        director.stopped -= OnCutsceneEnd;
        SceneManager.LoadScene(gameplaySceneName);
    }
}
