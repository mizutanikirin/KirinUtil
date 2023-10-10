using System;
using UnityEngine;
using UnityEngine.Events;

namespace KirinUtil
{
    public class FlickManager : MonoBehaviour
    {
        [SerializeField] private RectTransform targetCanvasRT;

        //フリック判定用 時間しきい値
        [SerializeField] private float flickTime = 0.15f;

        //フリック判定用 移動距離
        [SerializeField] private float flickMagnitude = 200;
        [SerializeField] private float flickHeightMagnitude = 100;

        private Vector2[] startPosition;
        private Vector2[] endPosition;
        private float[] timer;
        private Vector3 startPosition2;
        private float timer2;

        [Serializable] public class Flicked : UnityEvent<Vector2, float, float> { }
        [SerializeField] Flicked OnFlicked;

        private void Start()
        {
            // 指10本分
            startPosition = new Vector2[10];
            endPosition = new Vector2[10];
            timer = new float[10];
            for (int i = 0; i < timer.Length; i++)
            {
                timer[i] = 0;
            }
        }

        void Update()
        {
            if (Input.touchSupported)
            {
                if (Input.touchCount == 0) return;

                for (int i = 0; i < Input.touchCount; i++)
                {
                    FlickUpdate(i);
                }
            }
            else
            {
                FlickNoTouchSupportedUpdate();
            }

        }

        private void FlickUpdate(int num)
        {
            //タップ開始時
            if (Input.GetTouch(num).phase == TouchPhase.Began)
            {
                //タップ開始ポイントを取得
                startPosition[num] = GetCanvasPos(Input.GetTouch(num).position);
            }

            //タップ中
            if (Input.GetTouch(num).phase == TouchPhase.Moved)
            {
                //タップ時間がしきい値を越えた場合、スワイプと判定
                if (timer[num] >= flickTime)
                {
                    //Swipe
                }
                //押下している間、タイマーを加算
                timer[num] += Time.deltaTime;
            }

            //タップ終了時
            if (Input.GetTouch(num).phase == TouchPhase.Ended)
            {
                //タップ終了ポイントを取得
                endPosition[num] = GetCanvasPos(Input.GetTouch(num).position);

                //タップ開始～終了ポイントの距離
                Vector2 direction = endPosition[num] - startPosition[num];

                //距離が指定以上、タップ時間が指定以下の場合、フリックと判定
                if (direction.magnitude >= flickMagnitude && timer[num] <= flickTime)
                {
                    //x軸の距離が大きい場合は左右へのフリック
                    if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
                    {
                        Vector2 centerPos = direction / 2.0f + startPosition[num];
                        float harfWidth = targetCanvasRT.rect.width / 2.0f;
                        centerPos.x -= harfWidth;
                        centerPos.y -= targetCanvasRT.rect.height / 2.0f;
                        OnFlicked.Invoke(centerPos, startPosition[num].x - harfWidth, endPosition[num].x - harfWidth);

                        /*if (direction.x >= 0)
                        {
                            //Right Flick
                        }
                        else
                        {
                            //Left Flick
                        }*/
                    }
                }
                //タイマーを初期化
                timer[num] = 0.0f;
            }
        }

        private void FlickNoTouchSupportedUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition2 = GetCanvasPos(Input.mousePosition);
                timer2 = Time.realtimeSinceStartup;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (Time.realtimeSinceStartup - timer2 > flickTime) return;
                Vector3 dif = GetCanvasPos(Input.mousePosition) - startPosition2;

                //Debug.Log($"Flick: x={dif.x} y={dif.y}");

                float abs_x = Mathf.Abs(dif.x);
                float abs_y = Mathf.Abs(dif.y);

                if (abs_x > flickMagnitude && abs_y < flickHeightMagnitude)
                {
                    // 横方向
                    Vector2 centerPos = dif / 2.0f + startPosition2;
                    float harfWidth = targetCanvasRT.rect.width / 2.0f;
                    centerPos.x -= harfWidth;
                    centerPos.y -= targetCanvasRT.rect.height / 2.0f;
                    OnFlicked.Invoke(centerPos, startPosition2.x - harfWidth, GetCanvasPos(Input.mousePosition).x - harfWidth);
                }
            }
        }

        private Vector3 GetCanvasPos(Vector3 touchPos)
        {
            float rateX = targetCanvasRT.rect.width / (float)Screen.width;
            float rateY = targetCanvasRT.rect.height / (float)Screen.height;

            Vector3 pos = new Vector3(touchPos.x * rateX, touchPos.y * rateY);

            return pos;
        }
    }

}