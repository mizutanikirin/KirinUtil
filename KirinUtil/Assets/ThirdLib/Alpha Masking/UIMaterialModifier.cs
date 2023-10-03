using UnityEngine;
using UnityEngine.UI;

//This class exists, because material property block is applied
//to renderer, and thus you can't apply them to unity UI elements
[ExecuteInEditMode]
public class UIMaterialModifier : MonoBehaviour, IMaterialModifier
{
	private Matrix4x4 maskMatrix;
	private Vector4 tilingAndOffset;
	private bool screenSpaceEnabled;

	private Texture alphaTexture;

	private bool maskingEnabled = true;
	private bool isTextMaterial;
	private bool clampHorizontal = true;
	private bool clampVertical = true;
	private float clampingBorder = 0.1f;
	private bool useAlphaChannel = false;

	[SerializeField][HideInInspector]//serializing is to make it stop leaking
	private Material modifiedMaterial;

	private Material lastBaseMaterial;

	[SerializeField][HideInInspector]
	private int instanceID = 0;

	private Image image;
	private Text text;

	void Awake ()
	{
		if (instanceID != GetInstanceID())
		{
			if (instanceID == 0)
			{
				instanceID = GetInstanceID();
			}
			else
			{
				instanceID = GetInstanceID();
				if (instanceID < 0)
				{
					modifiedMaterial = null;
				}
			}
		}
		image = GetComponent<Image>();
		text = GetComponent<Text>();
	}

	private void OnDestroy ()
	{
		DestroyImmediate(modifiedMaterial);
	}

	public void ApplyMaterialProperties (Material target = null)
	{
		if (target == null)
		{
			target = modifiedMaterial;
		}

		if (target != null)
		{
			target.SetMatrix("_WorldToMask", maskMatrix);
			target.SetTexture("_AlphaTex", alphaTexture);
			target.SetVector("_AlphaTex_ST", tilingAndOffset);
			target.SetFloat("_IsThisText", isTextMaterial ? 1 : 0);
			target.SetFloat("_ClampHoriz", clampHorizontal ? 1 : 0);
			target.SetFloat("_ClampVert", clampVertical ? 1 : 0);
			target.SetFloat("_UseAlphaChannel", useAlphaChannel ? 1 : 0);
			target.SetFloat("_Enabled", maskingEnabled ? 1 : 0);
			target.SetFloat("_ClampingBorder", clampingBorder);
			target.SetFloat("_ScreenSpaceUI", screenSpaceEnabled ? 1 : 0);
			if (image) target.SetColor("_Color", image.color);
			if (text) target.SetColor("_Color", text.color);
		}
	}

	//Unity Interface function for similar effect to material property block
	public Material GetModifiedMaterial (Material baseMaterial)
	{
		if (baseMaterial != lastBaseMaterial)
		{
			lastBaseMaterial = baseMaterial;
			if (modifiedMaterial == null)
			{
				modifiedMaterial = new Material(baseMaterial);
				modifiedMaterial.name = baseMaterial.name + " modified";
			}
			else
			{
				modifiedMaterial.CopyPropertiesFromMaterial(baseMaterial);
			}
		}

		ApplyMaterialProperties(modifiedMaterial);
		return modifiedMaterial;
	}

	public void UpdateAlphaTex (Texture alphaTexture)
	{
		if (this.alphaTexture != alphaTexture)
		{
			this.alphaTexture = alphaTexture;
		}
	}

	public void SetMaskToMaskee (Matrix4x4 maskMatrix, Vector4 tilingAndOffset, float clampingBorder, bool maskingEnabled, bool screenSpaceEnabled,
		bool clampHor, bool clampVert, bool useAlphaChannel, bool isTextMaterial)
	{
		this.maskMatrix = maskMatrix;
		this.tilingAndOffset = tilingAndOffset;
		this.screenSpaceEnabled = screenSpaceEnabled;
		this.clampHorizontal = clampHor;
		this.clampVertical = clampVert;
		this.useAlphaChannel = useAlphaChannel;
		this.isTextMaterial = isTextMaterial;
		this.maskingEnabled = maskingEnabled;
		this.clampingBorder = clampingBorder;
	}
}
