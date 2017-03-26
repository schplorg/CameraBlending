Shader "Custom/CameraMix"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RenderTex ("Texture", 2D) = "white" {}
		_Mix("Mix", Range(0.0, 1.0)) = 0.5
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			half4 _MainTex_ST;
			sampler2D _RenderTex;
			half4 _RenderTex_ST;
			float _Mix;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
				fixed4 col2 = tex2D(_RenderTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
				col = lerp(col,col2,_Mix);
				return col;
			}
			ENDCG
		}
	}
}
