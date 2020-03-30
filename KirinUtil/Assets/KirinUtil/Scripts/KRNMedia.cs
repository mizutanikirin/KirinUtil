using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {

    public class KRNMedia : MonoBehaviour {

        //----------------------------------
        //  fade in / out
        //----------------------------------
        #region fade
        // gameObjectをフェードさせる

        // gameobject
        public void FadeInOut(GameObject _nowObj, GameObject _nextObj, float _fadeTime, float _delayTime) {
            iTween.FadeTo(_nowObj, iTween.Hash("alpha", 0, "time", _fadeTime));
            iTween.FadeTo(_nextObj, iTween.Hash("alpha", 1, "time", _fadeTime, "delay", _delayTime));

            StopTween stopTweenNow = _nowObj.GetComponent<StopTween>();
            if (stopTweenNow == null) _nowObj.AddComponent<StopTween>();

            StopTween stopTweenNext = _nextObj.GetComponent<StopTween>();
            if (stopTweenNext == null) _nextObj.AddComponent<StopTween>();
        }

        public void FadeIn(GameObject _obj, float _fadeTime, float _delayTime) {
            iTween.FadeTo(_obj, iTween.Hash("alpha", 1, "time", _fadeTime, "delay", _delayTime));

            StopTween stopTweenNow = _obj.GetComponent<StopTween>();
            if (stopTweenNow == null) _obj.AddComponent<StopTween>();
        }

        public void FadeOut(GameObject _obj, float _fadeTime, float _delayTime) {
            iTween.FadeTo(_obj, iTween.Hash("alpha", 0, "time", _fadeTime, "delay", _delayTime));

            StopTween stopTweenNow = _obj.GetComponent<StopTween>();
            if (stopTweenNow == null) _obj.AddComponent<StopTween>();
        }

        // gui
        public void FadeInOutUI(GameObject inObj, GameObject outObj, float time, float delay, iTween.EaseType easetype = iTween.EaseType.easeOutCubic) {
            FadeGui fadeGuiIn = inObj.GetComponent<FadeGui>();
            if (fadeGuiIn == null) {
                fadeGuiIn = inObj.AddComponent<FadeGui>();
                inObj.GetComponent<CanvasGroup>().alpha = 0f;
            }

            FadeGui fadeGuiOut = outObj.GetComponent<FadeGui>();
            if (fadeGuiOut == null) {
                fadeGuiOut = outObj.AddComponent<FadeGui>();
                outObj.GetComponent<CanvasGroup>().alpha = 1f;
            }

            fadeGuiIn.FadeIn(time, delay, easetype);
            fadeGuiOut.FadeOut(time, delay, easetype);
        }

        public void FadeInUI(GameObject obj, float time, float delay, iTween.EaseType easetype = iTween.EaseType.easeOutCubic) {
            FadeGui fadeGui = obj.GetComponent<FadeGui>();
            if (fadeGui == null) {
                fadeGui = obj.AddComponent<FadeGui>();
                obj.GetComponent<CanvasGroup>().alpha = 0f;
            }

            fadeGui.FadeIn(time, delay, easetype);
        }

        public void FadeOutUI(GameObject obj, float time, float delay, iTween.EaseType easetype = iTween.EaseType.easeOutCubic) {
            FadeGui fadeGui = obj.GetComponent<FadeGui>();
            if (fadeGui == null) {
                fadeGui = obj.AddComponent<FadeGui>();
                obj.GetComponent<CanvasGroup>().alpha = 1f;
            }

            fadeGui.FadeOut(time, delay, easetype);
        }
        #endregion


        //----------------------------------
        //  CloneGameObject
        //----------------------------------
        // 指定した親GameObjectに指定したGameObjectをコピーする
        public void CloneGameObject(GameObject obj, GameObject parentObj, string name = "") {
            GameObject clone = GameObject.Instantiate(obj) as GameObject;
            clone.transform.SetParent(parentObj.transform, false);
            if (name == "") clone.name = obj.name;
            else clone.name = name;

            clone.transform.localPosition = obj.transform.localPosition;
            clone.transform.localScale = obj.transform.localScale;
            clone.transform.localRotation = obj.transform.localRotation;
        }


        //----------------------------------
        //  CenterGameObject
        //----------------------------------
        // GameObjectを画面の中央に移動させる
        public void SetCenter(GameObject obj, Camera cam) {
            obj.transform.position = new Vector3(
                cam.transform.position.x,
                cam.transform.position.y,
                0
            );
        }

        //----------------------------------
        //  GetAllGameObject
        //----------------------------------
        #region GetAllGameObject
        // 親GameObjectの下にあるGameObjectをListで返す
        // original code
        // http://kazuooooo.hatenablog.com/entry/2015/08/07/010938

        public List<GameObject> GetAllGameObject(GameObject parentObj) {
            List<GameObject> allChildren = new List<GameObject>();
            GetChildren(parentObj, ref allChildren);
            return allChildren;
        }

        // 子要素を取得してリストに追加
        private void GetChildren(GameObject obj, ref List<GameObject> allChildren) {
            Transform children = obj.GetComponentInChildren<Transform>();
            // 子要素がいなければ終了
            if (children.childCount == 0) {
                return;
            }
            foreach (Transform ob in children) {
                allChildren.Add(ob.gameObject);
                GetChildren(ob.gameObject, ref allChildren);
            }
        }
        #endregion

        #region 指定の文字が入っているGameObjectのみgetする
        public List<GameObject> GetAllGameObject(GameObject parentObj, string name) {
            List<GameObject> allChildren = new List<GameObject>();
            GetChildren(parentObj, name, ref allChildren);
            return allChildren;
        }

        // 子要素を取得してリストに追加
        private void GetChildren(GameObject obj, string name, ref List<GameObject> allChildren) {
            Transform children = obj.GetComponentInChildren<Transform>();
            // 子要素がいなければ終了
            if (children.childCount == 0) {
                return;
            }
            foreach (Transform ob in children) {
                if (ob.gameObject.name.IndexOf(name) != -1) {
                    allChildren.Add(ob.gameObject);
                    GetChildren(ob.gameObject, name, ref allChildren);
                }

            }
        }
        #endregion

        #region 直下のGameObjectのみgetする
        // 直下のGameObjectのみgetする
        public List<GameObject> GetChildGameObject(GameObject parentObj) {
            List<GameObject> childrenObj = new List<GameObject>();
            foreach (Transform child in parentObj.transform) {
                childrenObj.Add(child.gameObject);
            }

            return childrenObj;
        }

        // 直下のGameObjectの指定した文字が入っているGameObjectのみgetする
        public List<GameObject> GetChildGameObject(GameObject parentObj, string name) {
            List<GameObject> childrenObj = new List<GameObject>();
            foreach (Transform child in parentObj.transform) {
                if (child.gameObject.name.IndexOf(name) != -1) {
                    childrenObj.Add(child.gameObject);
                }
            }

            return childrenObj;
        }
        #endregion


        //----------------------------------
        //  DeleteAllGameObject
        //----------------------------------
        // 親GameObjectの下にあるGameObjectをすべて消す
        public void DeleteAllGameObject(GameObject parentObj, bool delParent) {
            List<GameObject> allGameObj = GetAllGameObject(parentObj);
            foreach (GameObject thisObj in allGameObj) {
                Destroy(thisObj);
            }

            if(delParent) Destroy(parentObj);
        }


        //----------------------------------
        //  SetAlpha
        //----------------------------------
        public void SetAlpha(GameObject obj, float alpha) {
            obj.GetComponent<Renderer>().material.SetAlpha(alpha);
        }

        //----------------------------------
        //  SetAlphaAllGameObject
        //----------------------------------
        // 親GameObjectの下にあるGameObjectをすべて指定したalphaの値に透過させる
        public void SetAlphaAllGameObject(GameObject parentObj, float alpha) {
            List<GameObject> allGameObj = GetAllGameObject(parentObj);
            foreach (GameObject thisObj in allGameObj) {
                try {
                    //thisObj.GetComponent<Renderer>().material.SetAlpha(alpha);
                    SetAlpha(thisObj, alpha);
                } catch (Exception e) {
                    print("SetAlphaAllGameObject ERROR: " + e.Message);
                }

            }
        }

        //----------------------------------
        //  Textureの色を明るくする
        //----------------------------------
        #region ChangeTextureBright
        public void ChangeTextureBright(GameObject obj, float bright) {

            // エラーが出る場合はTexture設定をInspectorで[Read/Write Enabled]にする必要あり
            Texture2D mainTexture = obj.GetComponent<Renderer>().material.mainTexture as Texture2D;
            Color[] pixels = mainTexture.GetPixels();

            // 書き換え用テクスチャ用配列の作成
            Color[] change_pixels = new Color[pixels.Length];
            for (int i = 0; i < pixels.Length; i++) {
                Color pixel = pixels[i];

                // 書き換え用テクスチャのピクセル色を指定
                pixel = Bright(pixel, bright);
                Color change_pixel = new Color(pixel.r, pixel.g, pixel.b, pixel.a);
                change_pixels.SetValue(change_pixel, i);
            }

            // 書き換え用テクスチャの生成
            Texture2D changeTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
            changeTexture.filterMode = FilterMode.Point;
            changeTexture.SetPixels(change_pixels);
            changeTexture.Apply();

            // テクスチャを貼り替える
            obj.GetComponent<Renderer>().material.mainTexture = changeTexture;
        }

        private Color Bright(Color color, float bright) {

            color.r = Mathf.Clamp(bright * color.r, 0, 255);
            color.g = Mathf.Clamp(bright * color.g, 0, 255);
            color.b = Mathf.Clamp(bright * color.b, 0, 255);

            return color;
        }
        #endregion

        //----------------------------------
        //  #ffffからColorに変換
        //----------------------------------
        #region HexToColor
        public Color HexToColor(string hex) {
            hex = hex.Replace("0x", "");  // 0xFFFFFF
            hex = hex.Replace("#", "");  // #FFFFFF

            if (hex.Length < 6) {
                return new Color32(255, 255, 255, 255);
            }

            byte a = 255;
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            if (hex.Length == 8) {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }
        #endregion

        //----------------------------------
        //  GameObjectのパスを返す
        //----------------------------------
        #region GetGameObjectPath
        public string GetGameObjectPath(GameObject targetObj) {
            Transform targetTransform = targetObj.transform;

            string path = targetTransform.gameObject.name;
            Transform parentTransform = targetTransform.parent;

            while (parentTransform != null) {
                path = parentTransform.name + "/" + path;
                parentTransform = parentTransform.parent;
            }

            return path;
        }
        #endregion

        //----------------------------------
        //  GameObjectのレイヤをすべて変える
        //----------------------------------
        #region ChangeAllLayer
        public void ChangeAllLayer(GameObject prentObj, string layerName) {
            // レイヤー名変更
            prentObj.layer = LayerMask.NameToLayer(layerName);
            List<GameObject> objs = GetAllGameObject(prentObj);
            for (int i = 0; i < objs.Count; i++) {
                objs[i].layer = LayerMask.NameToLayer(layerName);
            }
        }
        #endregion

        //----------------------------------
        //  GameObjectを作る
        //----------------------------------
        #region CreateObj
        public GameObject CreateUIObj(GameObject prefab, GameObject parentObj, string name, Vector3 pos, Vector3 rotate, Vector3 scale) {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.name = name;
            obj.transform.SetParent(parentObj.transform, false);
            obj.transform.localPosition = pos;
            obj.transform.localScale = scale;
            obj.transform.localRotation = Quaternion.Euler(rotate.x, rotate.y, rotate.z);

            return obj;
        }

        public GameObject CreateObj(GameObject prefab, GameObject parentObj, string name, Vector3 pos, Vector3 rotate, Vector3 scale) {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.name = name;
            obj.transform.SetParent(parentObj.transform, false);
            obj.transform.position = pos;
            obj.transform.localScale = scale;
            obj.transform.rotation = Quaternion.Euler(rotate.x, rotate.y, rotate.z);

            return obj;
        }
        #endregion

        //----------------------------------
        //  縦横比維持(縦を基準)
        //----------------------------------
        public Vector2 AdjustResolutionHeight(Vector2 originalResolution, float height) {

            float rate = height / originalResolution.y;
            float resolutionX = rate * originalResolution.x;


            return new Vector2(resolutionX, height);

        }

        //----------------------------------
        //  移動しながらUIを表示 / 非表示
        //----------------------------------
        // メッセージボックスとかパネルのフェードしながら移動
        #region UIDisplay
        public void UIDisplay(GameObject obj, FadeType fadeType, Direction direction, float animTime = 1f, float movePixel = 10f) {

            if (fadeType == FadeType.FadeIn) FadeInUI(obj, animTime + 0.2f, 0);
            else FadeOutUI(obj, animTime + 0.2f, 0, 0);

            Vector3 initPos = obj.transform.localPosition;
            if (direction == Direction.Up || direction == Direction.Down) {
                // 初期位置
                float movePos = 0;

                if (fadeType == FadeType.FadeIn) {
                    float thisInitY = 0;
                    if (direction == Direction.Up) thisInitY = initPos.y - movePixel;
                    else thisInitY = initPos.y + movePixel;
                    Util.LocalPosY(obj, thisInitY);

                    iTween.MoveTo(obj,
                        iTween.Hash(
                            "y", initPos.y + movePos,
                            "time", animTime,
                            "islocal", true,
                            "EaseType", iTween.EaseType.easeOutQuart
                        )
                    );
                } else {
                    if (direction == Direction.Up) movePos = movePixel;
                    else movePos = -movePixel;

                    Hashtable param = new Hashtable();
                    param.Add("x", initPos.x);
                    param.Add("y", initPos.y);
                    param.Add("z", initPos.z);
                    param.Add("obj", obj);

                    iTween.MoveTo(obj,
                        iTween.Hash(
                            "y", initPos.y + movePos,
                            "time", animTime,
                            "islocal", true,
                            "EaseType", iTween.EaseType.easeOutQuart,
                            "oncomplete", "UIDisplayFadeOutEnd",
                            "oncompleteparams", param,
                            "oncompletetarget", gameObject
                        )
                    );
                }

            } else {
                // 初期位置
                float movePos = 0;

                if (fadeType == FadeType.FadeIn) {
                    float thisInitX = 0;
                    if (direction == Direction.Left) thisInitX = initPos.x + movePixel;
                    else thisInitX = initPos.x - movePixel;
                    Util.LocalPosX(obj, thisInitX);

                    iTween.MoveTo(obj,
                        iTween.Hash(
                            "x", initPos.x,
                            "time", animTime,
                            "islocal", true,
                            "EaseType", iTween.EaseType.easeOutQuart
                        )
                    );
                } else {
                    if (direction == Direction.Left) movePos = -movePixel;
                    else movePos = movePixel;

                    Hashtable param = new Hashtable();
                    param.Add("x", initPos.x);
                    param.Add("y", initPos.y);
                    param.Add("z", initPos.z);
                    param.Add("obj", obj);

                    iTween.MoveTo(obj,
                        iTween.Hash(
                            "x", initPos.x + movePos,
                            "time", animTime,
                            "islocal", true,
                            "EaseType", iTween.EaseType.easeOutQuart,
                            "oncomplete", "UIDisplayFadeOutEnd",
                            "oncompleteparams", param,
                            "oncompletetarget", gameObject
                        )
                    );
                }
            }

        }

        private void UIDisplayFadeOutEnd(Hashtable param) {
            GameObject obj = param["obj"] as GameObject;

            float initPosX = float.Parse(param["x"].ToString());
            float initPosY = float.Parse(param["y"].ToString());
            float initPosZ = float.Parse(param["z"].ToString());

            obj.transform.localPosition = new Vector3(initPosX, initPosY, initPosZ);
        }
        #endregion


        //----------------------------------
        //  UIのサイズをGet/Set
        //----------------------------------
        #region UIのサイズをGet/Set
        public Vector2 GetUISize(GameObject uiObj) {
            return uiObj.GetComponent<RectTransform>().sizeDelta;
        }

        public Vector2 GetUISize(Image uiObj) {
            return uiObj.GetComponent<RectTransform>().sizeDelta;
        }

        public Vector2 GetUISize(RawImage uiObj) {
            return uiObj.GetComponent<RectTransform>().sizeDelta;
        }

        public Vector2 GetUISize(Button uiObj) {
            return uiObj.GetComponent<RectTransform>().sizeDelta;
        }

        public void SetUISize(GameObject uiObj, Vector2 size) {
            uiObj.GetComponent<RectTransform>().sizeDelta = size;
        }

        public void SetUISize(Image uiObj, Vector2 size) {
            uiObj.GetComponent<RectTransform>().sizeDelta = size;
        }

        public void SetUISize(RawImage uiObj, Vector2 size) {
            uiObj.GetComponent<RectTransform>().sizeDelta = size;
        }

        public void SetUISize(Button uiObj, Vector2 size) {
            uiObj.GetComponent<RectTransform>().sizeDelta = size;
        }
        #endregion


        //----------------------------------
        //  簡単な動き
        //----------------------------------

        #region MoveHorizon
        public void MoveHorizon(GameObject obj, float moveValue, float moveTime, float delay, int times = -1, bool islocal = true, bool returnInitPos = false) {

            Hashtable hash = new Hashtable();
            hash.Add("obj", obj);
            hash.Add("moveValue", moveValue);
            hash.Add("moveTime", moveTime);
            hash.Add("times", times * 2);
            hash.Add("islocal", islocal);
            hash.Add("isPlus", true);
            hash.Add("returnInitPos", returnInitPos);
            hash.Add("delay", delay);

            float moveInitPosX;
            if (islocal) moveInitPosX = obj.transform.localPosition.x;
            else moveInitPosX = obj.transform.position.x;
            hash.Add("moveInitPosX", moveInitPosX);

            MoveHorizonLoop(hash);
        }

        private void MoveHorizonLoop(Hashtable hash) {

            GameObject obj = hash["obj"] as GameObject;
            float moveValue = float.Parse(hash["moveValue"].ToString());
            float moveTime = float.Parse(hash["moveTime"].ToString());
            float times = float.Parse(hash["times"].ToString());
            bool islocal = bool.Parse(hash["islocal"].ToString());
            bool isPlus = bool.Parse(hash["isPlus"].ToString());
            float moveInitPosX = float.Parse(hash["moveInitPosX"].ToString());
            bool returnInitPos = bool.Parse(hash["returnInitPos"].ToString());
            float delay = float.Parse(hash["delay"].ToString());
            hash["delay"] = 0;

            if (times >= 0) {
                if (times == 0) {
                    if (returnInitPos) {
                        iTween.MoveTo(obj,
                            iTween.Hash(
                                "x", moveInitPosX,
                                "time", moveTime,
                                "islocal", islocal,
                                "EaseType", iTween.EaseType.easeInOutQuart
                            )
                        );
                    } else {
                        iTween.Stop(obj, "move");
                    }

                    return;
                }
                times--;
                hash["times"] = times;
            }

            int plusMinus = 1;
            if (!isPlus) plusMinus = -1;
            isPlus = !isPlus;
            hash["isPlus"] = isPlus;

            iTween.MoveTo(obj,
                iTween.Hash(
                    "x", moveInitPosX + moveValue * plusMinus,
                    "time", moveTime,
                    "delay", delay,
                    "islocal", islocal,
                    "EaseType", iTween.EaseType.easeInOutQuart,
                    "oncomplete", "MoveHorizonLoop",
                    "oncompleteparams", hash,
                    "oncompletetarget", gameObject
                )
            );
        }

        public void StopMoveHorizon(GameObject obj) {
            iTween.Stop(obj, "move");
        }
        #endregion

        #region MoveVertical
        public void MoveVertical(GameObject obj, float moveValue, float moveTime, float delay, int times = -1, bool islocal = true, bool returnInitPos = false) {

            Hashtable hash = new Hashtable();
            hash.Add("obj", obj);
            hash.Add("moveValue", moveValue);
            hash.Add("moveTime", moveTime);
            hash.Add("times", times * 2);
            hash.Add("islocal", islocal);
            hash.Add("isPlus", true);
            hash.Add("returnInitPos", returnInitPos);
            hash.Add("delay", delay);

            float moveInitPosY;
            if (islocal) moveInitPosY = obj.transform.localPosition.y;
            else moveInitPosY = obj.transform.position.y;
            hash.Add("moveInitPosY", moveInitPosY);

            MoveVerticalLoop(hash);
        }

        private void MoveVerticalLoop(Hashtable hash) {

            GameObject obj = hash["obj"] as GameObject;
            float moveValue = float.Parse(hash["moveValue"].ToString());
            float moveTime = float.Parse(hash["moveTime"].ToString());
            float times = float.Parse(hash["times"].ToString());
            bool islocal = bool.Parse(hash["islocal"].ToString());
            bool isPlus = bool.Parse(hash["isPlus"].ToString());
            float moveInitPosY = float.Parse(hash["moveInitPosY"].ToString());
            bool returnInitPos = bool.Parse(hash["returnInitPos"].ToString());
            float delay = float.Parse(hash["delay"].ToString());
            hash["delay"] = 0;

            if (times >= 0) {
                if (times == 0) {
                    if (returnInitPos) {
                        iTween.MoveTo(obj,
                            iTween.Hash(
                                "y", moveInitPosY,
                                "time", moveTime,
                                "islocal", islocal,
                                "EaseType", iTween.EaseType.easeInOutQuart
                            )
                        );
                    } else {
                        iTween.Stop(obj, "move");
                    }

                    return;
                }
                times--;
                hash["times"] = times;
            }

            int plusMinus = 1;
            if (!isPlus) plusMinus = -1;
            isPlus = !isPlus;
            hash["isPlus"] = isPlus;

            iTween.MoveTo(obj,
                iTween.Hash(
                    "y", moveInitPosY + moveValue * plusMinus,
                    "time", moveTime,
                    "islocal", islocal,
                    "delay", delay,
                    "EaseType", iTween.EaseType.easeInOutQuart,
                    "oncomplete", "MoveVerticalLoop",
                    "oncompleteparams", hash,
                    "oncompletetarget", gameObject
                )
            );
        }

        public void StopMoveVertical(GameObject obj) {
            iTween.Stop(obj, "move");
        }
        #endregion

        #region MoveRandom
        public void MoveRandom(GameObject obj, float moveValue, float moveTime, float delay, int times = -1, bool islocal = true, bool returnInitPos = false) {

            Hashtable hash = new Hashtable();
            hash.Add("obj", obj);
            hash.Add("moveValue", moveValue);
            hash.Add("moveTime", moveTime);
            hash.Add("times", times);
            hash.Add("islocal", islocal);
            hash.Add("returnInitPos", returnInitPos);
            hash.Add("delay", delay);

            Vector3 moveInitPos;
            if (islocal) moveInitPos = obj.transform.localPosition;
            else moveInitPos = obj.transform.position;
            hash.Add("moveInitPosX", moveInitPos.x);
            hash.Add("moveInitPosY", moveInitPos.y);

            MoveRandomLoop(hash);
        }

        private void MoveRandomLoop(Hashtable hash) {

            GameObject obj = hash["obj"] as GameObject;
            float moveValue = float.Parse(hash["moveValue"].ToString());
            float moveTime = float.Parse(hash["moveTime"].ToString());
            float times = float.Parse(hash["times"].ToString());
            bool islocal = bool.Parse(hash["islocal"].ToString());
            Vector3 moveInitPos;
            moveInitPos.x = float.Parse(hash["moveInitPosX"].ToString());
            moveInitPos.y = float.Parse(hash["moveInitPosY"].ToString());
            bool returnInitPos = bool.Parse(hash["returnInitPos"].ToString());
            float delay = float.Parse(hash["delay"].ToString());
            hash["delay"] = 0;

            if (times >= 0) {
                if (times == 0) {
                    if (returnInitPos) {
                        iTween.MoveTo(obj,
                            iTween.Hash(
                                "x", moveInitPos.x,
                                "y", moveInitPos.y,
                                "time", moveTime,
                                "islocal", islocal,
                                "EaseType", iTween.EaseType.easeInOutQuart
                            )
                        );
                    } else {
                        iTween.Stop(obj, "move");
                    }

                    return;
                }
                times--;
                hash["times"] = times;
            }

            float moveX = moveInitPos.x + UnityEngine.Random.Range(-moveValue, moveValue);
            float moveY = moveInitPos.y + UnityEngine.Random.Range(-moveValue, moveValue);

            iTween.MoveTo(obj,
                iTween.Hash(
                    "x", moveX,
                    "y", moveY,
                    "time", moveTime,
                    "delay", delay,
                    "islocal", islocal,
                    "EaseType", iTween.EaseType.easeInOutQuart,
                    "oncomplete", "MoveRandomLoop",
                    "oncompleteparams", hash,
                    "oncompletetarget", gameObject
                )
            );
        }

        public void StopMoveRandom(GameObject obj) {
            iTween.Stop(obj, "move");
        }
        #endregion

        #region MoveShuffle
        public void MoveShuffle(GameObject obj, float moveValue, float moveTime, float delay, bool islocal = true, bool returnInitPos = false) {

            iTween.ShakePosition(obj,
                iTween.Hash(
                    "x", moveValue,
                    "y", moveValue,
                    "time", moveTime,
                    "delay", delay,
                    "islocal", islocal,
                    "EaseType", iTween.EaseType.easeInOutQuart
                )
            );
        }

        public void StopMoveShuffle(GameObject obj) {
            iTween.Stop(obj, "shake");
        }
        #endregion

        #region MoveRotate
        public void MoveRotate(GameObject obj, float moveAngle, float moveTime, float delay, int times = -1, bool returnInitPos = false) {

            Hashtable hash = new Hashtable();
            hash.Add("obj", obj);
            hash.Add("moveAngle", moveAngle);
            hash.Add("moveTime", moveTime);
            hash.Add("times", times * 2);
            hash.Add("isPlus", true);
            hash.Add("returnInitPos", returnInitPos);
            hash.Add("delay", delay);

            MoveRotateLoop(hash);
        }

        private void MoveRotateLoop(Hashtable hash) {

            GameObject obj = hash["obj"] as GameObject;
            float moveAngle = float.Parse(hash["moveAngle"].ToString());
            float moveTime = float.Parse(hash["moveTime"].ToString());
            float times = float.Parse(hash["times"].ToString());
            bool isPlus = bool.Parse(hash["isPlus"].ToString());
            bool returnInitPos = bool.Parse(hash["returnInitPos"].ToString());
            float delay = float.Parse(hash["delay"].ToString());
            hash["delay"] = 0;

            if (times >= 0) {
                if (times == 0) {
                    if (returnInitPos) {
                        iTween.RotateTo(obj,
                            iTween.Hash(
                                "z", 0,
                                "time", moveTime,
                                "EaseType", iTween.EaseType.easeInOutQuart
                            )
                        );
                    } else {
                        iTween.Stop(obj, "rotate");
                    }

                    return;
                }
                times--;
                hash["times"] = times;
            }

            int plusMinus = 1;
            if (!isPlus) plusMinus = -1;
            isPlus = !isPlus;
            hash["isPlus"] = isPlus;

            iTween.RotateTo(obj,
                iTween.Hash(
                    "z", moveAngle * plusMinus,
                    "time", moveTime,
                    "delay", delay,
                    "EaseType", iTween.EaseType.easeInOutQuart,
                    "oncomplete", "MoveRotateLoop",
                    "oncompleteparams", hash,
                    "oncompletetarget", gameObject
                )
            );
        }

        public void StopMoveRotate(GameObject obj) {
            iTween.Stop(obj, "rotate");
        }
        #endregion

        #region MoveScale
        public void MoveScale(GameObject obj, float scaleValue, float moveTime, float delay, int times = -1, bool returnInitPos = false) {

            Hashtable hash = new Hashtable();
            hash.Add("obj", obj);
            hash.Add("scaleValue", scaleValue);
            hash.Add("moveTime", moveTime);
            hash.Add("times", times * 2);
            hash.Add("isPlus", true);
            hash.Add("returnInitPos", returnInitPos);
            hash.Add("initScale", obj.transform.localScale.x);
            hash.Add("delay", delay);

            MoveScaleLoop(hash);
        }

        private void MoveScaleLoop(Hashtable hash) {

            GameObject obj = hash["obj"] as GameObject;
            float scaleValue = float.Parse(hash["scaleValue"].ToString());
            float moveTime = float.Parse(hash["moveTime"].ToString());
            float times = float.Parse(hash["times"].ToString());
            bool isPlus = bool.Parse(hash["isPlus"].ToString());
            bool returnInitPos = bool.Parse(hash["returnInitPos"].ToString());
            float initScale = float.Parse(hash["initScale"].ToString());
            float delay = float.Parse(hash["delay"].ToString());
            hash["delay"] = 0;

            if (times >= 0) {
                if (times == 0) {
                    if (returnInitPos) {
                        iTween.ScaleTo(obj,
                            iTween.Hash(
                                "x", initScale,
                                "y", initScale,
                                "time", moveTime,
                                "EaseType", iTween.EaseType.easeInOutQuart
                            )
                        );
                    } else {
                        iTween.Stop(obj, "scale");
                    }

                    return;
                }
                times--;
                hash["times"] = times;
            }

            float thisScale = scaleValue;
            if (!isPlus) thisScale = 1 / scaleValue;
            isPlus = !isPlus;
            hash["isPlus"] = isPlus;


            iTween.ScaleTo(obj,
                iTween.Hash(
                    "x", initScale * thisScale,
                    "y", initScale * thisScale,
                    "time", moveTime,
                    "delay", delay,
                    "EaseType", iTween.EaseType.easeInOutQuart,
                    "oncomplete", "MoveScaleLoop",
                    "oncompleteparams", hash,
                    "oncompletetarget", gameObject
                )
            );
        }

        public void StopMoveScale(GameObject obj) {
            iTween.Stop(obj, "scale");
        }
        #endregion

    }

}
