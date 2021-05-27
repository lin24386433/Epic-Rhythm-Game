using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLine : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "BeatSoundTrigger")
        {
            audioSource.Play();
        }

    }
}
