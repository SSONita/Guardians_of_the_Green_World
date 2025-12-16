using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private PhaseController phaseController;
    [SerializeField] private PhaseConfig config;
    [SerializeField] private AudioSource sfxSource; // second AudioSource for one-shots
    private AudioSource musicSource;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;
    }

    private void OnEnable()
    {
        phaseController.OnPhaseStarted += HandlePhaseStarted;
    }

    private void OnDisable()
    {
        phaseController.OnPhaseStarted -= HandlePhaseStarted;
    }

    private void HandlePhaseStarted(PhaseType phase)
    {
        AudioClip next = null;
        Debug.Log($"[AudioController] Phase started: {phase}");

        switch (phase)
        {
            case PhaseType.Safe:
                next = config.musicSafe;
                break;
            case PhaseType.Danger:
                next = config.musicDanger;
                PlayTransitionSfx();
                break;
            case PhaseType.Disaster:
                next = config.musicDisaster;
                PlayTransitionSfx();
                break;
        }

        // Simple switch (or implement crossfade)
        if (next != null)
        {
            StartCoroutine(CrossfadeMusic(next, 0.7f));
        }
    }

    private void PlayTransitionSfx()
    {
        var clip = config.sfxImpactTransition != null ? config.sfxImpactTransition : config.sfxAirTransition;
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, 0.9f);
        }
    }

    private System.Collections.IEnumerator CrossfadeMusic(AudioClip next, float time)
    {
        Debug.Log($"[AudioController] Crossfading to: {(next ? next.name : "null")} over {time}s");
        float startVol = musicSource.volume;
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, t / time);
            yield return null;
        }
        musicSource.Stop();
        musicSource.clip = next;
        musicSource.Play();

        t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVol, t / time);
            yield return null;
        }
        musicSource.volume = startVol;
    }
}
