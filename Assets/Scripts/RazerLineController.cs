using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerLineController : MonoBehaviour
{
    [SerializeField]
    GameObject razer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            razer.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            razer.SetActive(false);
        }
    }
}
