using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting
{
    public int volume;

    public int noteSpeed;

    public string[] keyCodes = new string[5];

    public PlayerSetting()
    {
        volume = 100;

        noteSpeed = 1;

        keyCodes = new string[5];
        keyCodes[0] = KeyCode.Y.ToString();
        keyCodes[1] = KeyCode.G.ToString();
        keyCodes[2] = KeyCode.B.ToString();
        keyCodes[3] = KeyCode.N.ToString();
        keyCodes[4] = KeyCode.J.ToString();
    }
}
