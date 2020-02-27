using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Syy.Utility
{
    /// <summary>
    /// Overridable UnityProvide CustomEditor
    /// #Usage 
    /// 1. Create C# file. And set CustomEditor Attribute
    /// 2. Extends this class 
    /// 3. Override EditorType property
    /// </summary>
    public abstract class UnityProvideEditor : Editor
    {
        MethodInfo _onSceneGUI;
        MethodInfo _onHeaderGUI;
        Editor _provideEditor;

        protected abstract UnityProvideEditorType EditorType { get; }

        protected virtual void OnEnable()
        {
            var assembly = Assembly.GetAssembly(typeof(Editor));
            var editorTypeName = EditorType.ToString();
            var provideEditorType = assembly.GetTypes().Where(type => type.Name == editorTypeName).FirstOrDefault();
            if (provideEditorType == null)
            {
                throw new Exception("Can not find EditorType. type={editorTypeName}");
            }

            var provideCustomEditorType = GetCustomEditorType(provideEditorType);
            var customEditorType = GetCustomEditorType(this.GetType());
            if (provideCustomEditorType == null || customEditorType == null || provideCustomEditorType != customEditorType)
            {
                throw new Exception("editor type is {provideCustomEditorType}. but CustomEditorType is {customEditorType}");
            }

            _onSceneGUI = GetMethodInfo(provideEditorType, "OnSceneGUI");
            _onHeaderGUI = GetMethodInfo(provideEditorType, "OnHeaderGUI");
            if (_onSceneGUI == null)
                throw new Exception("_onSceneGUI is null");
            if (_onHeaderGUI == null)
                throw new Exception("_onHeaderGUI is null");
            _provideEditor = Editor.CreateEditor(targets, provideEditorType);
            if (_provideEditor == null)
                throw new Exception("_provideEditor is null");
        }

        Type GetCustomEditorType(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
            var attribute = attributes.FirstOrDefault();
            if (attribute == null)
            {
                return null;
            }

            var field = attribute.GetType().GetField("m_InspectedType", BindingFlags.NonPublic | BindingFlags.Instance);
            return field.GetValue(attribute) as Type;
        }

        MethodInfo GetMethodInfo(Type type, string method)
        {
            var methodInfo = type.GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            return methodInfo;
        }

        public override void DrawPreview(Rect previewArea)
        {
            _provideEditor.DrawPreview(previewArea);
        }

        public override string GetInfoString()
        {
            return _provideEditor.GetInfoString();
        }

        public override GUIContent GetPreviewTitle()
        {
            return _provideEditor.GetPreviewTitle();
        }

        public override bool HasPreviewGUI()
        {
            return _provideEditor.HasPreviewGUI();
        }

        public override void OnInspectorGUI()
        {
            _provideEditor.OnInspectorGUI();
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            _provideEditor.OnInteractivePreviewGUI(r, background);
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            _provideEditor.OnPreviewGUI(r, background);
        }

        public override void OnPreviewSettings()
        {
            _provideEditor.OnPreviewSettings();
        }

        public override void ReloadPreviewInstances()
        {
            _provideEditor.ReloadPreviewInstances();
        }

        public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
        {
            return _provideEditor.RenderStaticPreview(assetPath, subAssets, width, height);
        }

        public override bool RequiresConstantRepaint()
        {
            return _provideEditor.RequiresConstantRepaint();
        }

        public override bool UseDefaultMargins()
        {
            return _provideEditor.UseDefaultMargins();
        }

        protected override void OnHeaderGUI()
        {
            _onHeaderGUI.Invoke(_provideEditor, new object[] { });
        }

        public void OnSceneGUI()
        {
            _onSceneGUI.Invoke(_provideEditor, new object[] { });
        }

        public void ProvideEditorRepaint()
        {
            _provideEditor.Repaint();
        }
    }

    public enum UnityProvideEditorType
    {
        AnchoredJoint2DEditor,
        AnimationClipEditor,
        AnimationEditor,
        AnimatorInspector,
        AnimatorOverrideControllerInspector,
        AssetStoreAssetInspector,
        AudioChorusFilterEditor,
        AudioClipInspector,
        AudioDistortionFilterEditor,
        AudioEchoFilterEditor,
        AudioHighPassFilterEditor,
        AudioImporterInspector,
        AudioLowPassFilterInspector,
        AudioMixerControllerInspector,
        AudioMixerGroupEditor,
        AudioMixerSnapshotControllerInspector,
        AudioReverbFilterEditor,
        AudioReverbZoneEditor,
        AudioSourceInspector,
        AvatarEditor,
        AvatarMaskInspector,
        BillboardAssetInspector,
        BillboardRendererInspector,
        BlendTreeInspector,
        BoxCollider2DEditor,
        BoxColliderEditor,
        CameraEditor,
        CanvasEditor,
        CapsuleColliderEditor,
        CharacterControllerEditor,
        CircleCollider2DEditor,
        ClothInspector,
        Collider2DEditorBase,
        Collider3DEditorBase,
        ColliderEditorBase,
        ColorPresetLibraryEditor,
        CubemapInspector,
        CurvePresetLibraryEditor,
        DefaultAssetInspector,
        DistanceJoint2DEditor,
        DoubleCurvePresetLibraryEditor,
        EdgeCollider2DEditor,
        EditorSettingsInspect, or,
        Effector2DEditor,
        FogEditor,
        FontInspector,
        GameObjectInspector,
        GenericInspector,
        GradientPresetLibraryEditor,
        GraphicsSettingsInspector,
        HingeJoint2DEditor,
        HingeJointEditor,
        Joint2DEditorBase,
        LightEditor,
        LightingEditor,
        LightmapParametersEditor,
        LightProbeGroupInspector,
        LightProbesInspector,
        LineRendererInspector,
        LODGroupEditor,
        MaterialEditor,
        MeshColliderEditor,
        MeshRendererEditor,
        ModelImporterClipEditor,
        ModelImporterEditor,
        ModelImporterModelEditor,
        ModelImporterRigEditor,
        ModelInspector,
        MonoScriptImporterInspector,
        MonoScriptInspector,
        MovieImporterInspector,
        MovieTextureInspector,
        NavMeshAgentInspector,
        NavMeshObstacleInspector,
        OcclusionAreaEditor,
        OcclusionPortalInspector,
        OffMeshLinkInspector,
        OtherRenderingEditor,
        ParticleRendererEditor,
        ParticleSystemInspector,
        Physics2DSettingsInspector,
        PhysicsManagerInspector,
        PlayerSettingsEditor,
        PluginImporterInspector,
        PolygonCollider2DEditor,
        ProceduralMaterialInspector,
        ProceduralTextureInspector,
        QualitySettingsEditor,
        RectTransformEditor,
        ReflectionProbeEditor,
        RendererEditorBase,
        RenderSettingsInspector,
        RenderTextureInspector,
        RigidbodyEditor,
        ScriptExecutionOrderInspector,
        ShaderImporterInspector,
        ShaderInspector,
        ShaderVariantCollectionInspector,
        SkinnedMeshRendererEditor,
        SliderJoint2DEditor,
        SpeedTreeImporterInspector,
        SpeedTreeMaterialInspector,
        SphereColliderEditor,
        SpringJoint2DEditor,
        SpriteInspector,
        SpriteRendererEditor,
        SubstanceImporterInspector,
        TagManagerInspector,
        TerrainInspector,
        TextAssetInspector,
        TextMeshInspector,
        Texture3DInspector,
        TextureImporterInspector,
        TextureInspector,
        TrailRendererInspector,
        TransformInspector,
        TreeEditor,
        TrueTypeFontImporterInspector,
        WebCamTextureInspector,
        WheelColliderEditor,
        WheelJoint2DEditor,
        WindInspector,
    }
}
