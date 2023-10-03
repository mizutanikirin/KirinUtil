using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace KirinUtil
{

    [RequireComponent(typeof(Image))]
    public class UILine : MonoBehaviour
    {
        //----------------------------------
        //  線を引く(gui)
        //----------------------------------
        public Vector2 startPos;
        public Vector2 endPos;
        public float thick;

        public void Draw()
        {
            // 長さ
            float lineLength = Vector2.Distance(startPos, endPos);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(thick, lineLength);

            // 角度
            float lineAngle = Angle2(startPos, endPos) + 90f;
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, lineAngle);

            // 再度位置を調整
            float moveY = lineLength * 0.5f * Mathf.Cos(lineAngle * Mathf.Deg2Rad);
            float moveX = lineLength * 0.5f * Mathf.Sin(lineAngle * Mathf.Deg2Rad);
            gameObject.transform.localPosition = new Vector3(
                startPos.x + moveX,
                startPos.y - moveY,
                0
            );
        }

        private float Angle2(Vector2 p1, Vector2 p2)
        {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            float rad = Mathf.Atan2(dy, dx);
            return rad * Mathf.Rad2Deg;
        }

    }
}
