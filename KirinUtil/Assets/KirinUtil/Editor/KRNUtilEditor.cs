//#define MovieEnable
//#define QREnable
//#define PrintEnable

using UnityEngine;
using UnityEditor;
using KirinUtil;
using UnityEngine.UI;

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
        float buttonWidth = (EditorGUIUtility.currentViewWidth - 12f) / 2 - 12f;

        if (basicOpen != isBasicOpen) {
            basicOpen = isBasicOpen;
        }

        if (isBasicOpen) {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(buttonWidth*2));
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("NetManager", GUILayout.Width(buttonWidth))) {
                        if (utilObj.GetComponent<NetManager>() == null) 
                            Undo.AddComponent<NetManager>(utilObj);
                    }

                    GUILayout.Space(2);

                    if (GUILayout.Button("Log", GUILayout.Width(buttonWidth))) {
                        if (utilObj.GetComponent<Log>() == null)
                            Undo.AddComponent<Log>(utilObj);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("SoundManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("soundManager");
                        if (thisObj.GetComponent<SoundManager>() == null) {
                            Debug.Log("Add SoundManager");
                            thisObj.AddComponent<SoundManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add SoundManager");
                    }

                    GUILayout.Space(2);

                    if (GUILayout.Button("ImageManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("imageManager");
                        if (thisObj.GetComponent<ImageManager>() == null) {
                            Debug.Log("Add ImageManager");
                            thisObj.AddComponent<ImageManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add ImageManager");
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("AlphaMediaManager", GUILayout.Width(buttonWidth)))
                    {
                        GameObject thisObj = ExistComponent("alphaMediaManager");
                        if (thisObj.GetComponent<AlphaMediaManager>() == null)
                        {
                            Debug.Log("Add AlphaMediaManager");
                            thisObj.AddComponent<AlphaMediaManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add AlphaMediaManager");
                    }

                    GUILayout.Space(2);

#if MovieEnable
                    if (GUILayout.Button("MovieManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("movieManager");
                        if (thisObj.GetComponent<MovieManager>() == null) {
                            Debug.Log("Add MovieManager");
                            thisObj.AddComponent<MovieManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add MovieManager");
                    }
#endif
                }
                GUILayout.EndHorizontal();

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
        float buttonWidth = (EditorGUIUtility.currentViewWidth - 12f) / 2 - 12f;

        if (optionOpen != isOptionOpen) {
            optionOpen = isOptionOpen;
        }

        if (isOptionOpen) {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(buttonWidth*2));
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("CaptureManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("captureManager");
                        if (thisObj.GetComponent<CaptureManager>() == null) {
                            Debug.Log("Add CaptureManager");
                            thisObj.AddComponent<CaptureManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add CaptureManager");
                    }

#if QREnable
                    GUILayout.Space(2);

                    if (GUILayout.Button("QRManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("qrManager");
                        if (thisObj.GetComponent<QRManager>() == null) {
                            Debug.Log("Add QRManager");
                            thisObj.AddComponent<QRManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add QRManager");
                    }
#endif
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("BalloonMessageManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("balloonMessageManager");
                        if (thisObj.GetComponent<BalloonMessageManager>() == null) {
                            Debug.Log("Add BalloonMessageManager");
                            thisObj.AddComponent<BalloonMessageManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add balloonMessageManager");
                    }

                    GUILayout.Space(2);

                    if (GUILayout.Button("DialogManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("dialogManager");
                        if (thisObj.GetComponent<DialogManager>() == null) {
                            Debug.Log("Add DialogManager");
                            thisObj.AddComponent<DialogManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add DialogManager");
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("SlideManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("slideManager");
                        if (thisObj.GetComponent<SlideManager>() == null) {
                            Debug.Log("Add SlideManager");
                            thisObj.AddComponent<SlideManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add SlideManager");
                    }

                    GUILayout.Space(2);

                    if (GUILayout.Button("ProcessManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("processManager");
                        if (thisObj.GetComponent<ProcessManager>() == null) {
                            Debug.Log("Add ProcessManager");
                            thisObj.AddComponent<ProcessManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add ProcessManager");
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("CountDown", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("countDown");
                        if (thisObj.GetComponent<CountDown>() == null) {
                            Debug.Log("Add CountDown");
                            thisObj.AddComponent<CountDown>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add CountDown");
                    }
#if PrintEnable
                    GUILayout.Space(2);

                    if (GUILayout.Button("PrintManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("printManager");
                        if (thisObj.GetComponent<PrintManager>() == null) {
                            Debug.Log("Add PrintManager");
                            thisObj.AddComponent<PrintManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add PrintManager");
                    }
#endif
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("UDPSendManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("udpManager");
                        if (thisObj.GetComponent<UDPSendManager>() == null) {
                            Debug.Log("Add UDPSendManager");
                            thisObj.AddComponent<UDPSendManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add UDPSendManager");
                    }

                    GUILayout.Space(2);

                    if (GUILayout.Button("UDPReceiveManager", GUILayout.Width(buttonWidth))) {
                        GameObject thisObj = ExistComponent("udpManager");
                        if (thisObj.GetComponent<UDPReceiveManager>() == null) {
                            Debug.Log("Add UDPReceiveManager");
                            thisObj.AddComponent<UDPReceiveManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add UDPReceiveManager");
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("FlickManager", GUILayout.Width(buttonWidth)))
                    {
                        GameObject thisObj = ExistComponent("flickManager");
                        if (thisObj.GetComponent<FlickManager>() == null)
                        {
                            Debug.Log("Add FlickManager");
                            thisObj.AddComponent<FlickManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add FlickManager");
                    }

                    GUILayout.Space(2);

                    if (GUILayout.Button("UIDragManager", GUILayout.Width(buttonWidth)))
                    {
                        GameObject thisObj = ExistComponent("uiDragManager");
                        if (thisObj.GetComponent<UIDragManager>() == null)
                        {
                            Debug.Log("Add UIDragManager");
                            thisObj.AddComponent<UIDragManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add UIDragManager");
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("AssetBundleManager", GUILayout.Width(buttonWidth)))
                    {
                        GameObject thisObj = ExistComponent("assetBundleManager");
                        if (thisObj.GetComponent<AssetBundleManager>() == null)
                        {
                            Debug.Log("Add AssetBundleManager");
                            thisObj.AddComponent<AssetBundleManager>();
                        }
                        Undo.RegisterCreatedObjectUndo(thisObj, "Add AssetBundleManager");
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
    [MenuItem("GameObject/KirinUtil/Add KirinUtil", false, 0)]
    public static void CreateKirinUtilObj()
    {
        GameObject obj = new GameObject();
        obj.name = "KirinUtil";
        obj.transform.position = Vector3.zero;
        obj.transform.SetParent(null);
        obj.AddComponent<Util>();
        Debug.Log("Create KirinUtil");

        Undo.RegisterCreatedObjectUndo(obj, "Create KirinUtil");
    }

    [MenuItem("GameObject/KirinUtil/Group Object", false, 20)]
    public static void CreateGroupObj()
    {
        GameObject obj = CreateBaseObj();
        obj.name = "Group";

        Selection.activeGameObject = obj;
        Debug.Log("Create GroupObj");

        Undo.RegisterCreatedObjectUndo(obj, "Create Group");
    }

    [MenuItem("GameObject/KirinUtil/GroupUI Object", false, 21)]
    public static void CreateGroupUI()
    {
        GameObject obj = CreateBaseObj();
        obj.name = "GroupUI";
        RectTransform trf = obj.AddComponent<RectTransform>();

        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.Euler(Vector3.zero);
        obj.transform.localScale = Vector3.one;
        trf.sizeDelta = Vector2.zero;

        Selection.activeGameObject = obj;
        Debug.Log("Create GroupUI");

        Undo.RegisterCreatedObjectUndo(obj, "Create GroupUI");
    }

    [MenuItem("GameObject/KirinUtil/Button - NoText", false, 22)]
    public static void CreateButton()
    {
        GameObject obj = CreateBaseObj();
        obj.name = "Btn";
        RectTransform trf = obj.AddComponent<RectTransform>();

        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.Euler(Vector3.zero);
        obj.transform.localScale = Vector3.one;
        trf.sizeDelta = new Vector2(100, 100);

        obj.AddComponent<Image>();
        obj.AddComponent<Button>();

        Selection.activeGameObject = obj;
        Debug.Log("Create NoTextButton");

        Undo.RegisterCreatedObjectUndo(obj, "Create NoTextButton");
    }


    [MenuItem("GameObject/KirinUtil/Bold Line", false, 40)]
    public static void CreateBoldLine()
    {
        GameObject obj = CreateBaseObj();
        obj.name = "=====";
        obj.transform.position = Vector3.zero;

        Debug.Log("Create Bold Line");
        Undo.RegisterCreatedObjectUndo(obj, "Create Bold Line");
    }

    [MenuItem("GameObject/KirinUtil/Thin Line", false, 41)]
    public static void CreateThinLine()
    {
        GameObject obj = CreateBaseObj();
        obj.name = "-----";
        obj.transform.position = Vector3.zero;

        Debug.Log("Create Thin Line");
        Undo.RegisterCreatedObjectUndo(obj, "Create Thin Line");
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

    private static GameObject CreateBaseObj()
    {
        GameObject obj = new GameObject();
        obj.transform.position = Vector3.zero;

        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.transform != null)
                obj.transform.SetParent(Selection.activeGameObject.transform);
            else
                obj.transform.SetParent(null);
        }
        else
        {
            obj.transform.SetParent(null);
        }

        return obj;
    }
}
