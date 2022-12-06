using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject tempAudioSourcePrefab;
    [Header("Audio clips")]
    [SerializeField] AudioClip audioLightOut;
    [SerializeField] AudioClip audioLevelComplete;

    public static AudioManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void PlayAudioEffect(AudioClip clip)
    {
        if (clip == null)
            return;

        AudioSource newSource = Instantiate(tempAudioSourcePrefab).gameObject.GetComponent<AudioSource>();
        newSource.clip = clip;
        newSource.Play();
    }

    public void PlayLightOut()
    {
        PlayAudioEffect(audioLightOut);
    }

    public void PlayLevelCompleted()
    {
        PlayAudioEffect(audioLevelComplete);
    }
}
