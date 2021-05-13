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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canDestroy = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            canDestroy = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(GameController.instance.songPosInBeats);
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
