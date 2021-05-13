using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : MonoBehaviour
{
    public GameObject startNote;

    public GameObject endNote;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position = (startNote.transform.position + endNote.transform.position) / 2;
        transform.localScale = new Vector3(transform.localScale.x, endNote.transform.position.y - startNote.transform.position.y, transform.localScale.y);
    }
}
