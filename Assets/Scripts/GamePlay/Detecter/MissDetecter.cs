using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissDetecter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
            Destroy(collision.gameObject);
        if (collision.CompareTag("LongNoteStart"))
        {
            collision.transform.parent.GetComponent<LongNote>().moving = false;
        }

    }
}
