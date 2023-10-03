// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Alpha Masked/Unlit Alpha Masked - World Coords"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		[HideInInspector][PerRendererData][Toggle] _Enabled("Mask Enabled", Float) = 1
		[HideInInspector][PerRendererData][Toggle] _ClampHoriz("Clamp Alpha Horizontally", Float) = 0
		[HideInInspector][PerRendererData][Toggle] _ClampVert("Clamp Alpha Vertically", Float) = 0
		[HideInInspector][PerRendererData][Toggle] _UseAlphaChannel("Use Mask Alpha Channel (not RGB)", Float) = 0
		[HideInInspector][PerRendererData][Toggle] _ScreenSpaceUI ("Is this screen space ui element?", Float) = 0
		[HideInInspector][PerRendererData]_AlphaTex("Alpha Mask", 2D) = "white" {}
		[HideInInspector][PerRendererData]_ClampBorder("Clamping Border", Float) = 0.01
	}

		SubShader
		{
			Tags { 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
			"ToJMasked" = "True"
			}
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off Lighting Off ZWrite Off Fog { Color(0, 0, 0, 0) }

			Pass
			{
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "../../ToJAlphaMasking.cginc"

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex; //form properties
			float4 _MainTex_ST;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR;
				float2 uvMain : TEXCOORD1;
				TOJ_MASK_COORDS(2)
			};

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uvMain = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;

				
				TOJ_TRANSFER_MASK(o, v.vertex);				

				return o;
			}

			half4 frag(v2f i) : SV_Target //If using version below 4.5 replace SV_Target with: COLOR
			{
				half4 texcol;

				texcol = tex2D(_MainTex, i.uvMain);
				texcol *= i.color;

				TOJ_APPLY_MASK(i, texcol.a);

				return texcol;
			}

		ENDCG
		}
	}
	Fallback "Unlit/Texture"
}
