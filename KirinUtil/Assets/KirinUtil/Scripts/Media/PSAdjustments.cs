using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil
{
    public class PSAdjustments : MonoBehaviour
    {

        [SerializeField] private Material adjustmentsMaterial;

        [Serializable]
        public class Setting
        {
            [Range(0f, 2f)] public float contrast = 1f;
            [Range(0f, 3f)] public float saturation = 1f;
            [Range(-1f, 1f)] public float brightness = 0;
            [Range(0f, 1f)] public float hue = 0;
            public bool invertColors = false;
            public bool binarize = false;
            [Range(0, 256)] public float posterizationLevels = 0;
        }

        public Texture Convert(Texture texture, Setting set)
        {
            texture = Adjustments(texture, set);
            return texture;
        }

        private Texture Adjustments(Texture texture, Setting set)
        {
            RenderTexture destination = new RenderTexture(texture.width, texture.height, 24);
            adjustmentsMaterial.SetTexture("_MainTex", texture);
            adjustmentsMaterial.SetFloat("_Contrast", set.contrast);
            adjustmentsMaterial.SetFloat("_Saturation", set.saturation);
            adjustmentsMaterial.SetFloat("_Brightness", set.brightness);
            adjustmentsMaterial.SetFloat("_Hue", set.hue);
            if (set.invertColors)
                adjustmentsMaterial.SetFloat("_InvertColors", 1);
            else
                adjustmentsMaterial.SetFloat("_InvertColors", 0);
            if (set.binarize)
                adjustmentsMaterial.SetFloat("_Binarize", 1);
            else
                adjustmentsMaterial.SetFloat("_Binarize", 0);
            adjustmentsMaterial.SetFloat("_PosterizationLevels", set.posterizationLevels);

            Graphics.Blit(texture, destination, adjustmentsMaterial);

            return destination;
        }
    }
}