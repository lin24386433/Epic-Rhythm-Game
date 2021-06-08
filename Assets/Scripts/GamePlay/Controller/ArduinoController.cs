using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{

    public SerialPort sp = new SerialPort("COM4", 9600);


    public float inputNow;

    public static bool[] isPressed = new bool[5];

    public static bool[] isPressing = new bool[5];

    public static bool[] buttonDown = new bool[5];

    public static bool[] buttonHold = new bool[5];

    public static bool[] buttonUp = new bool[5];

    // Start is called before the first frame update
    void Start()
    {
        isPressed = new bool[5];

        isPressing = new bool[5];

        sp.Open();
        //sp.ReadTimeout = 100;
    }

    // Update is called once per frame
    void Update()
    {
        inputNow = float.Parse(sp.ReadLine());
        ButtonCheck(0);
        ButtonCheck(1);
        ButtonCheck(2);
        ButtonCheck(3);
        ButtonCheck(4);


    }

    void ButtonCheck(int index)
    {
        if (inputNow == index * 2 + 1)
        {
            isPressing[index] = true;
        }

        if (inputNow == index * 2)
        {
            isPressing[index] = false;
        }

        if (!isPressed[index] && isPressing[index])
        {
            buttonDown[index] = true;
        }
        else
        {
            buttonDown[index] = false;
        }

        if (isPressed[index] && !isPressing[index])
        {
            buttonUp[index] = true;
        }
        else
        {
            buttonUp[index] = false;
        }

        if (isPressing[index])
        {
            buttonHold[index] = true;
        }
        else
        {
            buttonHold[index] = false;
        }

        isPressed[index] = isPressing[index];
    }

    public static bool GetButtonDown(int index)
    {
        return buttonDown[index];
    }

    public static bool GetButtonHold(int index)
    {
        return buttonHold[index];
    }

    public static bool GetButtonUp(int index)
    {
        return buttonUp[index];
    }
}
