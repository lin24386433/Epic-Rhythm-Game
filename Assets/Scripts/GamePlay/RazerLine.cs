using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerLine : MonoBehaviour
{ 
    private AudioSource beatSound;

    [Space(10)]
    [SerializeField]
    private KeyCode keyToPress;

    [SerializeField]
    private KeyCode keyToPress2;

    private bool isPressed = false;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(keyToPress) && Input.GetKey(keyToPress2))
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
        if(collision.tag == "LongNoteStart")
        {
            if (isPressed && collision.transform.GetComponentInParent<LongNote>().moving)
            {
                collision.transform.GetComponentInParent<LongNote>().moving = false;
                beatSound.Play();
            }
        }
        if(collision.tag == "LongNoteEnd")
        {
            if (!isPressed && collision.transform.GetComponentInParent<LongNote>().moving == false)
            {
                Destroy(collision.transform.parent.gameObject);
                beatSound.Play();
            }
        }
        /*
        if (!isPressed && collision.transform.GetComponentInParent<LongNote>().moving == false)
        {
            Destroy(collision.transform.parent.gameObject);
        }
        */
    }
}
