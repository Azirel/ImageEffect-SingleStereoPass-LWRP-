Shader "SupGames/MobileBloomLWRP"
{
	HLSLINCLUDE

	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
	
	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	TEXTURE2D_SAMPLER2D(_BloomTex, sampler_BloomTex);

	half _BloomAmount;
	half _BlurAmount;
	half _FadeAmount;
	half2 _Size;
	uniform half4 _MainTex_TexelSize;

	
	struct v2f {
		half4 pos : POSITION;
		half2 uv  : TEXCOORD0;
	};

	struct v2fb
	{
		half4 pos  : SV_POSITION;
		half4  uv1 : TEXCOORD0;
		half4  uv2 : TEXCOORD1;
	};

	v2f Vert(AttributesDefault v)
	{
		v2f o;
		o.pos = half4(v.vertex.xy, 0.0, 1.0);
		o.uv = TransformTriangleVertexToUV(v.vertex.xy);
		return o;
	}

	v2fb Vertb(AttributesDefault v)
	{
		v2fb o;
		o.pos = half4(v.vertex.xy, 0.0, 1.0);
		half2 uv = TransformTriangleVertexToUV(v.vertex.xy);
		o.uv1.xy = uv + _MainTex_TexelSize.xy * half2(0.5, 0.5) *  _BlurAmount;
		o.uv1.zw = uv + _MainTex_TexelSize.xy * half2(0.5, -0.5)*_BlurAmount;
		o.uv2.xy = uv + _MainTex_TexelSize.xy * half2(-0.5, 0.5) *  _BlurAmount;
		o.uv2.zw = uv + _MainTex_TexelSize.xy * half2(-0.5, -0.5)*_BlurAmount;
		return o;
	}

	half4 Fragm(v2fb i) : SV_Target
	{
		//i.uv = TransformStereoScreenSpaceTex(i.uv, 1);
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv1.xy));
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv1.zw));
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv2.xy));
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv2.zw));
		return max(color * 0.25h - _FadeAmount, 0.0h);
	}

	half4 Fragb(v2fb i) : SV_Target
	{
		//i.uv = TransformStereoScreenSpaceTex(i.uv, 1);
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv1.xy));
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv1.zw));
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv2.xy));
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv2.zw));
		return color * 0.25h;
	}

	half4 Fragc(v2f i) : SV_Target
	{
		//i.uv = TransformStereoScreenSpaceTex(i.uv, 1);
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2(i.uv.x,1.0-i.uv.y));
		half4 bloom = SAMPLE_TEXTURE2D(_BloomTex, sampler_BloomTex, half2(i.uv.x, i.uv.y));
		return color+bloom*_BloomAmount;
	}
	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM

				#pragma vertex Vertb
				#pragma fragment Fragm
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDHLSL
		}

		Pass
		{
			HLSLPROGRAM

				#pragma vertex Vertb
				#pragma fragment Fragb
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDHLSL
		}

		Pass
		{
			HLSLPROGRAM

				#pragma vertex Vert
				#pragma fragment Fragc
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDHLSL
		}
	}
}