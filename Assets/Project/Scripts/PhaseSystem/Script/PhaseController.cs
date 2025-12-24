using System.Collections;
using System.Collections.Generic;
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

    private void OnEnable()
    {
        // Auto-start for now (you'll move this to GameManager later)
        StartPhases();
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
        yield return RunPhase(PhaseType.Danger, config.dangerDuration);
        yield return RunPhase(PhaseType.Disaster, config.disasterDuration);


        OnAllPhasesCompleted?.Invoke();
        running = false;
    }

    private IEnumerator RunPhase(PhaseType phase, float duration)
    {
        CurrentPhase = phase;
        PhaseDuration = duration;
        PhaseElapsed = 0f;
        // Inside RunPhase() at the start:
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


        // After all phases finish (in RunPhases()):
        Debug.Log("[PhaseController] ALL PHASES COMPLETED");

    }
}
