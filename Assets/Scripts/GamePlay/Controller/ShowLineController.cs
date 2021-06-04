using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLineController : MonoBehaviour
{
    [SerializeField]
    private GameObject showLine;

    [Space(10)]
    [SerializeField]
    public KeyCode keyToPress;

    [SerializeField]
    public KeyCode keyToPress2;

    private void Start()
    {
        keyToPress = GetComponent<RazerLine>().keyToPress;
        keyToPress2 = GetComponent<RazerLine>().keyToPress2;
    }

    private void Update()
    {

        if (Input.GetKey(keyToPress) && Input.GetKey(keyToPress2))
        {
            showLine.SetActive(true);
        }
        else
        {
            showLine.SetActive(false);
        }
    }
}
