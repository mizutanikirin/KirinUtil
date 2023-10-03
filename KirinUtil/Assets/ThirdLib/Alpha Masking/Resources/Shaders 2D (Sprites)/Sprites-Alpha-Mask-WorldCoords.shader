// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Alpha Masked/Sprites Alpha Masked - World Coords"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}

		[HideInInspector][PerRendererData][Toggle] _Enabled ("Mask Enabled", Float) = 1
		[HideInInspector][PerRendererData][Toggle] _ClampHoriz ("Clamp Alpha Horizontally", Float) = 0
		[HideInInspector][PerRendererData][Toggle] _ClampVert ("Clamp Alpha Vertically", Float) = 0
		[HideInInspector][PerRendererData][Toggle] _UseAlphaChannel ("Use Mask Alpha Channel (not RGB)", Float) = 0
		[HideInInspector][PerRendererData][Toggle] _ScreenSpaceUI ("Is this screen space ui element?", Float) = 0
		[HideInInspector][PerRendererData]_AlphaTex ("Alpha Mask", 2D) = "white" {}
		[HideInInspector][PerRendererData]_ClampBorder ("Clamping Border", Float) = 0.01

		//Sprite related data
		[PerRendererData]_Color ("Tint", Color) = (1,1,1,1)
		[PerRendererData][Toggle] _PixelSnap("Pixel snap", Float) = 0
		[PerRendererData]_IsThisText("Is This Text?", Float) = 0
		
		[PerRendererData]_StencilComp ("Stencil Comparison", Float) = 8
		[PerRendererData]_Stencil ("Stencil ID", Float) = 0
		[PerRendererData]_StencilOp ("Stencil Operation", Float) = 0
		[PerRendererData]_StencilWriteMask ("Stencil Write Mask", Float) = 255
		[PerRendererData]_StencilReadMask ("Stencil Read Mask", Float) = 255

		[PerRendererData]_ColorMask ("Color Mask", Float) = 15
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
			"ToJMasked"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha
		ColorMask [_ColorMask]
		
		Pass
		{
		CGPROGRAM
			#include "UnityCG.cginc"
			#include "../../ToJAlphaMasking.cginc"
			
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex; //from properties
			float4 _MainTex_ST; 

			//Sprite rendering related
			float4 _Color; //from properties
			float _PixelSnap; //from propertiess
			float _IsThisText; //from properties

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float2 uvMain : TEXCOORD1;
				TOJ_MASK_COORDS(2)
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uvMain = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color * _Color;
				
				if (_PixelSnap)
					o.pos = UnityPixelSnap(o.pos);

				TOJ_TRANSFER_MASK(o, v.vertex);
				
				return o;
			}

			half4 frag (v2f i) : SV_Target //If using version below 4.5 replace SV_Target with: COLOR
			{
				half4 texcol;

				texcol = tex2D(_MainTex, i.uvMain);

				texcol.a *= i.color.a;
				texcol.rgb = clamp(texcol.rgb + _IsThisText, 0, 1) * i.color.rgb;

				TOJ_APPLY_MASK(i, texcol.a);

				texcol.rgb *= texcol.a;
				
				return texcol;
			}
			
		ENDCG
		}
	}
	Fallback "Unlit/Texture"
}
