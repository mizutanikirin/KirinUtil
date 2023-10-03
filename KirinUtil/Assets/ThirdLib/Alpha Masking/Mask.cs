using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToJ
{
	[ExecuteInEditMode]
	[AddComponentMenu("Alpha Mask")]
	[RequireComponent(typeof(MeshRenderer))]
	public class Mask : MonoBehaviour
	{
		//Can't rename because of version compatibility
		[SerializeField]
		private bool _isMaskingEnabled = true;
		[SerializeField]
		private bool _clampAlphaHorizontally = false;
		[SerializeField]
		private bool _clampAlphaVertically = false;
		[SerializeField]
		private float _clampingBorder = 0.01f;
		[SerializeField]
		private bool _useMaskAlphaChannel = false;
		[SerializeField]
		private Texture mainTex;
		[SerializeField]
		private Vector2 mainTexTiling = new Vector2(1, 1);
		[SerializeField]
		private Vector2 mainTexOffset = new Vector2(0, 0);

		public bool IsMaskingEnabled
		{
			get
			{
				return _isMaskingEnabled;
			}
			set
			{
				if (value != _isMaskingEnabled)
				{
					ScheduleFullMaskRefresh();
					_isMaskingEnabled = value;
				}
			}
		}

		public bool ClampAlphaHorizontally
		{
			get
			{
				return _clampAlphaHorizontally;
			}
			set
			{
				if (value != _clampAlphaHorizontally)
				{
					ScheduleFullMaskRefresh();
					_clampAlphaHorizontally = value;
				}
			}
		}

		public bool ClampAlphaVertically
		{
			get
			{
				return _clampAlphaVertically;
			}
			set
			{
				if (value != _clampAlphaVertically)
				{
					ScheduleFullMaskRefresh();
					_clampAlphaVertically = value;
				}
			}
		}

		public float ClampingBorder
		{
			get
			{
				return _clampingBorder;
			}
			set
			{
				if (value != _clampingBorder)
				{
					ScheduleFullMaskRefresh();
					_clampingBorder = value;
				}
			}
		}

		public bool UseMaskAlphaChannel
		{
			get
			{
				return _useMaskAlphaChannel;
			}
			set
			{
				if (value != _useMaskAlphaChannel)
				{
					ScheduleFullMaskRefresh();
					_useMaskAlphaChannel = value;
				}
			}
		}

		public Texture MainTex
		{
			get
			{
				return mainTex;
			}
			set
			{
				if (value != mainTex)
				{
					ScheduleFullMaskRefresh();
					mainTex = value;
				}
			}
		}

		public Vector2 MainTexTiling
		{
			get
			{
				return mainTexTiling;
			}
			set
			{
				if (value != mainTexTiling)
				{
					ScheduleFullMaskRefresh();
					mainTexTiling = value;
				}
			}
		}
		public Vector2 MainTexOffset
		{
			get
			{
				return mainTexOffset;
			}
			set
			{
				if (value != mainTexOffset)
				{
					ScheduleFullMaskRefresh();
					mainTexOffset = value;
				}
			}
		}

		private bool fullMaskRefresh = true;
		public void ScheduleFullMaskRefresh ()
		{
			fullMaskRefresh = true;
		}

		private Matrix4x4 oldWorldToMask = Matrix4x4.identity;
		private Matrix4x4 OldWorldToMask
		{
			get
			{
				return oldWorldToMask;
			}
			set
			{
				oldWorldToMask = value;
			}
		}

		//Must remaint for version upgrades
		private Shader defaultMaskedSpriteShader = null;
		private Shader DefaultMaskedSpriteShader
		{
			get
			{
				if (defaultMaskedSpriteShader == null) defaultMaskedSpriteShader = Shader.Find(MASKED_SPRITE_SHADER);
				return defaultMaskedSpriteShader;
			}
			set
			{
				defaultMaskedSpriteShader = value;
			}
		}

		//Must remain for version upgrades
		private Shader defaultMaskedUnlitShader = null;
		private Shader DefaultMaskedUnlitShader
		{
			get
			{
				if (defaultMaskedUnlitShader == null) defaultMaskedUnlitShader = Shader.Find(MASKED_UNLIT_SHADER);
				return defaultMaskedUnlitShader;
			}
			set
			{
				defaultMaskedUnlitShader = value;
			}
		}
		private MeshRenderer meshRenderer;
		private MeshRenderer MeshRenderer
		{
			get
			{
				if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
				return meshRenderer;
			}
		}


		private Material spritesAlphaMaskWorldCoords;
		private const string SPRITES_RESOURCE_ADDRESS = "Materials/Sprites-Alpha-Mask-WorldCoords";
		public const string MASKED_SPRITE_SHADER = "Alpha Masked/Sprites Alpha Masked - World Coords";
		public const string MASKED_UNLIT_SHADER = "Alpha Masked/Unlit Alpha Masked - World Coords";


		public Material SpritesAlphaMaskWorldCoords
		{
			get
			{
				if (spritesAlphaMaskWorldCoords == null)
				{
					spritesAlphaMaskWorldCoords = Resources.Load<Material>(SPRITES_RESOURCE_ADDRESS);
					if (spritesAlphaMaskWorldCoords == null)
					{
						Debug.LogError(SPRITES_RESOURCE_ADDRESS + " not found!");
					}
				}
				return spritesAlphaMaskWorldCoords;
			}
			set
			{
				spritesAlphaMaskWorldCoords = value;
			}
		}

		private Material maskMaterial;
		private const string MASK_RESOURCE_ADDRESS = "Materials/Mask-Material";
		public Material MaskMaterial
		{
			get
			{
				if (maskMaterial == null)
				{
					maskMaterial = Resources.Load<Material>(MASK_RESOURCE_ADDRESS);
					if (maskMaterial == null)
					{
						Debug.LogError(MASK_RESOURCE_ADDRESS + " not found!");
					}
				}
				return maskMaterial;
			}
			set
			{
				maskMaterial = value;
			}
		}



		//Assigning starter value here, throws error of initializing property block not in main thread
		private MaterialPropertyBlock maskeePropertyBlock;
		public MaterialPropertyBlock MaskeePropertyBlock
		{
			get
			{
				if (maskeePropertyBlock == null)
				{
					maskeePropertyBlock = new MaterialPropertyBlock();
				}
				return maskeePropertyBlock;
			}
			set
			{
				maskeePropertyBlock = value;
			}
		}

		private MaterialPropertyBlock maskPropertyBlock;
		public MaterialPropertyBlock MaskPropertyBlock
		{
			get
			{
				if (maskPropertyBlock == null)
				{
					maskPropertyBlock = new MaterialPropertyBlock();
				}
				return maskPropertyBlock;
			}
			set
			{
				maskPropertyBlock = value;
			}
		}

		public List<Material> createdMatsStorage = new List<Material>();


#if UNITY_EDITOR
		private Matrix4x4 maskQuadMatrix = Matrix4x4.identity;
		private bool freshlyInitialized = true;


		//Draws the displayMask image, unselectable
		private void OnRenderObject () //Unity Event
		{
			DrawReferenceMask(out maskQuadMatrix);
		}

		private void OnValidate () //Unity Event
		{
			if (maskVersion == 0)
			{
				VersionUpgradeMask(false);
				maskVersion = 1;
			}

			Event e = Event.current;
			if (e != null && !string.IsNullOrEmpty(e.commandName))
			{
				// freshlyInitialized is used to avoid false flags when pasting values into component fields
				bool componentPaste = (e.commandName == "Paste" && freshlyInitialized);

				if (e.commandName == "Duplicate" || componentPaste)
				{
					DuplicateMaskedMaterials();
					CoreInitialization();
				}
			}
			freshlyInitialized = false;


			RefreshMaskPropertyBlock();
			UpdateMasking();
		}

		private void Reset () //Unity Event
		{
			CoreInitialization();

			if (maskVersion == 0)
			{
				VersionUpgradeMask();
				maskVersion = 1;
			}
		}

		private void Awake () //Unity Event
		{
			CoreInitialization();

			if (maskVersion == 0)
			{
				VersionUpgradeMask();
				maskVersion = 1;
			}
		}

		private void CoreInitialization ()
		{
			if (MeshRenderer.sharedMaterial == null)
			{
				MeshRenderer.sharedMaterial = MaskMaterial;
				MeshRenderer.enabled = false;
			}

			InitializeMeshRenderer(MeshRenderer);
			ScheduleFullMaskRefresh();
		}
#endif

		void Start ()
		{
			if (Application.isPlaying)
			{
				MeshRenderer.enabled = false;
			}
		}

		public bool IsMaskeeMaterial (Material material)
		{
			string materialTag = material.GetTag("ToJMasked", false, "false").ToLowerInvariant();
			return materialTag.Equals("true");
		}

		private void ClearShaders ()
		{
			DefaultMaskedSpriteShader = null;
			DefaultMaskedUnlitShader = null;
		}

		private void InitializeMeshRenderer (MeshRenderer target)
		{
#if UNITY_5 || UNITY_5_3_OR_NEWER
            target.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
#else
			target.castShadows = false;
#endif
			target.receiveShadows = false;
		}

		void LateUpdate ()
		{

#if UNITY_EDITOR
			if (maskVersion == 0)
			{
				VersionUpgradeMask();
				maskVersion = 1;
			}

			// Fixes broken masks after scene save. Shouldn't impact performance, since updates
			// in edit mode aren't frequent anyway.
			if (!Application.isPlaying)
			{
				ScheduleFullMaskRefresh();
			}
#endif

			RefreshMaskPropertyBlock();
			UpdateMasking();

		}

		private void UpdateInstanciatedMaterials (List<Material> differentMaterials, Matrix4x4 worldToMask)
		{
			foreach (Material material in differentMaterials)
			{
				ValidateShader(material);


				if (IsMaskeeMaterial(material))
				{


					material.SetTexture("_AlphaTex", MainTex);
					//Set calculations
					material.SetTextureOffset("_AlphaTex", MainTexOffset);
					material.SetTextureScale("_AlphaTex", MainTexTiling);

					material.SetFloat("_ClampHoriz", ClampAlphaHorizontally ? 1 : 0);
					material.SetFloat("_ClampVert", ClampAlphaVertically ? 1 : 0);
					material.SetFloat("_UseAlphaChannel", UseMaskAlphaChannel ? 1 : 0);
					material.SetFloat("_Enabled", IsMaskingEnabled ? 1 : 0);
					material.SetFloat("_ClampBorder", ClampingBorder);
					material.SetFloat("_IsThisText", 0);

					material.SetFloat("_ScreenSpaceUI", 0);
					

					material.SetMatrix("_WorldToMask", worldToMask);
				}
			}
		}

		private void UpdateUIMaterials (List<Graphic> differentGraphics, Matrix4x4 worldToMask, Vector4 tilingAndOffset)
		{
			foreach (Graphic graphic in differentGraphics)
			{
#if UNITY_EDITOR
				if (maskeeVersion == 0)
				{
					VersionUpgradeMaskees(graphic);
				}
#endif

				if (IsMaskeeMaterial(graphic.material))
				{
					UIMaterialModifier modifier = graphic.GetComponent<UIMaterialModifier>();
					if (modifier == null)
					{
						modifier = graphic.gameObject.AddComponent<UIMaterialModifier>();
					}

					Canvas currentCanvas = graphic.canvas;
					Matrix4x4 canvasMatrix = Matrix4x4.identity;

					bool renderModeScreenSpace = currentCanvas != null && (currentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ||
							(currentCanvas.renderMode == RenderMode.ScreenSpaceCamera && currentCanvas.worldCamera == null));

					bool enableScreenSpaceUI = false;

					if (renderModeScreenSpace)
					{
						RectTransform canvasRect = currentCanvas.GetComponent<RectTransform>();
						canvasMatrix = Matrix4x4.TRS(canvasRect.rect.size / 2 * currentCanvas.scaleFactor, Quaternion.identity, Vector3.one * currentCanvas.scaleFactor);
						enableScreenSpaceUI = true;
					}

					modifier.SetMaskToMaskee(worldToMask * canvasMatrix, tilingAndOffset, ClampingBorder, IsMaskingEnabled, enableScreenSpaceUI,
						ClampAlphaHorizontally, ClampAlphaVertically, UseMaskAlphaChannel, graphic is Text);
					modifier.UpdateAlphaTex(MainTex);

					modifier.ApplyMaterialProperties();
				}
			}
		}

		private void UpdateSpriteMaterials (List<SpriteRenderer> differentSpriteRenderers, Matrix4x4 worldToMask, Vector4 tilingAndOffset)
		{
			foreach (SpriteRenderer spriteRenderer in differentSpriteRenderers)
			{
#if UNITY_EDITOR
				if (maskeeVersion == 0)
				{
					VersionUpgradeMaskees(spriteRenderer);
				}
#endif

				if (IsMaskeeMaterial(spriteRenderer.sharedMaterial))
				{
					spriteRenderer.GetPropertyBlock(MaskeePropertyBlock);

					if (MainTex != null)
					{
						MaskeePropertyBlock.SetTexture("_AlphaTex", MainTex);
					}


					MaskeePropertyBlock.SetFloat("_ClampHoriz", ClampAlphaHorizontally ? 1 : 0);
					MaskeePropertyBlock.SetFloat("_ClampVert", ClampAlphaVertically ? 1 : 0);
					MaskeePropertyBlock.SetFloat("_UseAlphaChannel", UseMaskAlphaChannel ? 1 : 0);
					MaskeePropertyBlock.SetFloat("_Enabled", IsMaskingEnabled ? 1 : 0);
					MaskeePropertyBlock.SetFloat("_ClampingBorder", ClampingBorder);
					MaskeePropertyBlock.SetFloat("_IsThisText", 0);


					MaskeePropertyBlock.SetVector("_AlphaTex_ST", tilingAndOffset);
					MaskeePropertyBlock.SetMatrix("_WorldToMask", worldToMask);
					// UI "Screen Space - Overlay mode" or "Screen Space - Camera", where the camera is null (actually, falls back to "Overlay")
					//Set calculations
					spriteRenderer.SetPropertyBlock(MaskeePropertyBlock);
				}
			}
		}

		public void UpdateMasking ()
		{
			//Find or create missing components
			if (DefaultMaskedSpriteShader == null || DefaultMaskedUnlitShader == null)
			{
				Debug.Log("Shaders necessary for masking don't seem to be present in the project.");
				return;
			}

			if (transform.parent != null)
			{
				//Per-mask calculations
				Vector4 tilingAndOffset = new Vector4(MainTexTiling.x, MainTexTiling.y, MainTexOffset.x, MainTexOffset.y);

				RectTransform maskRectTransform = GetComponent<RectTransform>();
				Matrix4x4 worldToMask = transform.worldToLocalMatrix;
				Vector3 maskSize = Vector3.one;

				if (maskRectTransform)
				{
					maskSize = maskRectTransform.rect.size;
					maskSize.z = 1;
				}
				maskSize = Vector3.Scale(maskSize, transform.lossyScale);

				worldToMask.SetTRS(transform.position, transform.rotation, maskSize);
				worldToMask = Matrix4x4.Inverse(worldToMask);

				if (worldToMask != OldWorldToMask)
				{
					ScheduleFullMaskRefresh();
				}

				OldWorldToMask = worldToMask;

				if (fullMaskRefresh)
				{
					List<Renderer> renderers = new List<Renderer>();
					transform.parent.gameObject.GetComponentsInChildren(true, renderers);
					Renderer excludedRenderer = transform.parent.GetComponent<Renderer>();
					if (excludedRenderer != null) renderers.Remove(excludedRenderer);


					List<Graphic> UIComponents = new List<Graphic>();
					transform.parent.gameObject.GetComponentsInChildren(true, UIComponents);
					Graphic excludedGraphic = transform.parent.GetComponent<Graphic>();
					if (excludedGraphic != null) UIComponents.Remove(excludedGraphic);


					List<SpriteRenderer> differentSpriteRenderers = new List<SpriteRenderer>();
					List<Graphic> differentGraphics = new List<Graphic>();
					List<Material> differentActiveMaterials = new List<Material>();

					//Get unique renderer materials to differentMaterials list.
					foreach (Renderer renderer in renderers)
					{
						if (renderer is SpriteRenderer)
						{
							if (renderer.gameObject != gameObject && !differentSpriteRenderers.Contains((SpriteRenderer)renderer))
							{
								differentSpriteRenderers.Add((SpriteRenderer)renderer);
							}
						}
						else
						{
							foreach (Material material in renderer.sharedMaterials)
							{
								if (material != null && !differentActiveMaterials.Contains(material))
								{
									differentActiveMaterials.Add(material);
								}
							}
						}
					}


					foreach (Graphic graphic in UIComponents)
					{
						if (graphic.gameObject != gameObject && !differentGraphics.Contains(graphic))
						{
							differentGraphics.Add(graphic);
						}
					}


					UpdateInstanciatedMaterials(differentActiveMaterials, worldToMask);

					UpdateUIMaterials(differentGraphics, worldToMask, tilingAndOffset);

					UpdateSpriteMaterials(differentSpriteRenderers, worldToMask, tilingAndOffset);

					fullMaskRefresh = false;
				}

#if UNITY_EDITOR
				if (maskeeVersion == 0)
				{
					for (int i = 0; i < upgradeDiscards.Count; i++)
					{
						DestroyImmediate(upgradeDiscards[i]);
						upgradeDiscards[i] = null;
					}
					upgradeDiscards = new List<Material>();
					maskeeVersion = 1;
				}
#endif
			}
		}

		private void ValidateShader (Material material)
		{
			if ((material.shader.ToString() == DefaultMaskedSpriteShader.ToString()) &&
				(material.shader.GetInstanceID() != DefaultMaskedSpriteShader.GetInstanceID()))
			{
				Debug.Log("There seems to be more than one masked shader in the project with the same display name, and it's preventing the mask from being properly applied.");
				DefaultMaskedSpriteShader = null;
			}
			if ((material.shader.ToString() == DefaultMaskedUnlitShader.ToString()) &&
				(material.shader.GetInstanceID() != DefaultMaskedUnlitShader.GetInstanceID()))
			{
				Debug.Log("There seems to be more than one masked shader in the project with the same display name, and it's preventing the mask from being properly applied.");
				DefaultMaskedUnlitShader = null;
			}
		}

		private void RefreshMaskPropertyBlock ()
		{
			if (MaskPropertyBlock == null)
			{
				MaskPropertyBlock = new MaterialPropertyBlock();
			}

			MeshRenderer.GetPropertyBlock(MaskPropertyBlock);

			if (MainTex != null)
			{
				MaskPropertyBlock.SetTexture("_MainTex", MainTex);
			}

			MaskPropertyBlock.SetVector("_MainTex_ST", new Vector4(MainTexTiling.x, MainTexTiling.y, MainTexOffset.x, MainTexOffset.y));

			MeshRenderer.SetPropertyBlock(MaskPropertyBlock);
		}



		private void GetMaskQuad (Mesh mesh, Rect r)
		{
			// assign vertices
			Vector3[] vertices = new Vector3[4];

			vertices[0] = new Vector3(r.xMin, r.yMin, 0);
			vertices[1] = new Vector3(r.xMax, r.yMin, 0);
			vertices[2] = new Vector3(r.xMin, r.yMax, 0);
			vertices[3] = new Vector3(r.xMax, r.yMax, 0);

			// assign triangles
			int[] triangles = new int[6];

			//  Lower left triangle.
			triangles[0] = 0;
			triangles[1] = 2;
			triangles[2] = 1;

			//  Upper right triangle.   
			triangles[3] = 2;
			triangles[4] = 3;
			triangles[5] = 1;


			// assign normals
			Vector3[] normals = new Vector3[4];

			normals[0] = -Vector3.forward;
			normals[1] = -Vector3.forward;
			normals[2] = -Vector3.forward;
			normals[3] = -Vector3.forward;

			// assign UVs
			Vector2[] uv = new Vector2[4];

			uv[0] = new Vector2(0, 0);
			uv[1] = new Vector2(1, 0);
			uv[2] = new Vector2(0, 1);
			uv[3] = new Vector2(1, 1);

			if (!BasicArrayCompare(mesh.vertices, vertices))
			{
				mesh.vertices = vertices;
			}
			if (!BasicArrayCompare(mesh.triangles, triangles))
			{
				mesh.triangles = triangles;
			}
			if (!BasicArrayCompare(mesh.normals, normals))
			{
				mesh.normals = normals;
			}
			if (!BasicArrayCompare(mesh.uv, uv))
			{
				mesh.uv = uv;
			}
		}

		private bool BasicArrayCompare<T> (T[] one, T[] two)
		{
			if (one.Length != two.Length)
			{
				return false;
			}
			for (int i = 0; i < one.Length; i++)
			{
				if (!one[i].Equals(two[i]))
				{
					return false;
				}
			}
			return true;
		}

		private List<Material> CollectDifferentMaterials ()
		{
			List<Material> differentMaterials = new List<Material>();
			if (transform.parent == null)
			{
				return differentMaterials;
			}

			List<Renderer> renderers = new List<Renderer>();
			transform.parent.gameObject.GetComponentsInChildren(true, renderers);
			Renderer excludedRenderer = transform.parent.GetComponent<Renderer>();
			if (excludedRenderer != null) renderers.Remove(excludedRenderer);


			foreach (Renderer renderer in renderers)
			{
				if (renderer.gameObject != gameObject)
				{
					foreach (Material material in renderer.sharedMaterials)
					{
						if (!differentMaterials.Contains(material))
						{
							differentMaterials.Add(material);
							Debug.Log(renderer.gameObject.GetComponent<Renderer>().gameObject);
						}
					}
				}
			}

			return differentMaterials;
		}

		//For custom user interaction.
		public void ChangeMaskTexture (Texture texture)
		{
			MainTex = texture;
		}

		public void SetMaskRendererActive (bool value)
		{
			MeshRenderer.enabled = value;
		}

		//For custom user interaction.
		public void DuplicateMaskedMaterials ()
		{
			List<Material> differentMaterials = CollectDifferentMaterials();
			Dictionary<Material, Material> duplicatedDifferentMaterials = new Dictionary<Material, Material>();

			if (differentMaterials.Count == 0)
			{
				return;
			}

			Debug.Log("Different Materials: " + differentMaterials.Count);

			foreach (Material material in differentMaterials)
			{
				if (IsMaskeeMaterial(material))
				{
					Material newMaterial = new Material(material);
					duplicatedDifferentMaterials.Add(material, newMaterial);
				}
			}

			Debug.Log("Duplicate different Materials: " + duplicatedDifferentMaterials.Count);

			foreach (Material material in duplicatedDifferentMaterials.Values)
			{
				Debug.Log("Material ID: " + material.GetInstanceID(), material);
			}

			if (transform.parent == null)
			{
				Debug.Log("Proper mask hierarchy not found.");
				return;
			}

			List<Renderer> renderers = new List<Renderer>();
			transform.parent.gameObject.GetComponentsInChildren(true, renderers);
			Renderer excludedRenderer = transform.parent.GetComponent<Renderer>();
			if (excludedRenderer != null) renderers.Remove(excludedRenderer);

			foreach (Renderer renderer in renderers)
			{
				if (renderer.gameObject != gameObject)
				{
					Material[] materials = new Material[renderer.sharedMaterials.Length];
					for (int i = 0; i < materials.Length; i++)
					{
						if (duplicatedDifferentMaterials.ContainsKey(renderer.sharedMaterials[i]))
						{
							materials[i] = duplicatedDifferentMaterials[renderer.sharedMaterials[i]];
						}
						else
						{
							materials[i] = renderer.sharedMaterials[i];
						}

					}
					renderer.sharedMaterials = materials;

				}
			}

			List<Graphic> UIComponents = new List<Graphic>();
			transform.parent.gameObject.GetComponentsInChildren(true, UIComponents);
			Graphic excludedGraphic = transform.parent.GetComponent<Graphic>();
			if (excludedGraphic != null) UIComponents.Remove(excludedGraphic);

			foreach (Graphic UIComponent in UIComponents)
			{
				if (UIComponent.gameObject != gameObject)
				{
					if (duplicatedDifferentMaterials.ContainsKey(UIComponent.material))
					{
						UIComponent.material = duplicatedDifferentMaterials[UIComponent.material];
					}
				}
			}
		}

#if UNITY_EDITOR
		public bool displayMaskReference
		{
			get
			{
				return MeshRenderer.enabled;
			}
			set
			{
				MeshRenderer.enabled = value;
			}
		}
		public Mesh editorMesh;

		[SerializeField]
		private int maskVersion = 0;
		[SerializeField]
		private int maskeeVersion = 0;

		private List<Material> upgradeDiscards = new List<Material>();

		private void VersionUpgradeMask (bool componentImmediate = true)
		{
			if (MeshRenderer.sharedMaterial.shader == Shader.Find("Unlit/Transparent"))
			{
				if (!MeshRenderer.sharedMaterial.Equals(MaskMaterial))
				{
					ScheduleFullMaskRefresh();
					Debug.Log("Version upgrade on: " + gameObject.name, gameObject);

					MainTex = MeshRenderer.sharedMaterial.GetTexture("_MainTex");
					MainTexOffset = MeshRenderer.sharedMaterial.GetTextureOffset("_MainTex");
					MainTexTiling = MeshRenderer.sharedMaterial.GetTextureScale("_MainTex");

					DestroyImmediate(MeshRenderer.sharedMaterial);
					MeshRenderer.sharedMaterial = MaskMaterial;

					MeshFilter filter = GetComponent<MeshFilter>();
					if (filter)
					{
						if (componentImmediate)
						{
							DestroyImmediate(filter);
						}
						else
						{
							EditorApplication.delayCall += () =>
							{
								DestroyImmediate(filter);
							};
						}

					}

					RefreshMaskPropertyBlock();
				}
			}
		}

		private void VersionUpgradeMaskees (Graphic targetRen)
		{
			if (!AssetDatabase.Contains(targetRen.material))
			{
				//Version upgrade content. Switches old instantiated materials to reference to core material.
				if ((targetRen.material.shader.ToString() == DefaultMaskedSpriteShader.ToString()))
				{
					if (!targetRen.material.Equals(SpritesAlphaMaskWorldCoords))
					{
						ScheduleFullMaskRefresh();
						Debug.Log("Version upgrade on: " + targetRen.gameObject.name, targetRen.gameObject);
						if (!upgradeDiscards.Contains(targetRen.material))
						{
							upgradeDiscards.Add(targetRen.material);
						}
						targetRen.material = SpritesAlphaMaskWorldCoords;
						return;
					}
				}
			}
		}

		private void VersionUpgradeMaskees (SpriteRenderer targetRen)
		{
			if (!AssetDatabase.Contains(targetRen.sharedMaterial))
			{
				if ((targetRen.sharedMaterial.shader.ToString() == DefaultMaskedSpriteShader.ToString()))
				{
					if (!targetRen.sharedMaterial.Equals(SpritesAlphaMaskWorldCoords))
					{
						Debug.Log("Version upgrade on: " + targetRen.gameObject.name, targetRen.gameObject);
						if (!upgradeDiscards.Contains(targetRen.sharedMaterial))
						{
							upgradeDiscards.Add(targetRen.sharedMaterial);
						}
						targetRen.material = SpritesAlphaMaskWorldCoords;
						return;
					}
				}
			}
		}


		private void OnDrawGizmos ()
		{
			//Don't draw quads inside here, otherwise they break selection

			//Draws an invisible gizmo that acts as a selectable area

			Gizmos.matrix = maskQuadMatrix;
			Vector3 outline = Vector3.one;
			outline.z = 0;
			Gizmos.color = new Color(0f, 0f, 0f, 0f);
			Gizmos.DrawCube(new Vector3(0.5f, 0.5f, 0.5f), outline);
		}

		private void DrawReferenceMask (out Matrix4x4 objectMatrix)
		{
			RectTransform rectTransform = GetComponent<RectTransform>();

			Rect texR = new Rect();
			texR.position = MainTexOffset;
			texR.size = MainTexTiling;

			Vector3 objectSize = transform.lossyScale;
			if (rectTransform != null)
			{
				objectSize = Vector3.Scale(objectSize, rectTransform.rect.size);
			}

			objectSize.z = 0.1f; //Because this is mask quad, and it doesn't have thickness

			//Scale difference based on material tiling value
			Vector3 tilingAdjustment = MainTexTiling;
			tilingAdjustment.x = 1 / tilingAdjustment.x;
			tilingAdjustment.y = 1 / tilingAdjustment.y;

			Vector3 objectSizeTilingAdjusted = Vector3.Scale(objectSize, tilingAdjustment);

			Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, objectSizeTilingAdjusted);

			Vector3 pivotTransposition = transform.rotation * -objectSize * .5f;
			Vector3 offsetTransposition = Vector3.Scale(MainTexOffset, -objectSizeTilingAdjusted);

			Matrix4x4 deltaMatrix = Matrix4x4.TRS(pivotTransposition + offsetTransposition, Quaternion.identity, Vector3.one);
			//matrix
			Matrix4x4 completeMatrix = deltaMatrix * matrix;

			if (displayMaskReference)
			{


				if (editorMesh == null)
				{
					editorMesh = new Mesh();
					editorMesh.name = "Mask Editor Mesh";
				}
				Texture originalTex = MeshRenderer.sharedMaterial.GetTexture("_MainTex");
				Vector2 originalOffset = MeshRenderer.sharedMaterial.GetTextureOffset("_MainTex");
				Vector2 originalScale = MeshRenderer.sharedMaterial.GetTextureScale("_MainTex");

				MeshRenderer.sharedMaterial.SetTexture("_MainTex", MainTex);
				MeshRenderer.sharedMaterial.SetTextureOffset("_MainTex", MainTexOffset);
				MeshRenderer.sharedMaterial.SetTextureScale("_MainTex", MainTexTiling);
				if (MeshRenderer.sharedMaterial.SetPass(0))
				{
					GetMaskQuad(editorMesh, texR);

					Graphics.DrawMeshNow(editorMesh, completeMatrix);
				}

				MeshRenderer.sharedMaterial.SetTexture("_MainTex", originalTex);
				MeshRenderer.sharedMaterial.SetTextureOffset("_MainTex", originalOffset);
				MeshRenderer.sharedMaterial.SetTextureScale("_MainTex", originalScale);
			}
			objectMatrix = completeMatrix;
		}
#endif
	}
}