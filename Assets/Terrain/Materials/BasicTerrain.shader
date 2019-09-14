// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Shader codes referenced from https://en.wikibooks.org/wiki/Cg_Programming/Unity/Diffuse_Reflection

Shader "Custom/BasicTerrain" {
Properties {

      // colour of terrain elements
      _ColorGrass ("Grass Color", Color) = (1,1,1,1)
      _ColorRock ("Rock Color", Color) = (1,1,1,1)
      _ColorSnow ("Snow Color", Color) = (1,1,1,1)
      
      // height correspond to the right colour
      _RockHeight2 ("Rock Height2", float) = 1.0
      _GrassHeight ("Grass Height", float) = 1.0
      _RockHeight ("Rock Height", float) = 1.0
      _SnowHeight ("Snow Height", float) = 1.0
     
   }
   SubShader {
	  tags { "RenderType" = "Opaque" }
	  LOD 200

      Pass {    
         Tags { "LightMode" = "ForwardBase" } 
            // make sure that all uniforms are correctly set
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"

         uniform float3 _LightColor;
         uniform float3 _LightPosition;
 
         // color of light source (from "Lighting.cginc")
         uniform float4 _Color; // define shader property for shaders
         float4 _ColorGrass;
         float4 _ColorRock;
         float4 _ColorSnow;
         float _GrassHeight;
         float _RockHeight;
         float _RockHeight2;
         float _SnowHeight;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
			   float4 col : COLOR;
         };

         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : COLOR;
            float3 worldNormal : TEXCOORD0;
            float4 worldVertex : TEXCOORD1;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;

            float4 color = float4(0.0, 0.0, 0.0, 0.0);
            float4 clear = float4(0.0, 0.0, 0.0, 1.0);
          
             // assign colour to terrain depending on the height
            if (input.vertex.y < _SnowHeight) {
               color = _ColorSnow;
            }
            if (input.vertex.y < _RockHeight) {
               color = _ColorRock;
            }
            if (input.vertex.y < _GrassHeight) {
               color = _ColorGrass;
            }
            if (input.vertex.y < _RockHeight2) {
               color = _ColorRock;
            }
               
            float4x4 modelMatrix = unity_ObjectToWorld;
            float4x4 modelMatrixInverse = unity_WorldToObject;
               
            // normalize values of vertex
            float3 normalDirection = normalize(
               mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            float4 worldVertex = mul(modelMatrix, input.vertex);
               
            // assign output
            output.col = color;
            output.pos = UnityObjectToClipPos(input.vertex);
            output.worldVertex = worldVertex;
            output.worldNormal = normalDirection;

            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            // Calculate ambient RGB intensities
            float Ka = 2;
            float3 amb = input.col.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

            // Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
            // (when calculating the reflected ray in our specular component)
            float fAtt = 1;
            float Kd = 1;
            float3 L = normalize(_LightPosition - input.worldVertex.xyz);
            float LdotN = dot(L, input.worldNormal.xyz);
            float3 dif = fAtt * _LightColor.rgb * Kd * input.col.rgb * saturate(LdotN);

            // Calculate specular reflections
            float Ks = 0.05;
            float specN = 4; // Values>>1 give tighter highlights
            float3 V = normalize(_WorldSpaceCameraPos - input.worldVertex.xyz);
            float3 R = normalize((2.0 * LdotN * input.worldNormal) - L);
            float3 spe = fAtt * _LightColor.rgb * Ks * pow(saturate(dot(V, R)), specN);

            // Combine Phong illumination model components
            input.col.rgb = amb.rgb + dif.rgb + spe.rgb;

               return input.col;
         }
 
         ENDCG
      }
   }
   Fallback "Diffuse"
}