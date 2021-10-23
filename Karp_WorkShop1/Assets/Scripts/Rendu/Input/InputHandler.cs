using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class InputHandler : MonoBehaviour
{
    [Header("Input Map"), Expandable]
    public InputMap_SCO inputMap;

    [Header("Input Events")]
    public UnityEvent onJump;

    public Vector2 StickDir
    {
        get
        {
            return inputMap.StickDir;
        }
    }

    public bool isPlanning
    {
        get
        {
            return inputMap.isJump;
        }
    }
    public bool Reset
    {
        get
        {
            return inputMap.pressR;
        }
    }
    private void Update()
    {
        if (inputMap.isJumpBegin)
        {
            onJump?.Invoke();
        }
    }
}
