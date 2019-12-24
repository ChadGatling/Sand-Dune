using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour {
    private static float leverAngle;
    private static bool handsOnly;

    public virtual bool GetHandsOnly()
    {
        return handsOnly;
    }

    public virtual void SetHandsOnly(bool value)
    {
        handsOnly = value;
    }

    public static float GetLeverAngle()
    {
        return leverAngle;
    }

    public static void SetLeverAngle(float value)
    {
        leverAngle = value;
    }
}
