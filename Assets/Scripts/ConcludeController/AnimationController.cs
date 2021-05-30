using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject gameController;

    private void Start()
    {
        Invoke("StartGameController", 0.75f);
    }

    private void StartGameController()
    {
        gameController.SetActive(true);
    }
}
