using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetecter : MonoBehaviour
{
    [SerializeField]
    private AudioSource beatSound;

    [SerializeField]
    private KeyCode keyToPress;

    private bool isPressed = false;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(keyToPress))
        {
            isPressed = true;
        }
        else
        {
            isPressed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(Conductor.instance.songPosInBeats);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Note")
        {
            if (isPressed)
            {
                Destroy(collision.gameObject);
                beatSound.Play();
            }

        }
    }
}
