
// UIPhaseHUD.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPhaseHUD : MonoBehaviour
{
    [SerializeField] private PhaseController phaseController;
    [SerializeField] private PhaseConfig config;
    [SerializeField] private TMP_Text phaseLabel;
    [SerializeField] private Image timeBar;
    [SerializeField] private Image banner;

    private void OnEnable()
    {
        phaseController.OnPhaseStarted += Started;
        phaseController.OnPhaseTick += Tick;
    }
    private void OnDisable()
    {
        phaseController.OnPhaseStarted -= Started;
        phaseController.OnPhaseTick -= Tick;
    }

    private void Started(PhaseType phase)
    {
        switch (phase)
        {
            case PhaseType.Safe: phaseLabel.text = "Phase 1: Safe"; banner.color = config.uiSafe; break;
            case PhaseType.Danger: phaseLabel.text = "Phase 2: Danger"; banner.color = config.uiDanger; break;
            case PhaseType.Disaster: phaseLabel.text = "Phase 3: Disaster"; banner.color = config.uiDisaster; break;
        }
        if (timeBar) timeBar.fillAmount = 1f;
    }

    private void Tick(PhaseType phase, float remaining)
    {
        float duration = phaseController.PhaseDuration;
        if (timeBar) timeBar.fillAmount = Mathf.Clamp01(remaining / duration);
    }
}