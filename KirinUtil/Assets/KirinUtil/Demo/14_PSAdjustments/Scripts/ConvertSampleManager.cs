using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil.Demo
{
    public class ConvertSampleManager : MonoBehaviour
    {
        [SerializeField] private PSAdjustments adjustments;
        [SerializeField] private RawImage targetlImage;
        [SerializeField] private RawImage convertImage;
        [SerializeField] private PSAdjustments.Setting adjustmentsSetting;

        public void ConvertBtnTap()
        {
            convertImage.texture = adjustments.Convert(targetlImage.texture, adjustmentsSetting);
        }
    }
}