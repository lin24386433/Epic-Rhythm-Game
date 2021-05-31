using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem big;

    [SerializeField]
    private ParticleSystem small;

    public GameObject parent;

    public float rotation;

    private void Start()
    {
        parent = this.transform.parent.gameObject;

        float z = 360-parent.transform.eulerAngles.z;

        rotation = z;

        var main = big.main;

        main.startRotation = z * Mathf.PI / 180f;

        

        main = small.main;

        main.startRotation = z * Mathf.PI / 180f;


    }


}
