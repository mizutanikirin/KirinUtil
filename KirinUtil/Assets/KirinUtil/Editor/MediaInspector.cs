using UnityEngine;
using UnityEditor;
using KirinUtil;

[CustomEditor(typeof(AlphaMediaManager))]
public class MediaInspector : Editor
{
    SerializedProperty mediasProp;
    private int currentArraySize;

    private void OnEnable()
    {
        mediasProp = serializedObject.FindProperty("medias");
        currentArraySize = mediasProp.arraySize;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("rootPath"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mediaDirPath"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startLoad"));

        // medias配列のサイズ調整
        // Medias配列のサイズ調整（横に表示）
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Medias");
        GUILayout.FlexibleSpace(); // 右端に寄せるためのスペース
        int newArraySize = EditorGUILayout.DelayedIntField(currentArraySize, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        if (newArraySize != currentArraySize)
        {
            currentArraySize = newArraySize;
            mediasProp.arraySize = currentArraySize;
        }

        EditorGUI.indentLevel++;
        for (int i = 0; i < mediasProp.arraySize; i++)
        {
            SerializedProperty mediaProp = mediasProp.GetArrayElementAtIndex(i);
            SerializedProperty typeProp = mediaProp.FindPropertyRelative("type");

            EditorGUILayout.PropertyField(mediaProp, new GUIContent("Media " + i), false);

            if (mediaProp.isExpanded)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(typeProp);

                AlphaMediaManager.MediaType mediaType = (AlphaMediaManager.MediaType)typeProp.enumValueIndex;

                // 共通設定
                EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("id"));
                EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("path"));
                EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("isLoop"));
                EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("playOnAwake"));
                EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("startVisible"));
                EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("parentObj"));
                EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("pos"));
                EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("angle"));
                EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("scale"));

                // MediaTypeに応じた追加フィールド
                if (mediaType == AlphaMediaManager.MediaType.Image)
                {
                    EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("loopStartFileName"));
                    EditorGUILayout.PropertyField(mediaProp.FindPropertyRelative("framerate"));
                }

                EditorGUI.indentLevel--;
            }
        }
        EditorGUI.indentLevel--;
        EditorGUI.indentLevel--;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("loadedEvent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playEndEvent"));

        serializedObject.ApplyModifiedProperties();
    }

}
