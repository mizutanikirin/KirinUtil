using UnityEngine;
using System.Collections;

namespace KirinUtil {
    public static class ExtensionMethods {
        public static void SetAlpha(this Material material, float value) {
            Color color = material.color;
            color.a = value;
            material.color = color;
        }
    }
}
