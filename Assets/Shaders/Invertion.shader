Shader "Hidden/Custom/Grayscale"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float _Blend;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            //float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
            //color.rgb = lerp(color.rgb, luminance.xxx, 0.5 );
            return 1-color;
        }

    ENDHLSL

	CGINCLUDE
	#include "UnityCG.cginc"
	UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
	float4 _MainTex_ST;

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	float2 TransformTriangleVertexToUV(float2 vertex)
	{
		float2 uv = (vertex + 1.0) * 0.5;
		return uv;
	}

	v2f vert (appdata v)
	{
		v2f o;
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.uv = TransformTriangleVertexToUV(v.vertex.xy);
		return o;
	}
            
	fixed4 frag (v2f i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		fixed4 col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);
		return 1-col;
	}

	ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        /*Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            ENDHLSL
        }*/

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}
    }
	Fallback off
}