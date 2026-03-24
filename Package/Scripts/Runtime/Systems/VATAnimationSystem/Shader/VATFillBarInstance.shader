Shader "D-Dev/VAT/FillBarInstance"
{
    Properties
    {
        _MainColor ("Health Color", Color) = (0.1, 0.8, 0.1, 1)
        _DamageColor ("Damage Color", Color) = (1, 0.9, 0, 1)
        _BackColor ("Back Color", Color) = (0.1, 0.1, 0.1, 1)
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
        
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _Fill)
                UNITY_DEFINE_INSTANCED_PROP(float, _DelayedFill)
            UNITY_INSTANCING_BUFFER_END(Props)

            float4 _MainColor;
            float4 _DamageColor;
            float4 _BackColor;

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                float3 worldPos = GetObjectToWorldMatrix()._m03_m13_m23;
                float3 viewPos = TransformWorldToView(worldPos);
                
                float2 scale = float2(length(GetObjectToWorldMatrix()._m00_m10_m20), 
                                     length(GetObjectToWorldMatrix()._m01_m11_m21));
                
                float3 pos = viewPos + float3(input.positionOS.xy * scale, 0);
                output.positionCS = TransformWViewToHClip(pos);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                float fill = UNITY_ACCESS_INSTANCED_PROP(Props, _Fill);
                float dFill = UNITY_ACCESS_INSTANCED_PROP(Props, _DelayedFill);

                half4 col = _BackColor;
                if (input.uv.x < fill)
                    col = _MainColor;
                else if (input.uv.x < dFill)
                    col = _DamageColor;

                return col;
            }
            ENDHLSL
        }
    }
}
