using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AmbienceController : MonoBehaviour
{
    [SerializeField]
    AudioSource ambienceSourceMain = null;
    [SerializeField]
    AudioSource ambienceSourceSecondary = null;
    [SerializeField]
    AudioSource extraSource = null;

    [SerializeField]
    List<AudioClip> ambienceTracks = new List<AudioClip>();

    [SerializeField]
    float fadeInDelay = 0.1f;

    [SerializeField]
    float fadeInIncrement = 0.1f;

    bool fadedIn = false;
    AmbienceTrack playingTrack = AmbienceTrack.None;
    AmbienceTrack fadingTrack = AmbienceTrack.None;

    [SerializeField]
    AudioMixer mixer = null;

    [SerializeField]
    Slider masterSlider = null;

    [SerializeField]
    Slider effectSlider = null;

    [SerializeField]
    Slider musicSlider = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("masterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetEffectVolume(float sliderValue)
    {
        mixer.SetFloat("effectVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetAmbienceVolume(float sliderValue)
    {
        mixer.SetFloat("ambienceVol", Mathf.Log10(sliderValue) * 20);
    }

    public void PlayTrack(AudioClip _track)
    {
        extraSource.PlayOneShot(_track);
    }

    public void FadeInAmbience(AmbienceTrack _track)
    {
        if (fadingTrack != _track)
        {
            switch (_track)
            {
                case AmbienceTrack.Surface:
                    fadedIn = false;
                    fadingTrack = _track;
                    StartCoroutine(FadeInSurfaceTrack());
                    break;
                case AmbienceTrack.UpperLayers:
                    fadedIn = false;
                    fadingTrack = _track;
                    StartCoroutine(FadeInUndergroundTrack());
                    break;
                case AmbienceTrack.DarkCaves:
                    fadedIn = false;
                    fadingTrack = _track;
                    StartCoroutine(FadeInDeepCavesTrack());
                    break;
            }
        }
    }

    IEnumerator FadeInSurfaceTrack()
    {
        if (fadedIn || playingTrack == AmbienceTrack.Surface)
            yield break;

        AudioSource trackPlayer = ambienceSourceMain;
        AudioSource altTrackPlayer = ambienceSourceSecondary;
        if (ambienceSourceMain.isPlaying)
        {
            altTrackPlayer = ambienceSourceMain;
            trackPlayer = ambienceSourceSecondary;
        }

        trackPlayer.clip = ambienceTracks[0];
        trackPlayer.Play();

        while (trackPlayer.volume < 1)
        {
            trackPlayer.volume += fadeInIncrement;
            altTrackPlayer.volume -= fadeInIncrement;
            yield return new WaitForSeconds(fadeInDelay);
        }
        trackPlayer.volume = 1;
        altTrackPlayer.volume = 0;
        fadedIn = true;
        altTrackPlayer.Stop();
        playingTrack = AmbienceTrack.Surface;
        fadingTrack = AmbienceTrack.None;
    }

    IEnumerator FadeInUndergroundTrack()
    {
        if (fadedIn || playingTrack == AmbienceTrack.UpperLayers)
            yield break;

        AudioSource trackPlayer = ambienceSourceMain;
        AudioSource altTrackPlayer = ambienceSourceSecondary;
        if (ambienceSourceMain.isPlaying)
        {
            altTrackPlayer = ambienceSourceMain;
            trackPlayer = ambienceSourceSecondary;
        }

        trackPlayer.clip = ambienceTracks[1];
        trackPlayer.Play();

        while (trackPlayer.volume < 1)
        {
            trackPlayer.volume += fadeInIncrement;
            altTrackPlayer.volume -= fadeInIncrement;
            yield return new WaitForSeconds(fadeInDelay);
        }
        trackPlayer.volume = 1;
        altTrackPlayer.volume = 0;
        fadedIn = true;
        altTrackPlayer.Stop();
        playingTrack = AmbienceTrack.UpperLayers;
        fadingTrack = AmbienceTrack.None;
    }

    IEnumerator FadeInDeepCavesTrack()
    {
        if (fadedIn || playingTrack == AmbienceTrack.DarkCaves)
            yield break;

        AudioSource trackPlayer = ambienceSourceMain;
        AudioSource altTrackPlayer = ambienceSourceSecondary;
        if (ambienceSourceMain.isPlaying)
        {
            altTrackPlayer = ambienceSourceMain;
            trackPlayer = ambienceSourceSecondary;
        }

        trackPlayer.clip = ambienceTracks[2];
        trackPlayer.Play();

        while (trackPlayer.volume < 1)
        {
            trackPlayer.volume += fadeInIncrement;
            altTrackPlayer.volume -= fadeInIncrement;
            yield return new WaitForSeconds(fadeInDelay);
        }
        trackPlayer.volume = 1;
        altTrackPlayer.volume = 0;
        fadedIn = true;
        altTrackPlayer.Stop();
        playingTrack = AmbienceTrack.DarkCaves;
        fadingTrack = AmbienceTrack.None;
    }
}

public enum AmbienceTrack
{
    None,
    Surface,
    UpperLayers,
    DarkCaves
}
