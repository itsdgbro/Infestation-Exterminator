using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    [Header("Idle Audio")]
    [SerializeField] private AudioClip[] idleAudioClips;

    [Header("Attack Audio")]
    [SerializeField] private AudioClip[] attackAudioClips;
    private AudioSource audioSource;

    [Header("Hurt Audio")]
    [SerializeField] private AudioClip[] hurtAudioClips;

    private bool isIdling = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Idle Audio
    public void ZombieIdleAudio()
    {
        isIdling = true;
        StartCoroutine(PlayOnDelay(idleAudioClips));
    }

    // attack Audio
    public void ZombieAttackAudio()
    {
        StartCoroutine(PlayOnDelay(attackAudioClips));
    }

    public void ZombieHurt()
    {
        StartCoroutine(PlayOnDelay(hurtAudioClips));
    }

    IEnumerator PlayOnDelay(AudioClip[] clips)
    {
        if (isIdling)
            yield return new WaitForSeconds(Random.Range(0.0f, 2.0f));

        int rand = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[rand]);

        isIdling = false;
    }


}
