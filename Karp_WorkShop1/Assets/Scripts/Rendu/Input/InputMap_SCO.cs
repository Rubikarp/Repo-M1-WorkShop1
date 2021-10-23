using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

[CreateAssetMenu(fileName = "InputMap_new", menuName = "SCO/InputMap", order = 1)]
public class InputMap_SCO : ScriptableObject
{
    public bool inputSyst = true;
    public bool brutKey
    {
        get{ return !inputSyst;}
    }

    [Header("Old Input Sytem")]
    [Header("Axis")]
    [InputAxis,ShowIf("inputSyst")] public string axisHori = "Horizontal";
    [InputAxis, ShowIf("inputSyst")] public string axisVerti = "Vertical";
    [Header("Button")]
    [InputAxis, ShowIf("inputSyst")] public string buttonJump = "Jump";
    [InputAxis, ShowIf("inputSyst")] public string buttonReset = "Reset";

    [Header("Brut Key Sytem")]
    [Header("Axis")]
    [ShowIf("brutKey")]public KeyCode keyForward = KeyCode.Z;
    [ShowIf("brutKey")] public KeyCode keyBackward = KeyCode.S, keyRight = KeyCode.D, keyLeft = KeyCode.Q;
    [Header("Button")]
    [ShowIf("brutKey")] public KeyCode keyJump = KeyCode.Space;
    [ShowIf("brutKey")] public KeyCode keyReset = KeyCode.R;

    public Vector2 StickDir
    {
        get
        {
            if (brutKey)
            {
                float x = Convert.ToInt32(Input.GetKey(keyRight)) - Convert.ToInt32(Input.GetKey(keyLeft));
                float y = Convert.ToInt32(Input.GetKey(keyForward)) - Convert.ToInt32(Input.GetKey(keyBackward));
                return new Vector2(x,y);
            }
            else
            {
                return new Vector2(Input.GetAxis(axisHori), Input.GetAxis(axisVerti)).normalized;
            }
        }
    }
    public bool isJumpBegin
    {
        get
        {
            if (brutKey)
            {
                return Input.GetKeyDown(keyJump);
            }
            else
            {
                return Input.GetButtonDown(buttonJump);
            }
        }
    }
    public bool isJump
    {
        get
        {
            if (brutKey)
            {
                return Input.GetKey(keyJump);
            }
            else
            {
                return Input.GetButton(buttonJump);
            }
        }
    }
    public bool pressR
    {
        get
        {
            if (brutKey)
            {
                return Input.GetKey(keyReset);
            }
            else
            {
                return Input.GetButton(buttonReset);
            }
        }

    }

}