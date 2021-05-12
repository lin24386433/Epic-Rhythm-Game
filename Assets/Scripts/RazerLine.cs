using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerLine : MonoBehaviour
{
    [SerializeField]
    private AudioSource beatSound;

    bool canDestroy = false;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            canDestroy = true;
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            canDestroy = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Note")
        {
            if (canDestroy)
            {
                Destroy(collision.gameObject);
                beatSound.Play();
            }
            
        }
    }
}
