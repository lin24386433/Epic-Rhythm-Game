using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerLineController : MonoBehaviour
{
    [SerializeField]
    GameObject razer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            razer.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            razer.SetActive(false);
        }
    }
}
