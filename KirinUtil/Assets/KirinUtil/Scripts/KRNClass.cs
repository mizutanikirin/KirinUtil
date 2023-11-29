using System;
using UnityEngine;

namespace KirinUtil
{
    [Serializable]
    public class OrderData
    {
        public string id;
        public float value;
    }

    [Serializable]
    public class MinMax
    {
        public float min;
        public float max;

        public MinMax(float minValue=0, float maxValue=0)
        {
            min = minValue;
            max = maxValue;
        }
    }

    [Serializable]
    public class SizeDelta
    {
        public float width;
        public float height;

        public SizeDelta(float widthValue, float heightValue)
        {
            width = widthValue;
            height = heightValue;
        }
    }


    [Serializable]
    public class TMPData
    {
        public string message;

        public Vector3 textPos;
        public float width;
        public MinMax fontSize;
        public Color fontColor;

        public bool outlineEnable;
        public Color outlineColor;
        public float outlineThikness;
        
        public Align align;
    }

    public enum Align
    {
        Left, Center, Right
    }
}
