Shader "Custom/perlinShader"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [Scale] _Scale ("Noise Scale", range(0,200)) = 8.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float _Scale;

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
            CBUFFER_END

            float2 hash(float2 p)
            {
                p = float2(dot(p, float2(127.1, 311.7)),
                           dot(p, float2(269.5, 183.3)));
                return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
            }

            float2 fade(float2 t)
            {
                return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
            }

            float perlinNoise(float2 p)
            {
                float2 pi = floor(p);
                float2 pf = frac(p);

                float2 g00 = hash(pi + float2(0.0, 0.0));
                float2 g10 = hash(pi + float2(1.0, 0.0));
                float2 g01 = hash(pi + float2(0.0, 1.0));
                float2 g11 = hash(pi + float2(1.0, 1.0));

                float2 d00 = pf - float2(0.0, 0.0);
                float2 d10 = pf - float2(1.0, 0.0);
                float2 d01 = pf - float2(0.0, 1.0);
                float2 d11 = pf - float2(1.0, 1.0);

                float v00 = dot(g00, d00);
                float v10 = dot(g10, d10);
                float v01 = dot(g01, d01);
                float v11 = dot(g11, d11);

                float2 u = fade(pf);

                float nx0 = lerp(v00, v10, u.x);
                float nx1 = lerp(v01, v11, u.x);
                return lerp(nx0, nx1, u.y);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float n = perlinNoise(IN.uv * _Scale);
                n = n * 0.5 + 0.5;
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * float4(n,n,n,1);
                return color;
            }
            ENDHLSL
        }
    }
}
