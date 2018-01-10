using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    public static float MainHorizontal()
    {
        float r = 0f;
        r += Input.GetAxis("J_Horizontal");
        return Mathf.Clamp(r, -1, 1);
    }

    public static float MainVertical()
    {
        float r = 0f;
        r += Input.GetAxis("J_Vertical");
        return Mathf.Clamp(r, -1, 1);
    }

    public static Vector3 MainJoystick()
    {
        return new Vector3(MainHorizontal(), MainVertical(), 0);
    }

    public static float AltHorizontal()
    {
        float r = 0f;
        r += Input.GetAxis("J_Alt_Horizontal");
        return Mathf.Clamp(r, -1, 1);
    }

    public static float AltVertical()
    {
        float r = 0f;
        r += Input.GetAxis("J_Alt_Vertical");
        return Mathf.Clamp(r, -1, 1);
    }

    public static float DHorizontal()
    {
        float r = 0f;
        r += Input.GetAxis("J_DPad_Horizontal");
        return Mathf.Clamp(r, -1, 1);
    }

    public static float DVertical()
    {
        float r = 0f;
        r += Input.GetAxis("J_DPad_Vertical");
        return Mathf.Clamp(r, -1, 1);
    }

    public static bool DVerticalUpPressed(float vert)
    {
        bool pressed = false;

        if (DVertical() > 0 && vert == 0)
        {
            pressed = true;
        }
        else
        {
            pressed = false;
        }

        return pressed;
    }

    public static Vector3 AltJoystick()
    {
        return new Vector3(AltHorizontal(), AltVertical(), 0);
    }

    public static bool AButton()
    {
        return Input.GetButtonDown("A_Button");
    }

    public static bool BButton()
    {
        return Input.GetButtonDown("B_Button");
    }

    public static bool XButton()
    {
        return Input.GetButtonDown("X_Button");
    }

    public static bool YButton()
    {
        return Input.GetButtonDown("Y_Button");
    }

    public static bool RightTrigger()
    {
        return Input.GetAxis("J_Triggers") < 0;
    }

    public static bool LeftTrigger()
    {
        return Input.GetAxis("J_Triggers") > 0;
    }

    public static bool RightBumper()
    {
        return Input.GetButtonDown("RightBumper");
    }

    public static bool LeftBumper()
    {
        return Input.GetButtonDown("LeftBumper");
    }
}
