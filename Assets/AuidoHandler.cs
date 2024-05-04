using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuidoHandler : MonoBehaviour
{
    private AudioSource audioSource;

    public static AuidoHandler Instance { get; private set; }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
