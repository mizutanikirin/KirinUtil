using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshFilter))]
[CanEditMultipleObjects]
public class MeshFilterEditor : Editor
{
    private static bool showVertices = false; // ���_�\���̏�Ԃ��g���b�N����ϐ�

    [MenuItem("KirinUtil/Vertex/Vertex display", false)]
    private static void ShowVertices()
    {
        showVertices = true;
        SceneView.RepaintAll(); // �V�[���r���[���ĕ`��
    }

    [MenuItem("KirinUtil/Vertex/Vertex hidden", false)]
    private static void HideVertices()
    {
        showVertices = false;
        SceneView.RepaintAll(); // �V�[���r���[���ĕ`��
    }

    [MenuItem("KirinUtil/Vertex/Vertex display", true)]
    private static bool ValidateShowVertices()
    {
        return !showVertices;
    }

    [MenuItem("KirinUtil/Vertex/Vertex hidden", true)]
    private static bool ValidateHideVertices()
    {
        return showVertices;
    }

    void OnSceneGUI()
    {
        if (!showVertices) return; // ���_��\�����Ȃ��ꍇ�A�������Ȃ�

        MeshFilter meshFilter = target as MeshFilter;
        if (meshFilter == null || meshFilter.sharedMesh == null) return;

        Vector3[] vertices = meshFilter.sharedMesh.vertices;
        Transform transform = meshFilter.transform;

        Handles.color = Color.red;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            Handles.SphereHandleCap(0, worldPos, Quaternion.identity, 0.15f * HandleUtility.GetHandleSize(worldPos), EventType.Repaint);
        }

        Handles.color = Color.white;
    }
}
