﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 moveAxis;
    public bool debugControl;
    public bool pressRight, pressLeft, pressUp, pressDown;
    public bool pressJump;

    private void Update()
    {
        if (!debugControl)
        {
            moveAxis.x = Input.GetAxisRaw("Horizontal_G");
            moveAxis.y = Input.GetAxisRaw("Vertical_G");
            pressJump = Input.GetButtonDown("A_G");
        }
        else
        {
            moveAxis = Vector2.zero;
            if (pressRight) moveAxis.x += 1;
            if (pressLeft) moveAxis.x -= 1;
            if (pressUp) moveAxis.y += 1;
            if (pressDown) moveAxis.y -= 1;
        }
    }
}