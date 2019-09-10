// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WaveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {

        Pass
        {
			Cull Off

			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;

			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			vertOut vert(vertIn v)
			{
				float4 displacement = float4(0.0f, 0.5f*sin(v.vertex.x + _Time.y), 0.0f, 0.0f);

				v.vertex += displacement;

				vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);
				return col;
			}
            ENDCG
        }
    }
}
