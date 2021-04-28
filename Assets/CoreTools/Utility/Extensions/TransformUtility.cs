using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtility
{
    public static void ResetPosition(this Transform t) => t.position = Vector3.zero;
    public static void ResetLocalPosition(this Transform t) => t.localPosition = Vector3.zero;

    public static void ResetRotation(this Transform t) => t.rotation = Quaternion.identity;
    public static void ResetLocalRotation(this Transform t) => t.localRotation = Quaternion.identity;

    public static void ResetScale(this Transform t) => t.localScale = Vector3.one;
    public static void ResetLocalScale(this Transform t) => t.localScale = Vector3.one;

    public static void SwitchPosition(this Transform t, Transform target)
    {
        var pos1 = t.position;
        var pos2 = target.position;
        t.position = pos2;
        target.position = pos1;
    }
}
