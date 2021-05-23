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

    //
    bool canDestroy = false;

    public GameObject obj;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (canDestroy)
        {
            if (Input.GetKeyDown(keyToPress) && Input.GetKey(keyToPress2) || Input.GetKey(keyToPress) && Input.GetKeyDown(keyToPress2))
            {
                if (obj == null)
                    return;
                if (obj.CompareTag("Note"))
                {
                    Destroy(obj);
                    beatSound.Play();
                }
                else if (obj.CompareTag("LongNoteStart"))
                {
                    if (obj.transform.GetComponentInParent<LongNote>().moving)
                    {
                        obj.transform.GetComponentInParent<LongNote>().moving = false;
                        beatSound.Play();
                    }
                }
            }

            if (Input.GetKeyUp(keyToPress) && Input.GetKey(keyToPress2) || Input.GetKey(keyToPress) && Input.GetKeyUp(keyToPress2) || Input.GetKeyUp(keyToPress) && Input.GetKeyUp(keyToPress2))
            {
                if (obj == null)
                    return;
                if (obj.CompareTag("LongNoteEnd"))
                {
                    Destroy(obj.transform.parent.gameObject);
                    beatSound.Play();
                }
                else
                {
                    if (obj.transform.parent != null)
                    {
                        Destroy(obj.transform.parent.gameObject);
                    }
                        
                }
            }

        }   

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            canDestroy = true;
        }
        if (collision.CompareTag("LongNoteStart"))
        {
            canDestroy = true;
        }
        if (collision.CompareTag("LongNoteEnd"))
        {
            canDestroy = true;
            obj = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if(obj == null)
                obj = collision.gameObject;
        }
        if(collision.CompareTag("LongNoteStart"))
        {
            if (obj == null)
                obj = collision.gameObject;
        }
        if(collision.CompareTag("LongNoteEnd"))
        {
            if (obj == null)
                obj = collision.gameObject;

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            canDestroy = false;
            obj = null;
        }
        if (collision.CompareTag("LongNoteStart"))
        {
            canDestroy = false;
            obj = null;
        }
        if (collision.CompareTag("LongNoteEnd"))
        {
            canDestroy = false;
            obj = null;
        }
    }

}
