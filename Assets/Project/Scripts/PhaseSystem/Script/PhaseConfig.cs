using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhaseType { Safe, Danger, Disaster }

[CreateAssetMenu(menuName = "Game/Phase Config", fileName = "PhaseConfig")]
public class PhaseConfig : ScriptableObject
{
    [Header("Durations (seconds)")]
    public float safeDuration = 60f;
    public float dangerDuration = 60f;
    public float disasterDuration = 60f;

    [Header("Music Clips")]
    public AudioClip musicSafe;      // Nature Ambience (Pixabay) or Nature Sounds Pack (Unity)
    public AudioClip musicDanger;    // Tense Atmosphere (Pixabay)
    public AudioClip musicDisaster;  // Dramatic Cinematic / Eerie Ambience (Pixabay)

    [Header("Transition SFX (optional)")]
    public AudioClip sfxImpactTransition; // Impact Transition Dramatic Boom (Pixabay)
    public AudioClip sfxAirTransition;    // Air Transition (Pixabay)

    [Header("UI Colors (optional)")]
    public Color uiSafe = new Color(0.1f, 0.8f, 0.2f);
    public Color uiDanger = new Color(1f, 0.6f, 0.1f);
    public Color uiDisaster = new Color(1f, 0.2f, 0.2f);
}