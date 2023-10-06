using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class KRNUtilHierarchy
{
    static KRNUtilHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
    }

    private static void OnHierarchyItemGUI(int instanceID, Rect selectionRect)
    {
        GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (go == null) return;
        if (go.name.Length < 3) return;

        int smallCount = 0;
        int bigCount = 0;
        for (int i = 0; i < go.name.Length; i++)
        {
            string word = go.name.Substring(i, 1);
            if (word == "-") smallCount++;
            else if (word == "=") bigCount++;
            else return;
        }

        int lineHeight;
        int linePosY;
        if (smallCount == go.name.Length)
        {
            lineHeight = 2;
            linePosY = 7;
        }
        else if (bigCount == go.name.Length)
        {
            lineHeight = 5;
            linePosY = 6;
        }
        else return;

        // �L���[�u�A�C�R�����B�����߂ɔw�i�F�ŏ㏑������
        Color hierarchyBGColor = EditorGUIUtility.isProSkin ? new Color(0.22f, 0.22f, 0.22f) : Color.white;
        GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, 16, 16), GetTextureWithColor(hierarchyBGColor));

        // ���O�̕��������Ƀ��C����`�悷�邽�߂�Rect�𒲐�
        Rect lineRect = new Rect(selectionRect.x, selectionRect.y + linePosY, selectionRect.width, lineHeight);
        EditorGUI.DrawRect(lineRect, Color.gray);

    }

    private static Texture2D GetTextureWithColor(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }
}
