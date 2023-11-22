using UnityEngine;
using System.Collections;

// https://q7z.hatenablog.com/entry/2016/07/21/204028
public class MinMaxSliderAttribute : PropertyAttribute
{
    public readonly float max;
    public readonly float min;

    public MinMaxSliderAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}