using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

namespace KirinUtil
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform))]
    public class TransformInspector : Editor
    {
        enum TargetType
        {
            Position,
            Rotation,
            Scale
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var transform = target as Transform;

            DrawLine("P", TargetType.Position, transform);
            DrawLine("R", TargetType.Rotation, transform);
            DrawLine("S", TargetType.Scale, transform);

            serializedObject.ApplyModifiedProperties();
        }


        void DrawLine(string label, TargetType type, Transform transform)
        {
            Vector3 newValue = Vector3.zero;
            bool reset = false;

            EditorGUI.BeginChangeCheck();

            // Property
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(label, GUILayout.Width(20)))
                {
                    newValue = type == TargetType.Scale ? Vector3.one : Vector3.zero;
                    reset = true;
                }
                if (!reset)
                {
                    switch (type)
                    {

                        case TargetType.Position:
                            newValue = Vector3Field(transform.localPosition);
                            break;
                        case TargetType.Rotation:
                            newValue = Vector3Field(transform.localEulerAngles);
                            break;
                        case TargetType.Scale:
                            newValue = Vector3Field(transform.localScale);
                            break;
                    }
                }
            }

            // Register Undo if changed
            if (EditorGUI.EndChangeCheck() || reset)
            {
                Undo.RecordObjects(targets, string.Format("{0} {1} {2}", (reset ? "Reset" : "Change"), transform.gameObject.name, type.ToString()));
                targets.ToList().ForEach(x =>
                {
                    var t = x as Transform;
                    switch (type)
                    {
                        case TargetType.Position:
                            t.localPosition = newValue;
                            break;
                        case TargetType.Rotation:
                            t.localEulerAngles = newValue;
                            break;
                        case TargetType.Scale:
                            t.localScale = newValue;
                            break;
                        default:
                            Debug.Assert(false, "should not reach here");
                            break;
                    }
                    EditorUtility.SetDirty(x);
                });
            }

        }

        private static Vector3 Vector3Field(Vector3 value)
        {
            return EditorGUILayout.Vector3Field(string.Empty, value, GUILayout.Height(16));
        }
    }


    /*[CanEditMultipleObjects]
    [CustomEditor(typeof(RectTransform))]
    public class RectTransformInspector : Editor
    {
        enum TargetType
        {
            Position,
            Rotation,
            Scale
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var transform = target as Transform;
            var rectTrf = target as RectTransform;

            DrawLine("P", TargetType.Position, transform);
            DrawLine("R", TargetType.Rotation, transform);
            DrawLine("S", TargetType.Scale, transform);
            //DrawLine2("P2", TargetType.Position, rectTrf);

            serializedObject.ApplyModifiedProperties();
        }
        void DrawLine2(string label, TargetType type, RectTransform transform)
        {
            Vector3 newValue = Vector3.zero;
            bool reset = false;

            EditorGUI.BeginChangeCheck();

            // Property
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(label, GUILayout.Width(20)))
                {
                    newValue = type == TargetType.Scale ? Vector3.one : Vector3.zero;
                    reset = true;
                }
                if (!reset)
                {
                    switch (type)
                    {

                        case TargetType.Position:
                            newValue = Vector3Field(transform.localPosition);
                            break;
                        case TargetType.Rotation:
                            newValue = Vector3Field(transform.localEulerAngles);
                            break;
                        case TargetType.Scale:
                            newValue = Vector3Field(transform.localScale);
                            break;
                    }
                }
            }

            // Register Undo if changed
            if (EditorGUI.EndChangeCheck() || reset)
            {
                Undo.RecordObjects(targets, string.Format("{0} {1} {2}", (reset ? "Reset" : "Change"), transform.gameObject.name, type.ToString()));
                targets.ToList().ForEach(x =>
                {
                    var t = x as Transform;
                    switch (type)
                    {
                        case TargetType.Position:
                            t.localPosition = newValue;
                            break;
                        case TargetType.Rotation:
                            t.localEulerAngles = newValue;
                            break;
                        case TargetType.Scale:
                            t.localScale = newValue;
                            break;
                        default:
                            Debug.Assert(false, "should not reach here");
                            break;
                    }
                    EditorUtility.SetDirty(x);
                });
            }

        }


        void DrawLine(string label, TargetType type, Transform transform)
        {
            Vector3 newValue = Vector3.zero;
            bool reset = false;

            EditorGUI.BeginChangeCheck();

            // Property
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(label, GUILayout.Width(20)))
                {
                    newValue = type == TargetType.Scale ? Vector3.one : Vector3.zero;
                    reset = true;
                }
                if (!reset)
                {
                    switch (type)
                    {

                        case TargetType.Position:
                            newValue = Vector3Field(transform.localPosition);
                            break;
                        case TargetType.Rotation:
                            newValue = Vector3Field(transform.localEulerAngles);
                            break;
                        case TargetType.Scale:
                            newValue = Vector3Field(transform.localScale);
                            break;
                    }
                }
            }

            // Register Undo if changed
            if (EditorGUI.EndChangeCheck() || reset)
            {
                Undo.RecordObjects(targets, string.Format("{0} {1} {2}", (reset ? "Reset" : "Change"), transform.gameObject.name, type.ToString()));
                targets.ToList().ForEach(x =>
                {
                    var t = x as Transform;
                    switch (type)
                    {
                        case TargetType.Position:
                            t.localPosition = newValue;
                            break;
                        case TargetType.Rotation:
                            t.localEulerAngles = newValue;
                            break;
                        case TargetType.Scale:
                            t.localScale = newValue;
                            break;
                        default:
                            Debug.Assert(false, "should not reach here");
                            break;
                    }
                    EditorUtility.SetDirty(x);
                });
            }

        }

        private static Vector3 Vector3Field(Vector3 value)
        {
            return EditorGUILayout.Vector3Field(string.Empty, value, GUILayout.Height(16));
        }
    }
    */
}