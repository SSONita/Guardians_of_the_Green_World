

using UnityEngine;

public class LightingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhaseController phaseController;
    [SerializeField] private LightingConfig lightingConfig;
    [SerializeField] private Light directionalLight; // your main sun light

    private void OnEnable()
    {
        if (phaseController != null)
            phaseController.OnPhaseStarted += HandlePhaseStarted;
    }

    private void OnDisable()
    {
        if (phaseController != null)
            phaseController.OnPhaseStarted -= HandlePhaseStarted;
    }

    private void HandlePhaseStarted(PhaseType phase)
    {
        switch (phase)
        {
            case PhaseType.Safe:
                ApplySkybox(lightingConfig.safeSkybox);
                ApplyDirectionalLight(lightingConfig.safeLightColor, lightingConfig.safeLightIntensity);
                ApplyAmbient(lightingConfig.safeAmbient);
                ApplyFog(lightingConfig.safeFogColor, lightingConfig.safeFogDensity);
                break;

            case PhaseType.Danger:
                ApplySkybox(lightingConfig.dangerSkybox);
                ApplyDirectionalLight(lightingConfig.dangerLightColor, lightingConfig.dangerLightIntensity);
                ApplyAmbient(lightingConfig.dangerAmbient);
                ApplyFog(lightingConfig.dangerFogColor, lightingConfig.dangerFogDensity);
                break;

            case PhaseType.Disaster:
                ApplySkybox(lightingConfig.disasterSkybox);
                ApplyDirectionalLight(lightingConfig.disasterLightColor, lightingConfig.disasterLightIntensity);
                ApplyAmbient(lightingConfig.disasterAmbient);
                ApplyFog(lightingConfig.disasterFogColor, lightingConfig.disasterFogDensity);
                break;
        }
    }

    private void ApplySkybox(Material skybox)
    {
        if (skybox != null) RenderSettings.skybox = skybox;
        // If using skybox exposure via shader property, you can adjust here too.
        DynamicGI.UpdateEnvironment(); // refresh reflection probes/ambient
    }

    private void ApplyDirectionalLight(Color color, float intensity)
    {
        if (!directionalLight) return;
        directionalLight.color = color;
        directionalLight.intensity = intensity;
        // Optional: rotate light for time-of-day effect
        // directionalLight.transform.rotation = Quaternion.Euler(x, y, z);
    }

    private void ApplyAmbient(Color ambient)
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = ambient;
    }

    private void ApplyFog(Color fogColor, float fogDensity)
    {
        RenderSettings.fog = lightingConfig.enableFog;
        if (!RenderSettings.fog) return;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = fogDensity;
    }
}
