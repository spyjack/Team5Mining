using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehicleAudio
{
    [SerializeField]
    VehicleClass vehicle = null;
    [SerializeField]
    AudioSource drillAudio = null;
    [SerializeField]
    AudioSource wheelAudio = null;
    [SerializeField]
    AudioSource engineAudio = null;
    [SerializeField]
    AudioClip drillSound = null;

    [SerializeField]
    AnimationCurve distanceFalloff = new AnimationCurve();

    [SerializeField]
    float volumeMultiplier = 1;

    [SerializeField]
    float maxDistance = 5;

    public float Volume
    {
        get { return volumeMultiplier; }
    }

    public float MaxDistance
    {
        get { return maxDistance; }
    }

    public AnimationCurve Falloff
    {
        get { return distanceFalloff; }
    }

    public VehicleAudio(VehicleClass _vehicle)
    {
        vehicle = _vehicle;
    }

    public VehicleAudio()
    {

    }

    public void SetVolume(float volume)
    {
        volumeMultiplier = volume;

        if(drillAudio != null)
            drillAudio.volume = volumeMultiplier;

        if (engineAudio != null)
            engineAudio.volume = volumeMultiplier * 0.25f;

        if (wheelAudio != null)
            wheelAudio.volume = volumeMultiplier * 0.5f;
    }

    public void PlayAudio(VehicleSounds _sound)
    {
        if (_sound == VehicleSounds.Movement && !wheelAudio.isPlaying)
        {
            vehicle.GetPart(out PartWheel _wheel);
            wheelAudio.clip = _wheel.Sound;
            wheelAudio.Play();
        }
        else if(_sound == VehicleSounds.Drilling && !drillAudio.isPlaying)
        {
            drillAudio.clip = drillSound;
            drillAudio.Play();
        }
        else if (engineAudio != null && _sound == VehicleSounds.Engine && !engineAudio.isPlaying)
        {
            vehicle.GetPart(out PartEngine _engine);
            engineAudio.clip = _engine.Sound;
            engineAudio.Play();
        }
    }

    public void StopAudio(VehicleSounds _sound)
    {
        if (_sound == VehicleSounds.Movement && wheelAudio.isPlaying)
        {
            wheelAudio.Stop();
        }
        else if (_sound == VehicleSounds.Drilling && drillAudio.isPlaying)
        {
            drillAudio.Stop();
        }
        else if (engineAudio != null && _sound == VehicleSounds.Engine && engineAudio.isPlaying)
        {
            engineAudio.Stop();
        }
    }
}

public enum VehicleSounds
{
    Movement,
    Drilling,
    Engine
}
