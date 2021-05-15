using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recorder : MonoBehaviour
{
    [SerializeField]
    private Text beatNowTxt;

    private void Update()
    {
        beatNowTxt.text = RecordConductor.instance.songPosInBeats.ToString("0.00");
    }
}
