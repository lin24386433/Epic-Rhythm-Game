using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetecter : MonoBehaviour
{
    [SerializeField]
    private AudioSource beatSound;

    [SerializeField]
    private KeyCode keyToPress;

    //
    bool canDestroy = false;

    GameObject obj;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(canDestroy)
        {
            if (Input.GetKeyDown(keyToPress))
            {
                Destroy(obj);
                beatSound.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
            canDestroy = true;
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if (obj == null)
                obj = collision.gameObject;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
            canDestroy = false;
    }
}
