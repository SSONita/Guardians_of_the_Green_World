using System.Collections;
using UnityEngine;
using System;

public class PhaseController : MonoBehaviour
{
    [SerializeField] private PhaseConfig config;

    public PhaseType CurrentPhase { get; private set; } = PhaseType.Safe;
    public float PhaseElapsed { get; private set; }
    public float PhaseDuration { get; private set; }

    // Public events other systems can hook into
    public event Action<PhaseType> OnPhaseStarted;
    public event Action<PhaseType, float> OnPhaseTick; // (phase, remainingSeconds)
    public event Action<PhaseType> OnPhaseEnded;
    public event Action OnAllPhasesCompleted;

    private Coroutine routine;
    private bool running;
    private bool stopPhaseProgression = false; // 🚨 new flag

    private void OnEnable()
    {
        StartPhases();
        GameEvents.OnPolluterStopped += HandlePolluterStopped;

        // Subscribe HUD to phase events
        if (GameHUD.Instance != null)
        {
            OnPhaseStarted += GameHUD.Instance.UpdatePhaseFromController;
        }
    }

    private void OnDisable()
    {
        GameEvents.OnPolluterStopped -= HandlePolluterStopped;

        if (GameHUD.Instance != null)
        {
            OnPhaseStarted -= GameHUD.Instance.UpdatePhaseFromController;
        }
    }


    public void StartPhases()
    {
        if (running || config == null) return;
        running = true;
        routine = StartCoroutine(RunPhases());
    }

    public void StopPhases()
    {
        running = false;
        if (routine != null) StopCoroutine(routine);
        routine = null;
    }

    private IEnumerator RunPhases()
    {
        yield return RunPhase(PhaseType.Safe, config.safeDuration);

        // 🚨 If polluter stopped, break out early
        if (stopPhaseProgression) yield break;

        yield return RunPhase(PhaseType.Danger, config.dangerDuration);

        if (stopPhaseProgression) yield break;

        yield return RunPhase(PhaseType.Disaster, config.disasterDuration);

        if (!stopPhaseProgression)
        {
            OnAllPhasesCompleted?.Invoke();
            Debug.Log("[PhaseController] ALL PHASES COMPLETED");
        }

        running = false;
    }

    private IEnumerator RunPhase(PhaseType phase, float duration)
    {
        CurrentPhase = phase;
        PhaseDuration = duration;
        PhaseElapsed = 0f;

        Debug.Log($"[PhaseController] STARTED phase: {phase}, duration: {duration}s");
        OnPhaseStarted?.Invoke(phase);

        while (PhaseElapsed < duration)
        {
            yield return null;
            PhaseElapsed += Time.deltaTime;
            float remaining = Mathf.Max(0f, duration - PhaseElapsed);
            OnPhaseTick?.Invoke(phase, remaining);

            // Dev hotkey to skip phase during testing
            if (Input.GetKeyDown(KeyCode.P))
            {
                PhaseElapsed = PhaseDuration;
            }

            if (Mathf.Abs(remaining % 1f) < Time.deltaTime)
            {
                Debug.Log($"[PhaseController] TICK {phase}: {remaining:F0}s remaining");
            }
        }

        Debug.Log($"[PhaseController] ENDED phase: {phase}");
        OnPhaseEnded?.Invoke(phase);

        // 🚨 If polluter stopped, don’t continue to next phase
        if (stopPhaseProgression)
        {
            Debug.Log("[PhaseController] Phase progression stopped after confrontation.");
            yield break;
        }
    }

    private void HandlePolluterStopped()
    {
        stopPhaseProgression = true;
        Debug.Log("[PhaseController] Polluter confronted! Phase progression halted, timer continues.");
    }
}
