using UnityEngine;
using UnityEditor;

namespace KirinUtil
{
#if UNITY_EDITOR
    // based on: https://gist.github.com/frarees/9791517
    // https://q7z.hatenablog.com/entry/2016/07/21/204028
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    class MinMaxSliderDrawer : PropertyDrawer
    {
        /// <summary>インデント段階を保存</summary>
        private int lastIndentLevel;

        /// <summary>インデント量</summary>
        private int indentAmount;

        /// <summary>描画領域全体</summary>
        private Rect wholeRect;

        /// <summary>ラベルの幅</summary>
        private const float LabelWidth = 60;

        /// <summary>ラベルの間隔</summary>
        private const float LabelMargin = 5;


        /// <summary>
        /// 描画を行う
        /// </summary>
        /// <param name="position">描画範囲</param>
        /// <param name="property">対象プロパティ</param>
        /// <param name="label">表示するラベル</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BeginProperty(position, property, label);

            // Vector2のみ対応
            if (property.propertyType == SerializedPropertyType.Vector2 ||
                property.propertyType == SerializedPropertyType.Vector2Int ||
                property.propertyType == SerializedPropertyType.Generic && property.type == "MinMax"
                )
            {
                float min;
                float max;
                Vector2 range;
                bool isMinMax;
                SerializedProperty minProp;
                SerializedProperty maxProp;
                if (property.propertyType == SerializedPropertyType.Generic && property.type == "MinMax")
                {
                    isMinMax = true;
                    minProp = property.FindPropertyRelative("min");
                    maxProp = property.FindPropertyRelative("max");
                    min = minProp.floatValue;
                    max = maxProp.floatValue;
                    range = new Vector2(min, max);
                }
                else
                {
                    isMinMax = false;
                    minProp = null;
                    maxProp = null;
                    range = property.vector2Value;
                    min = range.x;
                    max = range.y;
                }


                // 値の取得
                MinMaxSliderAttribute attr = attribute as MinMaxSliderAttribute;
                EditorGUI.BeginChangeCheck();

                var rect = this.wholeRect;

                // min field
                rect.width = LabelWidth;
                min = EditorGUI.FloatField(rect, min);
                min = Mathf.Clamp(min, attr.min, max);

                // min max slider
                rect.x += LabelWidth + LabelMargin;
                rect.width = this.wholeRect.width - LabelWidth * 2 - LabelMargin * 2;
                EditorGUI.MinMaxSlider(rect, new GUIContent(), ref min, ref max, attr.min, attr.max);

                // max field
                rect.x += this.wholeRect.width - LabelWidth * 2 - LabelMargin;
                rect.width = LabelWidth;
                max = EditorGUI.FloatField(rect, max);
                max = Mathf.Clamp(max, min, attr.max);

                // 変更の適用
                if (EditorGUI.EndChangeCheck())
                {
                    if (isMinMax)
                    {
                        minProp.floatValue = min;
                        maxProp.floatValue = max;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                    else
                    {
                        range.x = min;
                        range.y = max;
                        property.vector2Value = range;
                    }
                }
            }
            else
            {
                EditorGUI.LabelField(position, label, "Use only with Vector2");
            }

            EndProperty();
        }

        /// <summary>
        /// プロパティの開始
        /// </summary>
        /// <param name="position">OnGUIと同じ</param>
        /// <param name="property">OnGUIと同じ</param>
        /// <param name="label">OnGUIと同じ</param>
        protected void BeginProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            this.indentAmount = EditorGUI.indentLevel * 16;

            EditorGUIUtility.labelWidth = EditorGUIUtility.labelWidth;
            label = EditorGUI.BeginProperty(position, label, property);

            this.lastIndentLevel = EditorGUI.indentLevel;
            this.wholeRect = EditorGUI.PrefixLabel(position, label);
            EditorGUI.indentLevel = 0;
        }

        /// <summary>
        /// プロパティの終了
        /// </summary>
        protected void EndProperty()
        {
            EditorGUI.indentLevel = this.lastIndentLevel;
            EditorGUI.EndProperty();
        }
    }
#endif

}