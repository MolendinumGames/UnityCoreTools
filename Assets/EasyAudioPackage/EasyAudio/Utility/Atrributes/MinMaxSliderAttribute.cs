using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EasyAudioSystem.Utility
{
    [System.Serializable]
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        public float minValue;
        public float maxValue;
        public float minReset;
        public float maxReset;

        public MinMaxSliderAttribute(float minValue, float maxValue, float minReset, float maxReset)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.minReset = minReset;
            this.minValue = minValue;
        }
    }
}
