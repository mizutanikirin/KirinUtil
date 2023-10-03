using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ToJ
{
	[CustomEditor(typeof(Mask))]
	public class MaskEditor : Editor
	{
		//Supported default shaders through getters
		private Shader spriteDefaultShader;
		private Shader SpriteDefaultShader
		{
			get
			{
				if (!spriteDefaultShader)
					spriteDefaultShader = Shader.Find("Sprites/Default");
				return spriteDefaultShader;
			}
		}

		private Shader particlesDefaultShader;
		private Shader ParticlesDefaultShader
		{
			get
			{
				if (!particlesDefaultShader) particlesDefaultShader = Shader.Find("Particles/Alpha Blended Premultiply");
				return particlesDefaultShader;
			}
		}

		private Shader unlitTransparentShader;
		private Shader UnlitTransparentShader
		{
			get
			{
				if (!unlitTransparentShader)
					unlitTransparentShader = Shader.Find("Unlit/Transparent");
				return unlitTransparentShader;
			}
		}

		private Shader defaultShaderUI;
		private Shader DefaultShaderUI
		{
			get
			{
				if (!defaultShaderUI)
					defaultShaderUI = Shader.Find("UI/Default");
				return defaultShaderUI;
			}
		}

		private Shader defaultFontShaderUI;
		private Shader DefaultFontShaderUI
		{
			get
			{
				if (!defaultFontShaderUI)
					defaultFontShaderUI = Shader.Find("UI/Default Font");
				return defaultFontShaderUI;
			}
		}

		private Shader maskedUnlitShader;
		private Shader MaskedUnlitShader
		{
			get
			{
				if (!maskedUnlitShader)
					maskedUnlitShader = Shader.Find(Mask.MASKED_UNLIT_SHADER);
				return maskedUnlitShader;
			}
		}

		private Shader maskedSpriteShader;
		private Shader MaskedSpriteShader
		{
			get
			{
				if (!maskedSpriteShader)
					maskedSpriteShader = Shader.Find(Mask.MASKED_SPRITE_SHADER);
				return maskedSpriteShader;
			}
		}

		public override void OnInspectorGUI ()
		{
			Mask maskTarget = (Mask)target;
			

			bool changesMade = false;

			if (maskTarget.GetComponents<Mask>().Length > 1)
			{
				GUILayout.Label("More than one instance of Mask attached.\nPlease only use one.");
				return;
			}

			if ((maskTarget.GetComponent<MeshRenderer>().sharedMaterial != null))
			{
				Texture maskTexture = (Texture)EditorGUILayout.ObjectField("Main Tex:", maskTarget.MainTex, typeof(Texture), true);
				if (maskTexture != maskTarget.MainTex)
				{
					maskTarget.MainTex = maskTexture;
					changesMade = true;
				}

				Vector2 tiling = EditorGUILayout.Vector2Field("Tiling:", maskTarget.MainTexTiling);
				if (tiling != maskTarget.MainTexTiling)
				{
					maskTarget.MainTexTiling = tiling;
					changesMade = true;
				}

				Vector2 offset = EditorGUILayout.Vector2Field("Offset:", maskTarget.MainTexOffset);
				if (offset != maskTarget.MainTexOffset)
				{
					maskTarget.MainTexOffset = offset;
					changesMade = true;
				}


				bool isMaskingEnabled = EditorGUILayout.Toggle("Masking Enabled", maskTarget.IsMaskingEnabled);
				if (isMaskingEnabled != maskTarget.IsMaskingEnabled)
				{
					maskTarget.IsMaskingEnabled = isMaskingEnabled;
					changesMade = true;
				}

				bool clampAlphaHorizontally = EditorGUILayout.Toggle("Clamp Alpha Horizontally", maskTarget.ClampAlphaHorizontally);
				if (clampAlphaHorizontally != maskTarget.ClampAlphaHorizontally)
				{
					maskTarget.ClampAlphaHorizontally = clampAlphaHorizontally;
					changesMade = true;
				}

				bool clampAlphaVertically = EditorGUILayout.Toggle("Clamp Alpha Vertically", maskTarget.ClampAlphaVertically);
				if (clampAlphaVertically != maskTarget.ClampAlphaVertically)
				{
					maskTarget.ClampAlphaVertically = clampAlphaVertically;
					changesMade = true;
				}

				float clampingBorder = EditorGUILayout.FloatField("Clamping Border", maskTarget.ClampingBorder);
				if (clampingBorder != maskTarget.ClampingBorder)
				{
					maskTarget.ClampingBorder = clampingBorder;
					changesMade = true;
				}

				bool useMaskAlphaChannel = EditorGUILayout.Toggle("Use Mask Alpha Channel (not RGB)", maskTarget.UseMaskAlphaChannel);
				if (useMaskAlphaChannel != maskTarget.UseMaskAlphaChannel)
				{
					maskTarget.UseMaskAlphaChannel = useMaskAlphaChannel;
					changesMade = true;
				}


				if (!Application.isPlaying)
				{
					bool doDisplayMask = EditorGUILayout.Toggle("Display Mask", maskTarget.displayMaskReference);
					if (maskTarget.displayMaskReference != doDisplayMask)
					{
						maskTarget.displayMaskReference = doDisplayMask;
						changesMade = true;
					}
				}

				if (!Application.isPlaying)
				{
					if (GUILayout.Button("Apply Mask to Siblings in Hierarchy"))
					{
						ApplyMask();
						changesMade = true;
					}
				}
			}
			else
			{
				GUILayout.Label("Please assign Mask-Material to mesh renderer.");
			}

			if (changesMade)
			{
				EditorUtility.SetDirty(target);
			}
		}

		private bool IsSupported3DShader (Shader shader)
		{
			return (shader == UnlitTransparentShader);
		}

		private bool IsSupported2DShader (Shader shader)
		{
			return (shader == SpriteDefaultShader) ||
				   (shader == DefaultShaderUI) ||
				   (shader == DefaultFontShaderUI);
		}

		private List<Material> GetAllReusableMaterials (List<Renderer> renderers)
		{
			List<Material> reusableMaterials = new List<Material>();
			Mask maskTarget = (Mask)target;

			foreach (Renderer mRenderer in renderers)
			{
				if (mRenderer.gameObject != maskTarget.gameObject)
				{
					for (int i = 0; i < mRenderer.sharedMaterials.Length; i++)
					{
						Material material = mRenderer.sharedMaterials[i];

						if (material != null && (material.shader == MaskedUnlitShader || material.shader == MaskedSpriteShader) && !reusableMaterials.Contains(material))
						{
							reusableMaterials.Add(material);
						}
					}
				}
			}

			return reusableMaterials;
		}

		private Material FindSuitableMaskedMaterial (Material nonMaskedMaterial, List<Material> differentReusableMaterials, float isThisTextParam)
		{
			foreach (Material material in differentReusableMaterials)
			{
				if ((nonMaskedMaterial.shader == SpriteDefaultShader) || nonMaskedMaterial.shader == ParticlesDefaultShader || (nonMaskedMaterial.shader == DefaultShaderUI) && (material.shader == MaskedSpriteShader))
				{
					if ((material.name == nonMaskedMaterial.name + " Masked") &&
						(!material.HasProperty("PixelSnap") || !nonMaskedMaterial.HasProperty("PixelSnap") || (material.GetFloat("PixelSnap") == nonMaskedMaterial.GetFloat("PixelSnap"))) &&
						(material.GetFloat("_IsThisText") == isThisTextParam))
					{
						return material;
					}
				}
				else if (nonMaskedMaterial.shader == UnlitTransparentShader && material.shader == MaskedUnlitShader)
				{
					if (material.name == nonMaskedMaterial.name + " Masked" && material.mainTexture == nonMaskedMaterial.mainTexture)
					{
						return material;
					}
				}
				else if (nonMaskedMaterial.shader == DefaultFontShaderUI && material.shader == MaskedSpriteShader)
				{
					if (material.name == nonMaskedMaterial.name + " Masked")
					{
						return material;
					}
				}
			}

			return null;
		}

		private Shader GetMaskedShaderEquivalent (Shader defaultShader)
		{
			if (defaultShader == SpriteDefaultShader || defaultShader == DefaultShaderUI || defaultShader == DefaultFontShaderUI)
			{
				return MaskedSpriteShader;
			}
			if (defaultShader == UnlitTransparentShader)
			{
				return MaskedUnlitShader;
			}
			return defaultShader;
		}

		private void ApplyMaskToSpriteRenderer(SpriteRenderer spriteRenderer, Mask maskTarget)
		{
			Material materialToReplace = spriteRenderer.sharedMaterial;
			if (materialToReplace == null)
			{
				return;
			}

			if (!maskTarget.IsMaskeeMaterial(materialToReplace))
			{
				if (IsSupported2DShader(spriteRenderer.sharedMaterial.shader))
				{
					materialToReplace = maskTarget.SpritesAlphaMaskWorldCoords;
				}
				spriteRenderer.sharedMaterial = materialToReplace;
			}
		}

		private void ApplyMaskToGenericRenderer(Renderer mRenderer, Mask maskTarget, List<Material> originalMaterials, List<Material> newMaterials, List<Material> reusableMaterials)
		{
			List<Material> currSharedMaterials = new List<Material>();

			currSharedMaterials.AddRange(mRenderer.sharedMaterials);

			bool materialsChanged = false;

			for (int i = 0; i < currSharedMaterials.Count; i++)
			{
				Material material = currSharedMaterials[i];
				if (currSharedMaterials[i] == null)
				{
					continue;
				}

				if (!originalMaterials.Contains(material))
				{
					if ((material.shader == UnlitTransparentShader) || (material.shader = SpriteDefaultShader))
					{
						Material reusableMaterial = FindSuitableMaskedMaterial(material, reusableMaterials, 0);

						if (reusableMaterial == null)
						{
							Material newMaterial = new Material(material);

							newMaterial.shader = GetMaskedShaderEquivalent(material.shader);

							newMaterial.name = material.name + " Masked";
							newMaterial.SetTexture("_AlphaTex", maskTarget.MainTex);

							originalMaterials.Add(material);
							newMaterials.Add(newMaterial);

							currSharedMaterials[i] = newMaterial;
							materialsChanged = true;
						}
						else
						{
							currSharedMaterials[i] = reusableMaterial;
							materialsChanged = true;

							reusableMaterial.SetTexture("_AlphaTex", maskTarget.MainTex);
						}
					}
					else if ((material.shader == MaskedSpriteShader) || (material.shader == MaskedUnlitShader))
					{
						if (material.GetTexture("_AlphaTex") != maskTarget.MainTex)
						{
							material.SetTexture("_AlphaTex", maskTarget.MainTex);
						}
					}
				}
				else
				{
					int index = originalMaterials.IndexOf(material);

					currSharedMaterials[i] = newMaterials[index];
					materialsChanged = true;
				}
			}

			if (materialsChanged == true)
			{
				mRenderer.sharedMaterials = currSharedMaterials.ToArray();
			}
		}

		private void ApplyMaskToUIElement (Graphic mGraphic, Mask maskTarget)
		{

			bool maskeeMaterialUsed = maskTarget.IsMaskeeMaterial(mGraphic.material);

			if (IsSupported2DShader(mGraphic.material.shader) || maskeeMaterialUsed)
			{
				UIMaterialModifier modifier = mGraphic.gameObject.GetComponent<UIMaterialModifier>();
				if (modifier == null)
				{
					modifier = mGraphic.gameObject.AddComponent<UIMaterialModifier>();
				}

				if (!maskeeMaterialUsed)
				{
					mGraphic.material = maskTarget.SpritesAlphaMaskWorldCoords;
				}
			}
		}

		private void ApplyMask ()
		{
			//Set predefined maskee mats for UI and sprites (Shaders 2D sprite)
			//Set cloned maskee mats for mesh renderer (Shaders 3D)

			Mask maskTarget = (Mask)target;

			maskTarget.ScheduleFullMaskRefresh();

			if ((MaskedSpriteShader == null) || (MaskedUnlitShader == null))
			{
				Debug.Log("Shaders necessary for masking don't seem to be present in the project.");
				return;
			}

			
			
			
			List<Renderer> maskeeRenderers = new List<Renderer>();
			maskTarget.transform.parent.gameObject.GetComponentsInChildren(true, maskeeRenderers);
			Renderer excludedRenderer = maskTarget.transform.parent.GetComponent<Renderer>();
			if (excludedRenderer != null) maskeeRenderers.Remove(excludedRenderer);

			List<Graphic> maskeeGraphics = new List<Graphic>();
			maskTarget.transform.parent.gameObject.GetComponentsInChildren(true, maskeeGraphics);
			Graphic excludedGraphic = maskTarget.transform.parent.GetComponent<Graphic>();
			if (excludedGraphic != null) maskeeGraphics.Remove(excludedGraphic);


			//Mask materials that already exist within this masks hierarchy
			List<Material> reusableMaterials = GetAllReusableMaterials(maskeeRenderers);

			//Twin lists 
			List<Material> originalMaterials = new List<Material>();
			List<Material> newMaterials = new List<Material>();

			//Non-ui material loop
			foreach (Renderer mRenderer in maskeeRenderers)
			{
				//Don't mask the mask itself
				if (mRenderer.gameObject == maskTarget.gameObject)
				{
					continue;
				}

				if (mRenderer is SpriteRenderer)
				{
					ApplyMaskToSpriteRenderer((SpriteRenderer)mRenderer, maskTarget);
				}
				else
				{
					ApplyMaskToGenericRenderer(mRenderer, maskTarget, originalMaterials, newMaterials, reusableMaterials);
				}
			}

			//UI material loop
			foreach (Graphic mGraphic in maskeeGraphics)
			{
				//Don't mask the mask itself
				if (mGraphic.gameObject == maskTarget.gameObject)
				{
					continue;
				}

				ApplyMaskToUIElement(mGraphic, maskTarget);
			}
			Debug.Log("Mask applied." + (maskTarget.IsMaskingEnabled ? "" : " Have in mind that masking is disabled, so you will not see the effect, until you enable masking!"));
		}
	}

}