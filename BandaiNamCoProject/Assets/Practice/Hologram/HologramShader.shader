Shader "Unlit/HologramShader"
{
    Properties
    {
        _HologramCol ("Hologram Color", Color) = (1,1,1,1)
        _LineWidth ("Hologram Line Width", float) = 3
        _WavingStrength ("Pixel Strength", float) = 1
        _Alpha ("Alpha Value", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline"}
        LOD 100

        Pass
        {
            Name "Hologram"
            Tags {
                "LightMode" = "UniversalForward"
            }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back

            ZWrite On
            ZTest LEqual

            HLSLPROGRAM

            #pragma vertex HologramVertex
            #pragma fragment HologramFragment
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
            CBUFFER_START(UnityPerMaterial)
            float4 _HologramCol;
            float _LineWidth;
            float _WavingStrength;
            float _Alpha;
            CBUFFER_END

            struct appdata {
                float4 position : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 positionCS : SV_POSITION;
                float4 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            v2f HologramVertex(appdata i){
                v2f o;
                o.positionCS = TransformObjectToHClip(i.position);
                //float4 wavingPos = float4(i.position.xyz + (i.normal * frac(_Time.y * _WavingStrength)), i.position.w);
                //o.positionCS = TransformObjectToHClip(wavingPos);
                
                o.positionWS = mul(i.position, UNITY_MATRIX_M);
                o.normalWS = TransformObjectToWorldNormal(i.normal);

                return o;
            }

            float4 HologramFragment(v2f o) : SV_TARGET{
                Light mainLight = GetMainLight();
                float ndotl = saturate(dot(mainLight.direction, o.normalWS));
                //ndotl = 1 - ndotl;
                ndotl = ndotl + pow(frac(o.positionWS.y * _LineWidth + _Time.y), 50);
                //ndotl = frac(o.positionWS.y);
                
                return float4(_HologramCol.xyz, ndotl * _Alpha);
            }

            ENDHLSL
        }
    }
}
