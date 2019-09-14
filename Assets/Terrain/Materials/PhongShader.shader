// reference: https://janhalozan.com/2017/08/12/phong-shader/

Shader "PhongShader" {
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

        _Tex ("Pattern", 2D) = "white" {} //Optional texture

        _Shininess ("Shininess", Float) = 10 //Shininess
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1) //Specular highlights color
    }
    SubShader {
        Tags { "RenderType" = "Opaque" } //We're not rendering any transparent objects
        LOD 200 //Level of detail

        Pass {
            Tags { "LightMode" = "ForwardBase" } //For the first light

            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc" //Provides us with light data, camera information, etc

                uniform float4 _LightColor0; //From UnityCG

                sampler2D _Tex; //Used for texture
                float4 _Tex_ST; //For tiling

                // color of light source (from "Lighting.cginc")
                uniform float4 _Color; // define shader property for shaders
                float4 _ColorGrass;
                float4 _ColorRock;
                float4 _ColorSnow;
                float _GrassHeight;
                float _RockHeight;
                float _RockHeight2;
                float _SnowHeight;
                uniform float4 _SpecColor;
                uniform float _Shininess;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                    float4 col : COLOR;
                };

                struct v2f
                {
                    float4 pos : POSITION;
                    float3 normal : NORMAL;
                    float4 col : COLOR;
                    float2 uv : TEXCOORD0;
                    float4 posWorld : TEXCOORD1;
                };

                v2f vert(appdata v)
                {
                    v2f o;

                    float4 color = float4(0.0, 0.0, 0.0, 0.0);
                
                    // assign colour to terrain depending on the height
                    if (v.vertex.y < _SnowHeight) {
                        color = _ColorSnow;
                    }
                    if (v.vertex.y < _RockHeight) {
                        color = _ColorRock;
                    }
                    if (v.vertex.y < _GrassHeight) {
                        color = _ColorGrass;
                    }
                    if (v.vertex.y < _RockHeight2) {
                        color = _ColorRock;
                    }

                    o.posWorld = mul(unity_ObjectToWorld, v.vertex); //Calculate the world position for our point
                    o.normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz); //Calculate the normal
                    o.pos = UnityObjectToClipPos(v.vertex); //And the position
                    o.uv = TRANSFORM_TEX(v.uv, _Tex);
                    o.col = color;

                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {
                    float3 normalDirection = normalize(i.normal);
                    float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);

                    float3 vert2LightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
                    float oneOverDistance = 1.0 / length(vert2LightSource);
                    float attenuation = lerp(1.0, oneOverDistance, _WorldSpaceLightPos0.w); //Optimization for spot lights. This isn't needed if you're just getting started.
                    float3 lightDirection = _WorldSpaceLightPos0.xyz - i.posWorld.xyz * _WorldSpaceLightPos0.w;

                    float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * i.col.rgb; //Ambient component
                    float3 diffuseReflection = attenuation * _LightColor0.rgb * i.col.rgb * max(0.0, dot(normalDirection, lightDirection)); //Diffuse component
                    float3 specularReflection;
                    if (dot(i.normal, lightDirection) < 0.0) //Light on the wrong side - no specular
                    {
                        specularReflection = float3(0.0, 0.0, 0.0);
                	  }
                    else
                    {
                        //Specular component
                        specularReflection = attenuation * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
                    }

                    float3 color = (ambientLighting + diffuseReflection) * tex2D(_Tex, i.uv) + specularReflection; //Texture is not applient on specularReflection
                    return float4(color, 1.0);
                }
            ENDCG
        }
    }
}