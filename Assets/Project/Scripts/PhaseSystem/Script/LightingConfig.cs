using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/Lighting Config", fileName = "LightingConfig")]
public class LightingConfig : ScriptableObject
{
    [Header("Common (optional)")]
    public Material safeSkybox;
    public Material dangerSkybox;
    public Material disasterSkybox;

    [Header("Directional Light settings")]
    public Color safeLightColor = new Color(1f, 0.95f, 0.85f); // warm
    public float safeLightIntensity = 1.2f;

    public Color dangerLightColor = new Color(0.85f, 0.85f, 1f); // cooler
    public float dangerLightIntensity = 0.8f;

    public Color disasterLightColor = new Color(0.7f, 0.75f, 0.9f); // very cool
    public float disasterLightIntensity = 0.4f;

    [Header("Ambient (RenderSettings)")]
    public Color safeAmbient = new Color(0.6f, 0.7f, 0.6f);
    public Color dangerAmbient = new Color(0.4f, 0.45f, 0.5f);
    public Color disasterAmbient = new Color(0.25f, 0.3f, 0.35f);

    [Header("Fog (RenderSettings)")]
    public bool enableFog = true;

    public Color safeFogColor = new Color(0.75f, 0.85f, 0.8f);
    public float safeFogDensity = 0.002f;

    public Color dangerFogColor = new Color(0.5f, 0.55f, 0.6f);
    public float dangerFogDensity = 0.01f;

    public Color disasterFogColor = new Color(0.2f, 0.22f, 0.25f);
    public float disasterFogDensity = 0.03f;

}