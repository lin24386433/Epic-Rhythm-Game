using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerLine : MonoBehaviour
{
    [SerializeField]
    private AudioSource beatSound;

    bool isPressed = false;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isPressed = false;
        }
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
