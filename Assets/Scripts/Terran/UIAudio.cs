using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [SerializeField]
    AudioSource source = null;

    [SerializeField]
    AudioClip upgradeClip = null;
    [SerializeField]
    AudioClip purchaseClip = null;

    public void PlaySound(UIsounds _sound)
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        if (_sound == UIsounds.Upgrade)
        {
            source.PlayOneShot(upgradeClip);
        }else if (_sound == UIsounds.Purchase)
        {
            source.PlayOneShot(purchaseClip);
        }
    }
}

public enum UIsounds
{
    Upgrade,
    Purchase,
    Button
}
