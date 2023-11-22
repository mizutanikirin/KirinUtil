using System;
using System.Collections;
using System.Collections.Generic;
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

        public MinMax(float minValue, float maxValue)
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
}
