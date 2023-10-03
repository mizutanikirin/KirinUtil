// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

#define TOJ_MASK_COORDS(idx) float2 tojAlphaUV : TEXCOORD##idx;
#define TOJ_TRANSFER_MASK(o, vertex) o.tojAlphaUV = VertCalculateMask((vertex))
#define TOJ_APPLY_MASK(i, texa) texa *= FragCalculateMask((i).tojAlphaUV) 


// Copy below (without the //) into your shader "Properties" section

// [HideInInspector][PerRendererData][Toggle] _Enabled("Mask Enabled", Float) = 1
// [HideInInspector][PerRendererData][Toggle] _ClampHoriz("Clamp Alpha Horizontally", Float) = 0
// [HideInInspector][PerRendererData][Toggle] _ClampVert("Clamp Alpha Vertically", Float) = 0
// [HideInInspector][PerRendererData][Toggle] _UseAlphaChannel("Use Mask Alpha Channel (not RGB)", Float) = 0
// [HideInInspector][PerRendererData][Toggle] _ScreenSpaceUI ("Is this screen space ui element?", Float) = 0
// [HideInInspector][PerRendererData]_AlphaTex("Alpha Mask", 2D) = "white" {}
// [HideInInspector][PerRendererData]_ClampBorder("Clamping Border", Float) = 0.01


// Copy below to "Tags" section

// "ToJMasked" = "True"

float _Enabled; //from properties
float _ClampHoriz; //from properties
float _ClampVert; //from properties
float _UseAlphaChannel; //from properties
float _ClampBorder; //from properties
float _ScreenSpaceUI; //from properties

sampler2D _AlphaTex; //from properties
float4 _AlphaTex_ST;

float4x4 _WorldToMask;

inline float2 VertCalculateMask(float4 vertex)
{
    float2 result = float2(0, 0);

    if (_Enabled > 0)
    {
        float4 endVert; //Using endVert and not already existing uvAlpha, because we need to retain y and z.

        if (_ScreenSpaceUI > 0)
        {
            endVert = vertex;
        }
        else
        {
            endVert = mul(unity_ObjectToWorld, vertex);
        }

        result = mul(_WorldToMask, endVert).xy + float2(0.5f, 0.5f);
        result = result * _AlphaTex_ST.xy + _AlphaTex_ST.zw;
    }
    return result;
}

inline float FragCalculateMask(float2 uvAlpha)
{
    float result = 1;
    
    if (_Enabled > 0)
    {
        if (_ClampHoriz)
            uvAlpha.x = clamp(uvAlpha.x, _ClampBorder, 1.0 - _ClampBorder);
					
        if (_ClampVert)
            uvAlpha.y = clamp(uvAlpha.y, _ClampBorder, 1.0 - _ClampBorder);
					
		//Sample uv main
					
        if (_UseAlphaChannel)
            result = tex2D(_AlphaTex, uvAlpha).a;
        else
            result = tex2D(_AlphaTex, uvAlpha).rgb;
    }

    return result;
}