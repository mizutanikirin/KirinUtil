//#define MovieEnable
//#define QREnable
//#define PrintEnable

using UnityEngine;
using UnityEditor;
using KirinUtil;

[CustomEditor(typeof(Util))]
public class KRNUtilEditor : Editor {

    private bool basicOpen = true;
    private bool optionOpen = true;

    private Util util;
    private GUIStyle titleStyle;
    private GameObject utilObj;

    //----------------------------------
    //  init
    //----------------------------------
    public override void OnInspectorGUI() {

        util = target as Util;
        utilObj = util.gameObject;

        // make style
        titleStyle = new GUIStyle();
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = new Color(0.7058824f, 0.7058824f, 0.7058824f);

        // title
        GUILayout.Label("Add Component", titleStyle);

        // Add Component
        BasicComponent();
        OptionComponent();

    }

    //----------------------------------
    //  Basic Component
    //----------------------------------
    private void BasicComponent() {
        bool isBasicOpen = EditorGUILayout.Foldout(basicOpen, "Basic");

        if (basicOpen != isBasicOpen) {
            basicOpen = isBasicOpen;
        }

        if (isBasicOpen) {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(300));
            {

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("NetManager", GUILayout.Width(150))) {
                        if (utilObj.GetComponent<NetManager>() == null)
                            utilObj.AddComponent<NetManager>();
                    }
                    if (GUILayout.Button("Log", GUILayout.Width(150))) {
                        if (utilObj.GetComponent<Log>() == null)
                            utilObj.AddComponent<Log>();
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("SoundManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("soundManager");
                        if (thisObj.GetComponent<SoundManager>() == null) {
                            Debug.Log("Added SoundManager");
                            thisObj.AddComponent<SoundManager>();
                        }
                    }

                    if (GUILayout.Button("ImageManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("imageManager");
                        if (thisObj.GetComponent<ImageManager>() == null) {
                            Debug.Log("Added ImageManager");
                            thisObj.AddComponent<ImageManager>();
                        }
                    }
                }
                GUILayout.EndHorizontal();

#if MovieEnable
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("MovieManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("movieManager");
                        if (thisObj.GetComponent<MovieManager>() == null) {
                            Debug.Log("Added MovieManager");
                            thisObj.AddComponent<MovieManager>();
                        }
                    }
                }
                GUILayout.EndHorizontal();
#endif
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }
    }

    //----------------------------------
    //  Option Component
    //----------------------------------
    private void OptionComponent() {
        bool isOptionOpen = EditorGUILayout.Foldout(optionOpen, "Option");

        if (optionOpen != isOptionOpen) {
            optionOpen = isOptionOpen;
        }

        if (isOptionOpen) {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(300));
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("CaptureManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("captureManager");
                        if (thisObj.GetComponent<CaptureManager>() == null) {
                            Debug.Log("Added CaptureManager");
                            thisObj.AddComponent<CaptureManager>();
                        }
                    }

#if QREnable
                    if (GUILayout.Button("QRManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("qrManager");
                        if (thisObj.GetComponent<QRManager>() == null) {
                            Debug.Log("Added QRManager");
                            thisObj.AddComponent<QRManager>();
                        }
                    }
#endif
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("BalloonMessageManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("balloonMessageManager");
                        if (thisObj.GetComponent<BalloonMessageManager>() == null) {
                            Debug.Log("Added BalloonMessageManager");
                            thisObj.AddComponent<BalloonMessageManager>();
                        }
                    }
                    if (GUILayout.Button("DialogManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("dialogManager");
                        if (thisObj.GetComponent<DialogManager>() == null) {
                            Debug.Log("Added DialogManager");
                            thisObj.AddComponent<DialogManager>();
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("SlideManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("slideManager");
                        if (thisObj.GetComponent<SlideManager>() == null) {
                            Debug.Log("Added SlideManager");
                            thisObj.AddComponent<SlideManager>();
                        }
                    }
                    if (GUILayout.Button("ProcessManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("processManager");
                        if (thisObj.GetComponent<ProcessManager>() == null) {
                            Debug.Log("Added ProcessManager");
                            thisObj.AddComponent<ProcessManager>();
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("CountDown", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("countDown");
                        if (thisObj.GetComponent<CountDown>() == null) {
                            Debug.Log("Added CountDown");
                            thisObj.AddComponent<CountDown>();
                        }
                    }
#if PrintEnable
                    if (GUILayout.Button("PrintManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("printManager");
                        if (thisObj.GetComponent<PrintManager>() == null) {
                            Debug.Log("Added PrintManager");
                            thisObj.AddComponent<PrintManager>();
                        }
                    }
#endif
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("UDPSendManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("udpManager");
                        if (thisObj.GetComponent<UDPSendManager>() == null) {
                            Debug.Log("Added UDPSendManager");
                            thisObj.AddComponent<UDPSendManager>();
                        }
                    }
                    if (GUILayout.Button("UDPReceiveManager", GUILayout.Width(150))) {
                        GameObject thisObj = ExistComponent("udpManager");
                        if (thisObj.GetComponent<UDPReceiveManager>() == null) {
                            Debug.Log("Added UDPReceiveManager");
                            thisObj.AddComponent<UDPReceiveManager>();
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }
    }


    //----------------------------------
    //  Hierarchy
    //----------------------------------
    [MenuItem("GameObject/KirinUtil/Group Object", false, 0)]
    public static void CreateGroupObj()
    {
        GameObject obj = new GameObject();
        obj.name = "Group";
        obj.transform.position = Vector3.zero;

        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.transform.parent != null)
            {
                obj.transform.SetParent(Selection.activeGameObject.transform.parent);
            }
        }

        Undo.RegisterCreatedObjectUndo(obj, "Create Group");
    }

    [MenuItem("GameObject/KirinUtil/GroupUI Object", false, 0)]
    public static void CreateGroupUI()
    {
        GameObject obj = new GameObject();
        obj.name = "GroupUI";
        RectTransform trf = obj.AddComponent<RectTransform>();

        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.transform.parent != null)
            {
                obj.transform.SetParent(Selection.activeGameObject.transform.parent);
            }
        }
        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.Euler(Vector3.zero);
        obj.transform.localScale = Vector3.one;
        trf.sizeDelta = Vector2.zero;

        Undo.RegisterCreatedObjectUndo(obj, "Create GroupUI");
    }

    //----------------------------------
    //  function
    //----------------------------------
    private GameObject ExistComponent(string componentName) {
        Transform trf = utilObj.transform.Find(componentName);

        if (trf != null) {
            return trf.gameObject;
        } else {
            GameObject obj = new GameObject();
            obj.name = componentName;
            obj.transform.SetParent(utilObj.transform, false);
            return obj;
        }
    }

}
