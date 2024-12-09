using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

namespace KirinUtil
{
    public class SetTMP : MonoBehaviour
    {
        // xmlをTMPData型に変換
        public TMPData Xml2TMPData(XmlNodeList nodeList)
        {
            TMPData textData = new TMPData();

            foreach (XmlNode node in nodeList)
            {
                print(node.Name);
                if (node.Name == "text")
                {
                    textData.message = node.InnerText;
                    
                    textData.textPos.x = float.Parse(node.Attributes["x"].Value);
                    textData.textPos.y = float.Parse(node.Attributes["y"].Value);
                    textData.textPos.z = 0;
                    
                    textData.width = float.Parse(node.Attributes["width"].Value);

                    if (node.Attributes["size"] != null)
                    {
                        textData.fontSize = new MinMax(
                            int.Parse(node.Attributes["size"].Value), 
                            int.Parse(node.Attributes["size"].Value)
                        );
                    }
                    textData.fontSize = new MinMax();
                    if (node.Attributes["sizeMin"] != null)
                        textData.fontSize.min = int.Parse(node.Attributes["sizeMin"].Value);
                    if (node.Attributes["sizeMax"] != null)
                        textData.fontSize.max = int.Parse(node.Attributes["sizeMax"].Value);

                    string align = node.Attributes["align"].Value;
                    if (align == "Left") textData.align = Align.Left;
                    else if (align == "Right") textData.align = Align.Right;
                    else textData.align = Align.Center;

                    ColorUtility.TryParseHtmlString(node.Attributes["color"].Value, out textData.fontColor);

                    textData.outlineEnable = bool.Parse(node.Attributes["outline"].Value);
                    if (node.Attributes["outlineColor"] != null)
                        ColorUtility.TryParseHtmlString(node.Attributes["outlineColor"].Value, out textData.outlineColor);
                    if (node.Attributes["outlineThickness"] != null)
                        textData.outlineThikness = float.Parse(node.Attributes["outlineThickness"].Value);
                }
            }

            return textData;
        }

        // targetTextにtextData設定を適用
        public void Custum(TextMeshProUGUI targetText, TMPData textData)
        {
            // message
            targetText.text = textData.message;

            // posiiton
            targetText.transform.localPosition = textData.textPos;

            // font size
            targetText.enableAutoSizing = true;
            targetText.fontSizeMax = textData.fontSize.max;
            targetText.fontSizeMin = textData.fontSize.min;

            // font width
            targetText.rectTransform.sizeDelta = new Vector2(
                textData.width,
                targetText.rectTransform.sizeDelta.y
            );

            // font color
            targetText.color = textData.fontColor;

            // align
            if (textData.align == Align.Left) targetText.alignment = TextAlignmentOptions.Left;
            else if (textData.align == Align.Center) targetText.alignment = TextAlignmentOptions.Center;
            else targetText.alignment = TextAlignmentOptions.Right;

            // outline
            targetText.fontSharedMaterial.EnableKeyword(ShaderUtilities.Keyword_Outline);
            float outlineThikness;
            if (textData.outlineEnable)
            {
                outlineThikness = textData.outlineThikness;
                targetText.outlineColor = textData.outlineColor;
            }
            else
            {
                outlineThikness = 0;
            }
            targetText.outlineWidth = outlineThikness;
            targetText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, outlineThikness);
        }
    }
}
