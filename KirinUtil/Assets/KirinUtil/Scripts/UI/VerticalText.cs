using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace KirinUtil
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class VerticalText : MonoBehaviour
    {
        // List of characters to rotate.
        [SerializeField] private List<char> RotatableCharacters;
        [SerializeField] private int spaceSize;
        private TextMeshProUGUI thisText;
        private string preText;

        private void Start()
        {
            thisText = gameObject.GetComponent<TextMeshProUGUI>();
            preText = thisText.text;
            ModifyVertices();
        }

        private void Update()
        {
            if (thisText == null) return;

            if(thisText.text != preText)
            {
                ModifyVertices();
            }
        }

        void ModifyVertices()
        {
            if (thisText == null) return;

            thisText.text = thisText.text.Replace(" ", "<size="+ spaceSize + ">\n\n</size>");
            thisText.ForceMeshUpdate();

            TMP_TextInfo textInfo = thisText.textInfo;
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                // Rotate only if the character is included in the RotatableCharacters list.
                if (IsRotatableCharacter(charInfo.character))
                {
                    // Rotation logic
                    Vector3 mid = (charInfo.bottomLeft + charInfo.topRight) / 2;
                    for (int j = 0; j < 4; j++)
                    {
                        Vector3 offset =
                            textInfo.meshInfo[0].vertices[charInfo.vertexIndex + j] - mid;
                        textInfo.meshInfo[0].vertices[charInfo.vertexIndex + j] =
                            mid + new Vector3(offset.y, -offset.x, offset.z);
                    }
                }
            }

            thisText.UpdateVertexData();
        }

        bool IsRotatableCharacter(char character)
        {
            if (RotatableCharacters == null) return false;
            return RotatableCharacters.Contains(character);
        }

    }
}