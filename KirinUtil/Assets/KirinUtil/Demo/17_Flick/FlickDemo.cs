using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil.Demo
{
    public class FlickDemo : MonoBehaviour
    {
        public void Flicked(Vector2 centerPos, float startPosX, float endPosX)
        {
            print("[FLICK] center: " + centerPos + ", start: " + startPosX + ", end: " + endPosX);
        }
    }
}
