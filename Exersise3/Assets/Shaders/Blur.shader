Shader "Hidden/Blur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		// Horizontal
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
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float blurSize;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 s = tex2D(_MainTex, i.uv) * 0.38774;
				s += tex2D(_MainTex, i.uv + float2(blurSize.x *  2, 0)) * 0.06136;
				s += tex2D(_MainTex, i.uv + float2(blurSize.x *  1, 0)) * 0.24477;
				s += tex2D(_MainTex, i.uv + float2(blurSize.x * -1, 0)) * 0.24477;
				s += tex2D(_MainTex, i.uv + float2(blurSize.x * -2, 0)) * 0.06136;

				return s;
			}
			ENDCG
		}
	
		// Vertical
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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
		
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float2 _BlurSize;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 s = tex2D(_MainTex, i.uv) * 0.38774;
				s += tex2D(_MainTex, i.uv + float2(0, _BlurSize.y * 2)) * 0.06136;
				s += tex2D(_MainTex, i.uv + float2(0, _BlurSize.y)) * 0.24477;
				s += tex2D(_MainTex, i.uv + float2(0, _BlurSize.y * -1)) * 0.24477;
				s += tex2D(_MainTex, i.uv + float2(0, _BlurSize.y * -2)) * 0.06136;	

				return s;
			}
			ENDCG
		}
	}
}
