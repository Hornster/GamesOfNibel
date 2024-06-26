﻿Shader "PBR Master"
{
    Properties
    {
        [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        [NoScaleOffset]Texture2D_906780E9("MaskTexture", 2D) = "white" {}
        [HDR]Color_D25B2F83("BaseColor", Color) = (2.044308, 7.968627, 7.968627, 1)
        Vector1_5D8C30A("DistortionAmounts", Float) = 0.1
        Vector2_F89F6FDB("DistortionSpeed", Vector) = (0, -0.2, 0, 0)
        Vector1_5C9FEA09("DistortionScale", Float) = 5
        Vector2_18F30DF1("DissolveSpeed", Vector) = (-0.1, -0.5, 0, 0)
        Vector1_444105F2("DissolveScale", Float) = 1.9
        Vector1_45E24A1A("DissolveAngle", Float) = 3.69
        Vector1_B0C7E46C("DissolvePower", Float) = 0.5
        _Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15

        Vector1_CE26F4AA("BrightnessPower", Float) = 10
    }
        SubShader
        {
            Tags
            {
                "RenderPipeline" = "UniversalPipeline"
                "RenderType" = "Transparent"
                "Queue" = "Transparent+0"
            }

            Pass
            {
                Name "Universal Forward"
                Tags
                {
                    "LightMode" = "UniversalForward"
                }

            ZTest[unity_GUIZTestMode]

            Stencil{
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }
            ColorMask[_ColorMask]

            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Off
            ZTest LEqual
            ZWrite Off
            // ColorMask: <None>


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS 
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define SHADERPASS_FORWARD

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 Color_D25B2F83;
            float Vector1_5D8C30A;
            float2 Vector2_F89F6FDB;
            float Vector1_5C9FEA09;
            float2 Vector2_18F30DF1;
            float Vector1_444105F2;
            float Vector1_45E24A1A;
            float Vector1_B0C7E46C;
            float _Stencil;
            float _StencilOp;
            float _StencilComp;
            float _StencilReadMask;
            float _StencilWriteMask;
            float _ColorMask;
            float Vector1_CE26F4AA;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            TEXTURE2D(Texture2D_906780E9); SAMPLER(samplerTexture2D_906780E9); float4 Texture2D_906780E9_TexelSize;
            SAMPLER(_SampleTexture2D_A487BAE3_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_57E03179_Sampler_3_Linear_Repeat);

            // Graph Functions

            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }

            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }


            float2 Unity_GradientNoise_Dir_float(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }

            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            {
                float2 p = UV * Scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }

            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }


            inline float2 Unity_Voronoi_RandomVector_float(float2 UV, float offset)
            {
                float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                UV = frac(sin(mul(UV, m)) * 46839.32);
                return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
            }

            void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
            {
                float2 g = floor(UV * CellDensity);
                float2 f = frac(UV * CellDensity);
                float t = 8.0;
                float3 res = float3(8.0, 0.0, 0.0);

                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        float2 lattice = float2(x,y);
                        float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                        float d = distance(lattice + offset, f);

                        if (d < res.x)
                        {
                            res = float3(d, offset.x, offset.y);
                            Out = res.x;
                            Cells = res.y;
                        }
                    }
                }
            }

            void Unity_Power_float(float A, float B, out float Out)
            {
                Out = pow(A, B);
            }

            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            // Graph Vertex
            // GraphVertex: <None>

            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float4 uv0;
                float3 TimeParameters;
            };

            struct SurfaceDescription
            {
                float3 Albedo;
                float3 Normal;
                float3 Emission;
                float Metallic;
                float Smoothness;
                float Occlusion;
                float Alpha;
                float AlphaClipThreshold;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _Property_FF70338C_Out_0 = Color_D25B2F83;
                float4 _UV_AC8399CB_Out_0 = IN.uv0;
                float2 _Property_FA566B97_Out_0 = Vector2_F89F6FDB;
                float2 _Multiply_F713A7A3_Out_2;
                Unity_Multiply_float(_Property_FA566B97_Out_0, (IN.TimeParameters.x.xx), _Multiply_F713A7A3_Out_2);
                float2 _TilingAndOffset_3C47AE24_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_F713A7A3_Out_2, _TilingAndOffset_3C47AE24_Out_3);
                float _Property_4C6DBA76_Out_0 = Vector1_5C9FEA09;
                float _GradientNoise_49BEC5FB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_3C47AE24_Out_3, _Property_4C6DBA76_Out_0, _GradientNoise_49BEC5FB_Out_2);
                float _Property_F59EB235_Out_0 = Vector1_5D8C30A;
                float4 _Lerp_4A94D267_Out_3;
                Unity_Lerp_float4(_UV_AC8399CB_Out_0, (_GradientNoise_49BEC5FB_Out_2.xxxx), (_Property_F59EB235_Out_0.xxxx), _Lerp_4A94D267_Out_3);
                float4 _SampleTexture2D_A487BAE3_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (_Lerp_4A94D267_Out_3.xy));
                float _SampleTexture2D_A487BAE3_R_4 = _SampleTexture2D_A487BAE3_RGBA_0.r;
                float _SampleTexture2D_A487BAE3_G_5 = _SampleTexture2D_A487BAE3_RGBA_0.g;
                float _SampleTexture2D_A487BAE3_B_6 = _SampleTexture2D_A487BAE3_RGBA_0.b;
                float _SampleTexture2D_A487BAE3_A_7 = _SampleTexture2D_A487BAE3_RGBA_0.a;
                float2 _Property_548424FB_Out_0 = Vector2_18F30DF1;
                float2 _Multiply_60FBCA80_Out_2;
                Unity_Multiply_float((IN.TimeParameters.x.xx), _Property_548424FB_Out_0, _Multiply_60FBCA80_Out_2);
                float2 _TilingAndOffset_E4074479_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_60FBCA80_Out_2, _TilingAndOffset_E4074479_Out_3);
                float _Property_2247BE3E_Out_0 = Vector1_45E24A1A;
                float _Property_88EB50A3_Out_0 = Vector1_444105F2;
                float _Voronoi_80D160C3_Out_3;
                float _Voronoi_80D160C3_Cells_4;
                Unity_Voronoi_float(_TilingAndOffset_E4074479_Out_3, _Property_2247BE3E_Out_0, _Property_88EB50A3_Out_0, _Voronoi_80D160C3_Out_3, _Voronoi_80D160C3_Cells_4);
                float _Property_DCD3B7CE_Out_0 = Vector1_B0C7E46C;
                float _Power_AE954BD6_Out_2;
                Unity_Power_float(_Voronoi_80D160C3_Out_3, _Property_DCD3B7CE_Out_0, _Power_AE954BD6_Out_2);
                float _Multiply_1CBE5BB9_Out_2;
                Unity_Multiply_float(_GradientNoise_49BEC5FB_Out_2, _Power_AE954BD6_Out_2, _Multiply_1CBE5BB9_Out_2);
                float4 _Multiply_11FBF49E_Out_2;
                Unity_Multiply_float(_SampleTexture2D_A487BAE3_RGBA_0, (_Multiply_1CBE5BB9_Out_2.xxxx), _Multiply_11FBF49E_Out_2);
                float4 _SampleTexture2D_57E03179_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_906780E9, samplerTexture2D_906780E9, IN.uv0.xy);
                float _SampleTexture2D_57E03179_R_4 = _SampleTexture2D_57E03179_RGBA_0.r;
                float _SampleTexture2D_57E03179_G_5 = _SampleTexture2D_57E03179_RGBA_0.g;
                float _SampleTexture2D_57E03179_B_6 = _SampleTexture2D_57E03179_RGBA_0.b;
                float _SampleTexture2D_57E03179_A_7 = _SampleTexture2D_57E03179_RGBA_0.a;
                float4 _Multiply_CE6129D4_Out_2;
                Unity_Multiply_float(_Multiply_11FBF49E_Out_2, _SampleTexture2D_57E03179_RGBA_0, _Multiply_CE6129D4_Out_2);
                float _Property_9D24B5F2_Out_0 = Vector1_CE26F4AA;
                float4 _Multiply_5A448973_Out_2;
                Unity_Multiply_float(_Multiply_CE6129D4_Out_2, (_Property_9D24B5F2_Out_0.xxxx), _Multiply_5A448973_Out_2);
                float4 _Multiply_D2160EC4_Out_2;
                Unity_Multiply_float(_Property_FF70338C_Out_0, _Multiply_5A448973_Out_2, _Multiply_D2160EC4_Out_2);
                surface.Albedo = (_Multiply_D2160EC4_Out_2.xyz);
                surface.Normal = IN.TangentSpaceNormal;
                surface.Emission = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                surface.Metallic = 0;
                surface.Smoothness = 0.5;
                surface.Occlusion = 1;
                surface.Alpha = (_Multiply_5A448973_Out_2).x;
                surface.AlphaClipThreshold = 0;
                return surface;
            }

            // --------------------------------------------------
            // Structs and Packing

            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float3 normalWS;
                float4 tangentWS;
                float4 texCoord0;
                float3 viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                float2 lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                float3 sh;
                #endif
                float4 fogFactorAndVertexLight;
                float4 shadowCoord;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if defined(LIGHTMAP_ON)
                #endif
                #if !defined(LIGHTMAP_ON)
                #endif
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                float3 interp01 : TEXCOORD1;
                float4 interp02 : TEXCOORD2;
                float4 interp03 : TEXCOORD3;
                float3 interp04 : TEXCOORD4;
                float2 interp05 : TEXCOORD5;
                float3 interp06 : TEXCOORD6;
                float4 interp07 : TEXCOORD7;
                float4 interp08 : TEXCOORD8;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.tangentWS;
                output.interp03.xyzw = input.texCoord0;
                output.interp04.xyz = input.viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                output.interp05.xy = input.lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.interp06.xyz = input.sh;
                #endif
                output.interp07.xyzw = input.fogFactorAndVertexLight;
                output.interp08.xyzw = input.shadowCoord;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.tangentWS = input.interp02.xyzw;
                output.texCoord0 = input.interp03.xyzw;
                output.viewDirectionWS = input.interp04.xyz;
                #if defined(LIGHTMAP_ON)
                output.lightmapUV = input.interp05.xy;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.sh = input.interp06.xyz;
                #endif
                output.fogFactorAndVertexLight = input.interp07.xyzw;
                output.shadowCoord = input.interp08.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                output.uv0 = input.texCoord0;
                output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }


            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

                // Render State
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                Cull Off
                ZTest LEqual
                ZWrite On
                // ColorMask: <None>


                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                // Pragmas
                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 2.0
                #pragma multi_compile_instancing

                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>

                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define VARYINGS_NEED_TEXCOORD0
                #define SHADERPASS_SHADOWCASTER

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Color_D25B2F83;
                float Vector1_5D8C30A;
                float2 Vector2_F89F6FDB;
                float Vector1_5C9FEA09;
                float2 Vector2_18F30DF1;
                float Vector1_444105F2;
                float Vector1_45E24A1A;
                float Vector1_B0C7E46C;
                float _Stencil;
                float _StencilOp;
                float _StencilComp;
                float _StencilReadMask;
                float _StencilWriteMask;
                float _ColorMask;
                float Vector1_CE26F4AA;
                CBUFFER_END
                TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                TEXTURE2D(Texture2D_906780E9); SAMPLER(samplerTexture2D_906780E9); float4 Texture2D_906780E9_TexelSize;
                SAMPLER(_SampleTexture2D_A487BAE3_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_57E03179_Sampler_3_Linear_Repeat);

                // Graph Functions

                void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                {
                    Out = A * B;
                }

                void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                {
                    Out = UV * Tiling + Offset;
                }


                float2 Unity_GradientNoise_Dir_float(float2 p)
                {
                    // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                    p = p % 289;
                    float x = (34 * p.x + 1) * p.x % 289 + p.y;
                    x = (34 * x + 1) * x % 289;
                    x = frac(x / 41) * 2 - 1;
                    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
                }

                void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
                {
                    float2 p = UV * Scale;
                    float2 ip = floor(p);
                    float2 fp = frac(p);
                    float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                    float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                    float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                    float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                    Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
                }

                void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
                {
                    Out = lerp(A, B, T);
                }


                inline float2 Unity_Voronoi_RandomVector_float(float2 UV, float offset)
                {
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                    UV = frac(sin(mul(UV, m)) * 46839.32);
                    return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
                }

                void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
                {
                    float2 g = floor(UV * CellDensity);
                    float2 f = frac(UV * CellDensity);
                    float t = 8.0;
                    float3 res = float3(8.0, 0.0, 0.0);

                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            float2 lattice = float2(x,y);
                            float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                            float d = distance(lattice + offset, f);

                            if (d < res.x)
                            {
                                res = float3(d, offset.x, offset.y);
                                Out = res.x;
                                Cells = res.y;
                            }
                        }
                    }
                }

                void Unity_Power_float(float A, float B, out float Out)
                {
                    Out = pow(A, B);
                }

                void Unity_Multiply_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }

                void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                {
                    Out = A * B;
                }

                // Graph Vertex
                // GraphVertex: <None>

                // Graph Pixel
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float4 uv0;
                    float3 TimeParameters;
                };

                struct SurfaceDescription
                {
                    float Alpha;
                    float AlphaClipThreshold;
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _UV_AC8399CB_Out_0 = IN.uv0;
                    float2 _Property_FA566B97_Out_0 = Vector2_F89F6FDB;
                    float2 _Multiply_F713A7A3_Out_2;
                    Unity_Multiply_float(_Property_FA566B97_Out_0, (IN.TimeParameters.x.xx), _Multiply_F713A7A3_Out_2);
                    float2 _TilingAndOffset_3C47AE24_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_F713A7A3_Out_2, _TilingAndOffset_3C47AE24_Out_3);
                    float _Property_4C6DBA76_Out_0 = Vector1_5C9FEA09;
                    float _GradientNoise_49BEC5FB_Out_2;
                    Unity_GradientNoise_float(_TilingAndOffset_3C47AE24_Out_3, _Property_4C6DBA76_Out_0, _GradientNoise_49BEC5FB_Out_2);
                    float _Property_F59EB235_Out_0 = Vector1_5D8C30A;
                    float4 _Lerp_4A94D267_Out_3;
                    Unity_Lerp_float4(_UV_AC8399CB_Out_0, (_GradientNoise_49BEC5FB_Out_2.xxxx), (_Property_F59EB235_Out_0.xxxx), _Lerp_4A94D267_Out_3);
                    float4 _SampleTexture2D_A487BAE3_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (_Lerp_4A94D267_Out_3.xy));
                    float _SampleTexture2D_A487BAE3_R_4 = _SampleTexture2D_A487BAE3_RGBA_0.r;
                    float _SampleTexture2D_A487BAE3_G_5 = _SampleTexture2D_A487BAE3_RGBA_0.g;
                    float _SampleTexture2D_A487BAE3_B_6 = _SampleTexture2D_A487BAE3_RGBA_0.b;
                    float _SampleTexture2D_A487BAE3_A_7 = _SampleTexture2D_A487BAE3_RGBA_0.a;
                    float2 _Property_548424FB_Out_0 = Vector2_18F30DF1;
                    float2 _Multiply_60FBCA80_Out_2;
                    Unity_Multiply_float((IN.TimeParameters.x.xx), _Property_548424FB_Out_0, _Multiply_60FBCA80_Out_2);
                    float2 _TilingAndOffset_E4074479_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_60FBCA80_Out_2, _TilingAndOffset_E4074479_Out_3);
                    float _Property_2247BE3E_Out_0 = Vector1_45E24A1A;
                    float _Property_88EB50A3_Out_0 = Vector1_444105F2;
                    float _Voronoi_80D160C3_Out_3;
                    float _Voronoi_80D160C3_Cells_4;
                    Unity_Voronoi_float(_TilingAndOffset_E4074479_Out_3, _Property_2247BE3E_Out_0, _Property_88EB50A3_Out_0, _Voronoi_80D160C3_Out_3, _Voronoi_80D160C3_Cells_4);
                    float _Property_DCD3B7CE_Out_0 = Vector1_B0C7E46C;
                    float _Power_AE954BD6_Out_2;
                    Unity_Power_float(_Voronoi_80D160C3_Out_3, _Property_DCD3B7CE_Out_0, _Power_AE954BD6_Out_2);
                    float _Multiply_1CBE5BB9_Out_2;
                    Unity_Multiply_float(_GradientNoise_49BEC5FB_Out_2, _Power_AE954BD6_Out_2, _Multiply_1CBE5BB9_Out_2);
                    float4 _Multiply_11FBF49E_Out_2;
                    Unity_Multiply_float(_SampleTexture2D_A487BAE3_RGBA_0, (_Multiply_1CBE5BB9_Out_2.xxxx), _Multiply_11FBF49E_Out_2);
                    float4 _SampleTexture2D_57E03179_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_906780E9, samplerTexture2D_906780E9, IN.uv0.xy);
                    float _SampleTexture2D_57E03179_R_4 = _SampleTexture2D_57E03179_RGBA_0.r;
                    float _SampleTexture2D_57E03179_G_5 = _SampleTexture2D_57E03179_RGBA_0.g;
                    float _SampleTexture2D_57E03179_B_6 = _SampleTexture2D_57E03179_RGBA_0.b;
                    float _SampleTexture2D_57E03179_A_7 = _SampleTexture2D_57E03179_RGBA_0.a;
                    float4 _Multiply_CE6129D4_Out_2;
                    Unity_Multiply_float(_Multiply_11FBF49E_Out_2, _SampleTexture2D_57E03179_RGBA_0, _Multiply_CE6129D4_Out_2);
                    float _Property_9D24B5F2_Out_0 = Vector1_CE26F4AA;
                    float4 _Multiply_5A448973_Out_2;
                    Unity_Multiply_float(_Multiply_CE6129D4_Out_2, (_Property_9D24B5F2_Out_0.xxxx), _Multiply_5A448973_Out_2);
                    surface.Alpha = (_Multiply_5A448973_Out_2).x;
                    surface.AlphaClipThreshold = 0;
                    return surface;
                }

                // --------------------------------------------------
                // Structs and Packing

                // Generated Type: Attributes
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };

                // Generated Type: Varyings
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Generated Type: PackedVaryings
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    float4 interp00 : TEXCOORD0;
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Packed Type: Varyings
                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output = (PackedVaryings)0;
                    output.positionCS = input.positionCS;
                    output.interp00.xyzw = input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // Unpacked Type: Varyings
                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output = (Varyings)0;
                    output.positionCS = input.positionCS;
                    output.texCoord0 = input.interp00.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // --------------------------------------------------
                // Build Graph Inputs

                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                    output.uv0 = input.texCoord0;
                    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
                }


                // --------------------------------------------------
                // Main

                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

                ENDHLSL
            }

            Pass
            {
                Name "DepthOnly"
                Tags
                {
                    "LightMode" = "DepthOnly"
                }

                    // Render State
                    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                    Cull Off
                    ZTest LEqual
                    ZWrite On
                    ColorMask 0


                    HLSLPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag

                    // Debug
                    // <None>

                    // --------------------------------------------------
                    // Pass

                    // Pragmas
                    #pragma prefer_hlslcc gles
                    #pragma exclude_renderers d3d11_9x
                    #pragma target 2.0
                    #pragma multi_compile_instancing

                    // Keywords
                    // PassKeywords: <None>
                    // GraphKeywords: <None>

                    // Defines
                    #define _SURFACE_TYPE_TRANSPARENT 1
                    #define _NORMAL_DROPOFF_TS 1
                    #define ATTRIBUTES_NEED_NORMAL
                    #define ATTRIBUTES_NEED_TANGENT
                    #define ATTRIBUTES_NEED_TEXCOORD0
                    #define VARYINGS_NEED_TEXCOORD0
                    #define SHADERPASS_DEPTHONLY

                    // Includes
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                    // --------------------------------------------------
                    // Graph

                    // Graph Properties
                    CBUFFER_START(UnityPerMaterial)
                    float4 Color_D25B2F83;
                    float Vector1_5D8C30A;
                    float2 Vector2_F89F6FDB;
                    float Vector1_5C9FEA09;
                    float2 Vector2_18F30DF1;
                    float Vector1_444105F2;
                    float Vector1_45E24A1A;
                    float Vector1_B0C7E46C;
                    float _Stencil;
                    float _StencilOp;
                    float _StencilComp;
                    float _StencilReadMask;
                    float _StencilWriteMask;
                    float _ColorMask;
                    float Vector1_CE26F4AA;
                    CBUFFER_END
                    TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                    TEXTURE2D(Texture2D_906780E9); SAMPLER(samplerTexture2D_906780E9); float4 Texture2D_906780E9_TexelSize;
                    SAMPLER(_SampleTexture2D_A487BAE3_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_57E03179_Sampler_3_Linear_Repeat);

                    // Graph Functions

                    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                    {
                        Out = A * B;
                    }

                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                    {
                        Out = UV * Tiling + Offset;
                    }


                    float2 Unity_GradientNoise_Dir_float(float2 p)
                    {
                        // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                        p = p % 289;
                        float x = (34 * p.x + 1) * p.x % 289 + p.y;
                        x = (34 * x + 1) * x % 289;
                        x = frac(x / 41) * 2 - 1;
                        return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
                    }

                    void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
                    {
                        float2 p = UV * Scale;
                        float2 ip = floor(p);
                        float2 fp = frac(p);
                        float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                        float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                        float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                        float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                        fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                        Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
                    }

                    void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
                    {
                        Out = lerp(A, B, T);
                    }


                    inline float2 Unity_Voronoi_RandomVector_float(float2 UV, float offset)
                    {
                        float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                        UV = frac(sin(mul(UV, m)) * 46839.32);
                        return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
                    }

                    void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
                    {
                        float2 g = floor(UV * CellDensity);
                        float2 f = frac(UV * CellDensity);
                        float t = 8.0;
                        float3 res = float3(8.0, 0.0, 0.0);

                        for (int y = -1; y <= 1; y++)
                        {
                            for (int x = -1; x <= 1; x++)
                            {
                                float2 lattice = float2(x,y);
                                float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                                float d = distance(lattice + offset, f);

                                if (d < res.x)
                                {
                                    res = float3(d, offset.x, offset.y);
                                    Out = res.x;
                                    Cells = res.y;
                                }
                            }
                        }
                    }

                    void Unity_Power_float(float A, float B, out float Out)
                    {
                        Out = pow(A, B);
                    }

                    void Unity_Multiply_float(float A, float B, out float Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }

                    // Graph Vertex
                    // GraphVertex: <None>

                    // Graph Pixel
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal;
                        float4 uv0;
                        float3 TimeParameters;
                    };

                    struct SurfaceDescription
                    {
                        float Alpha;
                        float AlphaClipThreshold;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _UV_AC8399CB_Out_0 = IN.uv0;
                        float2 _Property_FA566B97_Out_0 = Vector2_F89F6FDB;
                        float2 _Multiply_F713A7A3_Out_2;
                        Unity_Multiply_float(_Property_FA566B97_Out_0, (IN.TimeParameters.x.xx), _Multiply_F713A7A3_Out_2);
                        float2 _TilingAndOffset_3C47AE24_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_F713A7A3_Out_2, _TilingAndOffset_3C47AE24_Out_3);
                        float _Property_4C6DBA76_Out_0 = Vector1_5C9FEA09;
                        float _GradientNoise_49BEC5FB_Out_2;
                        Unity_GradientNoise_float(_TilingAndOffset_3C47AE24_Out_3, _Property_4C6DBA76_Out_0, _GradientNoise_49BEC5FB_Out_2);
                        float _Property_F59EB235_Out_0 = Vector1_5D8C30A;
                        float4 _Lerp_4A94D267_Out_3;
                        Unity_Lerp_float4(_UV_AC8399CB_Out_0, (_GradientNoise_49BEC5FB_Out_2.xxxx), (_Property_F59EB235_Out_0.xxxx), _Lerp_4A94D267_Out_3);
                        float4 _SampleTexture2D_A487BAE3_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (_Lerp_4A94D267_Out_3.xy));
                        float _SampleTexture2D_A487BAE3_R_4 = _SampleTexture2D_A487BAE3_RGBA_0.r;
                        float _SampleTexture2D_A487BAE3_G_5 = _SampleTexture2D_A487BAE3_RGBA_0.g;
                        float _SampleTexture2D_A487BAE3_B_6 = _SampleTexture2D_A487BAE3_RGBA_0.b;
                        float _SampleTexture2D_A487BAE3_A_7 = _SampleTexture2D_A487BAE3_RGBA_0.a;
                        float2 _Property_548424FB_Out_0 = Vector2_18F30DF1;
                        float2 _Multiply_60FBCA80_Out_2;
                        Unity_Multiply_float((IN.TimeParameters.x.xx), _Property_548424FB_Out_0, _Multiply_60FBCA80_Out_2);
                        float2 _TilingAndOffset_E4074479_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_60FBCA80_Out_2, _TilingAndOffset_E4074479_Out_3);
                        float _Property_2247BE3E_Out_0 = Vector1_45E24A1A;
                        float _Property_88EB50A3_Out_0 = Vector1_444105F2;
                        float _Voronoi_80D160C3_Out_3;
                        float _Voronoi_80D160C3_Cells_4;
                        Unity_Voronoi_float(_TilingAndOffset_E4074479_Out_3, _Property_2247BE3E_Out_0, _Property_88EB50A3_Out_0, _Voronoi_80D160C3_Out_3, _Voronoi_80D160C3_Cells_4);
                        float _Property_DCD3B7CE_Out_0 = Vector1_B0C7E46C;
                        float _Power_AE954BD6_Out_2;
                        Unity_Power_float(_Voronoi_80D160C3_Out_3, _Property_DCD3B7CE_Out_0, _Power_AE954BD6_Out_2);
                        float _Multiply_1CBE5BB9_Out_2;
                        Unity_Multiply_float(_GradientNoise_49BEC5FB_Out_2, _Power_AE954BD6_Out_2, _Multiply_1CBE5BB9_Out_2);
                        float4 _Multiply_11FBF49E_Out_2;
                        Unity_Multiply_float(_SampleTexture2D_A487BAE3_RGBA_0, (_Multiply_1CBE5BB9_Out_2.xxxx), _Multiply_11FBF49E_Out_2);
                        float4 _SampleTexture2D_57E03179_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_906780E9, samplerTexture2D_906780E9, IN.uv0.xy);
                        float _SampleTexture2D_57E03179_R_4 = _SampleTexture2D_57E03179_RGBA_0.r;
                        float _SampleTexture2D_57E03179_G_5 = _SampleTexture2D_57E03179_RGBA_0.g;
                        float _SampleTexture2D_57E03179_B_6 = _SampleTexture2D_57E03179_RGBA_0.b;
                        float _SampleTexture2D_57E03179_A_7 = _SampleTexture2D_57E03179_RGBA_0.a;
                        float4 _Multiply_CE6129D4_Out_2;
                        Unity_Multiply_float(_Multiply_11FBF49E_Out_2, _SampleTexture2D_57E03179_RGBA_0, _Multiply_CE6129D4_Out_2);
                        float _Property_9D24B5F2_Out_0 = Vector1_CE26F4AA;
                        float4 _Multiply_5A448973_Out_2;
                        Unity_Multiply_float(_Multiply_CE6129D4_Out_2, (_Property_9D24B5F2_Out_0.xxxx), _Multiply_5A448973_Out_2);
                        surface.Alpha = (_Multiply_5A448973_Out_2).x;
                        surface.AlphaClipThreshold = 0;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Structs and Packing

                    // Generated Type: Attributes
                    struct Attributes
                    {
                        float3 positionOS : POSITION;
                        float3 normalOS : NORMAL;
                        float4 tangentOS : TANGENT;
                        float4 uv0 : TEXCOORD0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : INSTANCEID_SEMANTIC;
                        #endif
                    };

                    // Generated Type: Varyings
                    struct Varyings
                    {
                        float4 positionCS : SV_POSITION;
                        float4 texCoord0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Generated Type: PackedVaryings
                    struct PackedVaryings
                    {
                        float4 positionCS : SV_POSITION;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        float4 interp00 : TEXCOORD0;
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Packed Type: Varyings
                    PackedVaryings PackVaryings(Varyings input)
                    {
                        PackedVaryings output = (PackedVaryings)0;
                        output.positionCS = input.positionCS;
                        output.interp00.xyzw = input.texCoord0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // Unpacked Type: Varyings
                    Varyings UnpackVaryings(PackedVaryings input)
                    {
                        Varyings output = (Varyings)0;
                        output.positionCS = input.positionCS;
                        output.texCoord0 = input.interp00.xyzw;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs

                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                        output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                        output.uv0 = input.texCoord0;
                        output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                    }


                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

                    ENDHLSL
                }

                Pass
                {
                    Name "Meta"
                    Tags
                    {
                        "LightMode" = "Meta"
                    }

                        // Render State
                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                        Cull Off
                        ZTest LEqual
                        ZWrite On
                        // ColorMask: <None>


                        HLSLPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag

                        // Debug
                        // <None>

                        // --------------------------------------------------
                        // Pass

                        // Pragmas
                        #pragma prefer_hlslcc gles
                        #pragma exclude_renderers d3d11_9x
                        #pragma target 2.0

                        // Keywords
                        #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                        // GraphKeywords: <None>

                        // Defines
                        #define _SURFACE_TYPE_TRANSPARENT 1
                        #define _NORMAL_DROPOFF_TS 1
                        #define ATTRIBUTES_NEED_NORMAL
                        #define ATTRIBUTES_NEED_TANGENT
                        #define ATTRIBUTES_NEED_TEXCOORD0
                        #define ATTRIBUTES_NEED_TEXCOORD1
                        #define ATTRIBUTES_NEED_TEXCOORD2
                        #define VARYINGS_NEED_TEXCOORD0
                        #define SHADERPASS_META

                        // Includes
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
                        #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                        // --------------------------------------------------
                        // Graph

                        // Graph Properties
                        CBUFFER_START(UnityPerMaterial)
                        float4 Color_D25B2F83;
                        float Vector1_5D8C30A;
                        float2 Vector2_F89F6FDB;
                        float Vector1_5C9FEA09;
                        float2 Vector2_18F30DF1;
                        float Vector1_444105F2;
                        float Vector1_45E24A1A;
                        float Vector1_B0C7E46C;
                        float _Stencil;
                        float _StencilOp;
                        float _StencilComp;
                        float _StencilReadMask;
                        float _StencilWriteMask;
                        float _ColorMask;
                        float Vector1_CE26F4AA;
                        CBUFFER_END
                        TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                        TEXTURE2D(Texture2D_906780E9); SAMPLER(samplerTexture2D_906780E9); float4 Texture2D_906780E9_TexelSize;
                        SAMPLER(_SampleTexture2D_A487BAE3_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_57E03179_Sampler_3_Linear_Repeat);

                        // Graph Functions

                        void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                        {
                            Out = A * B;
                        }

                        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                        {
                            Out = UV * Tiling + Offset;
                        }


                        float2 Unity_GradientNoise_Dir_float(float2 p)
                        {
                            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                            p = p % 289;
                            float x = (34 * p.x + 1) * p.x % 289 + p.y;
                            x = (34 * x + 1) * x % 289;
                            x = frac(x / 41) * 2 - 1;
                            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
                        }

                        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
                        {
                            float2 p = UV * Scale;
                            float2 ip = floor(p);
                            float2 fp = frac(p);
                            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
                        }

                        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
                        {
                            Out = lerp(A, B, T);
                        }


                        inline float2 Unity_Voronoi_RandomVector_float(float2 UV, float offset)
                        {
                            float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                            UV = frac(sin(mul(UV, m)) * 46839.32);
                            return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
                        }

                        void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
                        {
                            float2 g = floor(UV * CellDensity);
                            float2 f = frac(UV * CellDensity);
                            float t = 8.0;
                            float3 res = float3(8.0, 0.0, 0.0);

                            for (int y = -1; y <= 1; y++)
                            {
                                for (int x = -1; x <= 1; x++)
                                {
                                    float2 lattice = float2(x,y);
                                    float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                                    float d = distance(lattice + offset, f);

                                    if (d < res.x)
                                    {
                                        res = float3(d, offset.x, offset.y);
                                        Out = res.x;
                                        Cells = res.y;
                                    }
                                }
                            }
                        }

                        void Unity_Power_float(float A, float B, out float Out)
                        {
                            Out = pow(A, B);
                        }

                        void Unity_Multiply_float(float A, float B, out float Out)
                        {
                            Out = A * B;
                        }

                        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                        {
                            Out = A * B;
                        }

                        // Graph Vertex
                        // GraphVertex: <None>

                        // Graph Pixel
                        struct SurfaceDescriptionInputs
                        {
                            float3 TangentSpaceNormal;
                            float4 uv0;
                            float3 TimeParameters;
                        };

                        struct SurfaceDescription
                        {
                            float3 Albedo;
                            float3 Emission;
                            float Alpha;
                            float AlphaClipThreshold;
                        };

                        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                        {
                            SurfaceDescription surface = (SurfaceDescription)0;
                            float4 _Property_FF70338C_Out_0 = Color_D25B2F83;
                            float4 _UV_AC8399CB_Out_0 = IN.uv0;
                            float2 _Property_FA566B97_Out_0 = Vector2_F89F6FDB;
                            float2 _Multiply_F713A7A3_Out_2;
                            Unity_Multiply_float(_Property_FA566B97_Out_0, (IN.TimeParameters.x.xx), _Multiply_F713A7A3_Out_2);
                            float2 _TilingAndOffset_3C47AE24_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_F713A7A3_Out_2, _TilingAndOffset_3C47AE24_Out_3);
                            float _Property_4C6DBA76_Out_0 = Vector1_5C9FEA09;
                            float _GradientNoise_49BEC5FB_Out_2;
                            Unity_GradientNoise_float(_TilingAndOffset_3C47AE24_Out_3, _Property_4C6DBA76_Out_0, _GradientNoise_49BEC5FB_Out_2);
                            float _Property_F59EB235_Out_0 = Vector1_5D8C30A;
                            float4 _Lerp_4A94D267_Out_3;
                            Unity_Lerp_float4(_UV_AC8399CB_Out_0, (_GradientNoise_49BEC5FB_Out_2.xxxx), (_Property_F59EB235_Out_0.xxxx), _Lerp_4A94D267_Out_3);
                            float4 _SampleTexture2D_A487BAE3_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (_Lerp_4A94D267_Out_3.xy));
                            float _SampleTexture2D_A487BAE3_R_4 = _SampleTexture2D_A487BAE3_RGBA_0.r;
                            float _SampleTexture2D_A487BAE3_G_5 = _SampleTexture2D_A487BAE3_RGBA_0.g;
                            float _SampleTexture2D_A487BAE3_B_6 = _SampleTexture2D_A487BAE3_RGBA_0.b;
                            float _SampleTexture2D_A487BAE3_A_7 = _SampleTexture2D_A487BAE3_RGBA_0.a;
                            float2 _Property_548424FB_Out_0 = Vector2_18F30DF1;
                            float2 _Multiply_60FBCA80_Out_2;
                            Unity_Multiply_float((IN.TimeParameters.x.xx), _Property_548424FB_Out_0, _Multiply_60FBCA80_Out_2);
                            float2 _TilingAndOffset_E4074479_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_60FBCA80_Out_2, _TilingAndOffset_E4074479_Out_3);
                            float _Property_2247BE3E_Out_0 = Vector1_45E24A1A;
                            float _Property_88EB50A3_Out_0 = Vector1_444105F2;
                            float _Voronoi_80D160C3_Out_3;
                            float _Voronoi_80D160C3_Cells_4;
                            Unity_Voronoi_float(_TilingAndOffset_E4074479_Out_3, _Property_2247BE3E_Out_0, _Property_88EB50A3_Out_0, _Voronoi_80D160C3_Out_3, _Voronoi_80D160C3_Cells_4);
                            float _Property_DCD3B7CE_Out_0 = Vector1_B0C7E46C;
                            float _Power_AE954BD6_Out_2;
                            Unity_Power_float(_Voronoi_80D160C3_Out_3, _Property_DCD3B7CE_Out_0, _Power_AE954BD6_Out_2);
                            float _Multiply_1CBE5BB9_Out_2;
                            Unity_Multiply_float(_GradientNoise_49BEC5FB_Out_2, _Power_AE954BD6_Out_2, _Multiply_1CBE5BB9_Out_2);
                            float4 _Multiply_11FBF49E_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_A487BAE3_RGBA_0, (_Multiply_1CBE5BB9_Out_2.xxxx), _Multiply_11FBF49E_Out_2);
                            float4 _SampleTexture2D_57E03179_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_906780E9, samplerTexture2D_906780E9, IN.uv0.xy);
                            float _SampleTexture2D_57E03179_R_4 = _SampleTexture2D_57E03179_RGBA_0.r;
                            float _SampleTexture2D_57E03179_G_5 = _SampleTexture2D_57E03179_RGBA_0.g;
                            float _SampleTexture2D_57E03179_B_6 = _SampleTexture2D_57E03179_RGBA_0.b;
                            float _SampleTexture2D_57E03179_A_7 = _SampleTexture2D_57E03179_RGBA_0.a;
                            float4 _Multiply_CE6129D4_Out_2;
                            Unity_Multiply_float(_Multiply_11FBF49E_Out_2, _SampleTexture2D_57E03179_RGBA_0, _Multiply_CE6129D4_Out_2);
                            float _Property_9D24B5F2_Out_0 = Vector1_CE26F4AA;
                            float4 _Multiply_5A448973_Out_2;
                            Unity_Multiply_float(_Multiply_CE6129D4_Out_2, (_Property_9D24B5F2_Out_0.xxxx), _Multiply_5A448973_Out_2);
                            float4 _Multiply_D2160EC4_Out_2;
                            Unity_Multiply_float(_Property_FF70338C_Out_0, _Multiply_5A448973_Out_2, _Multiply_D2160EC4_Out_2);
                            surface.Albedo = (_Multiply_D2160EC4_Out_2.xyz);
                            surface.Emission = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                            surface.Alpha = (_Multiply_5A448973_Out_2).x;
                            surface.AlphaClipThreshold = 0;
                            return surface;
                        }

                        // --------------------------------------------------
                        // Structs and Packing

                        // Generated Type: Attributes
                        struct Attributes
                        {
                            float3 positionOS : POSITION;
                            float3 normalOS : NORMAL;
                            float4 tangentOS : TANGENT;
                            float4 uv0 : TEXCOORD0;
                            float4 uv1 : TEXCOORD1;
                            float4 uv2 : TEXCOORD2;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : INSTANCEID_SEMANTIC;
                            #endif
                        };

                        // Generated Type: Varyings
                        struct Varyings
                        {
                            float4 positionCS : SV_POSITION;
                            float4 texCoord0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        // Generated Type: PackedVaryings
                        struct PackedVaryings
                        {
                            float4 positionCS : SV_POSITION;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            float4 interp00 : TEXCOORD0;
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        // Packed Type: Varyings
                        PackedVaryings PackVaryings(Varyings input)
                        {
                            PackedVaryings output = (PackedVaryings)0;
                            output.positionCS = input.positionCS;
                            output.interp00.xyzw = input.texCoord0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        // Unpacked Type: Varyings
                        Varyings UnpackVaryings(PackedVaryings input)
                        {
                            Varyings output = (Varyings)0;
                            output.positionCS = input.positionCS;
                            output.texCoord0 = input.interp00.xyzw;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        // --------------------------------------------------
                        // Build Graph Inputs

                        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                        {
                            SurfaceDescriptionInputs output;
                            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                            output.uv0 = input.texCoord0;
                            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                        #else
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                        #endif
                        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                            return output;
                        }


                        // --------------------------------------------------
                        // Main

                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

                        ENDHLSL
                    }

                    Pass
                    {
                            // Name: <None>
                            Tags
                            {
                                "LightMode" = "Universal2D"
                            }

                            // Render State
                            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                            Cull Off
                            ZTest LEqual
                            ZWrite Off
                            // ColorMask: <None>


                            HLSLPROGRAM
                            #pragma vertex vert
                            #pragma fragment frag

                            // Debug
                            // <None>

                            // --------------------------------------------------
                            // Pass

                            // Pragmas
                            #pragma prefer_hlslcc gles
                            #pragma exclude_renderers d3d11_9x
                            #pragma target 2.0
                            #pragma multi_compile_instancing

                            // Keywords
                            // PassKeywords: <None>
                            // GraphKeywords: <None>

                            // Defines
                            #define _SURFACE_TYPE_TRANSPARENT 1
                            #define _NORMAL_DROPOFF_TS 1
                            #define ATTRIBUTES_NEED_NORMAL
                            #define ATTRIBUTES_NEED_TANGENT
                            #define ATTRIBUTES_NEED_TEXCOORD0
                            #define VARYINGS_NEED_TEXCOORD0
                            #define SHADERPASS_2D

                            // Includes
                            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                            // --------------------------------------------------
                            // Graph

                            // Graph Properties
                            CBUFFER_START(UnityPerMaterial)
                            float4 Color_D25B2F83;
                            float Vector1_5D8C30A;
                            float2 Vector2_F89F6FDB;
                            float Vector1_5C9FEA09;
                            float2 Vector2_18F30DF1;
                            float Vector1_444105F2;
                            float Vector1_45E24A1A;
                            float Vector1_B0C7E46C;
                            float _Stencil;
                            float _StencilOp;
                            float _StencilComp;
                            float _StencilReadMask;
                            float _StencilWriteMask;
                            float _ColorMask;
                            float Vector1_CE26F4AA;
                            CBUFFER_END
                            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                            TEXTURE2D(Texture2D_906780E9); SAMPLER(samplerTexture2D_906780E9); float4 Texture2D_906780E9_TexelSize;
                            SAMPLER(_SampleTexture2D_A487BAE3_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_57E03179_Sampler_3_Linear_Repeat);

                            // Graph Functions

                            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                            {
                                Out = A * B;
                            }

                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                            {
                                Out = UV * Tiling + Offset;
                            }


                            float2 Unity_GradientNoise_Dir_float(float2 p)
                            {
                                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                                p = p % 289;
                                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                                x = (34 * x + 1) * x % 289;
                                x = frac(x / 41) * 2 - 1;
                                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
                            }

                            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
                            {
                                float2 p = UV * Scale;
                                float2 ip = floor(p);
                                float2 fp = frac(p);
                                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
                            }

                            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
                            {
                                Out = lerp(A, B, T);
                            }


                            inline float2 Unity_Voronoi_RandomVector_float(float2 UV, float offset)
                            {
                                float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                                UV = frac(sin(mul(UV, m)) * 46839.32);
                                return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
                            }

                            void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
                            {
                                float2 g = floor(UV * CellDensity);
                                float2 f = frac(UV * CellDensity);
                                float t = 8.0;
                                float3 res = float3(8.0, 0.0, 0.0);

                                for (int y = -1; y <= 1; y++)
                                {
                                    for (int x = -1; x <= 1; x++)
                                    {
                                        float2 lattice = float2(x,y);
                                        float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                                        float d = distance(lattice + offset, f);

                                        if (d < res.x)
                                        {
                                            res = float3(d, offset.x, offset.y);
                                            Out = res.x;
                                            Cells = res.y;
                                        }
                                    }
                                }
                            }

                            void Unity_Power_float(float A, float B, out float Out)
                            {
                                Out = pow(A, B);
                            }

                            void Unity_Multiply_float(float A, float B, out float Out)
                            {
                                Out = A * B;
                            }

                            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                            {
                                Out = A * B;
                            }

                            // Graph Vertex
                            // GraphVertex: <None>

                            // Graph Pixel
                            struct SurfaceDescriptionInputs
                            {
                                float3 TangentSpaceNormal;
                                float4 uv0;
                                float3 TimeParameters;
                            };

                            struct SurfaceDescription
                            {
                                float3 Albedo;
                                float Alpha;
                                float AlphaClipThreshold;
                            };

                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                            {
                                SurfaceDescription surface = (SurfaceDescription)0;
                                float4 _Property_FF70338C_Out_0 = Color_D25B2F83;
                                float4 _UV_AC8399CB_Out_0 = IN.uv0;
                                float2 _Property_FA566B97_Out_0 = Vector2_F89F6FDB;
                                float2 _Multiply_F713A7A3_Out_2;
                                Unity_Multiply_float(_Property_FA566B97_Out_0, (IN.TimeParameters.x.xx), _Multiply_F713A7A3_Out_2);
                                float2 _TilingAndOffset_3C47AE24_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_F713A7A3_Out_2, _TilingAndOffset_3C47AE24_Out_3);
                                float _Property_4C6DBA76_Out_0 = Vector1_5C9FEA09;
                                float _GradientNoise_49BEC5FB_Out_2;
                                Unity_GradientNoise_float(_TilingAndOffset_3C47AE24_Out_3, _Property_4C6DBA76_Out_0, _GradientNoise_49BEC5FB_Out_2);
                                float _Property_F59EB235_Out_0 = Vector1_5D8C30A;
                                float4 _Lerp_4A94D267_Out_3;
                                Unity_Lerp_float4(_UV_AC8399CB_Out_0, (_GradientNoise_49BEC5FB_Out_2.xxxx), (_Property_F59EB235_Out_0.xxxx), _Lerp_4A94D267_Out_3);
                                float4 _SampleTexture2D_A487BAE3_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (_Lerp_4A94D267_Out_3.xy));
                                float _SampleTexture2D_A487BAE3_R_4 = _SampleTexture2D_A487BAE3_RGBA_0.r;
                                float _SampleTexture2D_A487BAE3_G_5 = _SampleTexture2D_A487BAE3_RGBA_0.g;
                                float _SampleTexture2D_A487BAE3_B_6 = _SampleTexture2D_A487BAE3_RGBA_0.b;
                                float _SampleTexture2D_A487BAE3_A_7 = _SampleTexture2D_A487BAE3_RGBA_0.a;
                                float2 _Property_548424FB_Out_0 = Vector2_18F30DF1;
                                float2 _Multiply_60FBCA80_Out_2;
                                Unity_Multiply_float((IN.TimeParameters.x.xx), _Property_548424FB_Out_0, _Multiply_60FBCA80_Out_2);
                                float2 _TilingAndOffset_E4074479_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_60FBCA80_Out_2, _TilingAndOffset_E4074479_Out_3);
                                float _Property_2247BE3E_Out_0 = Vector1_45E24A1A;
                                float _Property_88EB50A3_Out_0 = Vector1_444105F2;
                                float _Voronoi_80D160C3_Out_3;
                                float _Voronoi_80D160C3_Cells_4;
                                Unity_Voronoi_float(_TilingAndOffset_E4074479_Out_3, _Property_2247BE3E_Out_0, _Property_88EB50A3_Out_0, _Voronoi_80D160C3_Out_3, _Voronoi_80D160C3_Cells_4);
                                float _Property_DCD3B7CE_Out_0 = Vector1_B0C7E46C;
                                float _Power_AE954BD6_Out_2;
                                Unity_Power_float(_Voronoi_80D160C3_Out_3, _Property_DCD3B7CE_Out_0, _Power_AE954BD6_Out_2);
                                float _Multiply_1CBE5BB9_Out_2;
                                Unity_Multiply_float(_GradientNoise_49BEC5FB_Out_2, _Power_AE954BD6_Out_2, _Multiply_1CBE5BB9_Out_2);
                                float4 _Multiply_11FBF49E_Out_2;
                                Unity_Multiply_float(_SampleTexture2D_A487BAE3_RGBA_0, (_Multiply_1CBE5BB9_Out_2.xxxx), _Multiply_11FBF49E_Out_2);
                                float4 _SampleTexture2D_57E03179_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_906780E9, samplerTexture2D_906780E9, IN.uv0.xy);
                                float _SampleTexture2D_57E03179_R_4 = _SampleTexture2D_57E03179_RGBA_0.r;
                                float _SampleTexture2D_57E03179_G_5 = _SampleTexture2D_57E03179_RGBA_0.g;
                                float _SampleTexture2D_57E03179_B_6 = _SampleTexture2D_57E03179_RGBA_0.b;
                                float _SampleTexture2D_57E03179_A_7 = _SampleTexture2D_57E03179_RGBA_0.a;
                                float4 _Multiply_CE6129D4_Out_2;
                                Unity_Multiply_float(_Multiply_11FBF49E_Out_2, _SampleTexture2D_57E03179_RGBA_0, _Multiply_CE6129D4_Out_2);
                                float _Property_9D24B5F2_Out_0 = Vector1_CE26F4AA;
                                float4 _Multiply_5A448973_Out_2;
                                Unity_Multiply_float(_Multiply_CE6129D4_Out_2, (_Property_9D24B5F2_Out_0.xxxx), _Multiply_5A448973_Out_2);
                                float4 _Multiply_D2160EC4_Out_2;
                                Unity_Multiply_float(_Property_FF70338C_Out_0, _Multiply_5A448973_Out_2, _Multiply_D2160EC4_Out_2);
                                surface.Albedo = (_Multiply_D2160EC4_Out_2.xyz);
                                surface.Alpha = (_Multiply_5A448973_Out_2).x;
                                surface.AlphaClipThreshold = 0;
                                return surface;
                            }

                            // --------------------------------------------------
                            // Structs and Packing

                            // Generated Type: Attributes
                            struct Attributes
                            {
                                float3 positionOS : POSITION;
                                float3 normalOS : NORMAL;
                                float4 tangentOS : TANGENT;
                                float4 uv0 : TEXCOORD0;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                uint instanceID : INSTANCEID_SEMANTIC;
                                #endif
                            };

                            // Generated Type: Varyings
                            struct Varyings
                            {
                                float4 positionCS : SV_POSITION;
                                float4 texCoord0;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                uint instanceID : CUSTOM_INSTANCE_ID;
                                #endif
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                #endif
                            };

                            // Generated Type: PackedVaryings
                            struct PackedVaryings
                            {
                                float4 positionCS : SV_POSITION;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                uint instanceID : CUSTOM_INSTANCE_ID;
                                #endif
                                float4 interp00 : TEXCOORD0;
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                #endif
                            };

                            // Packed Type: Varyings
                            PackedVaryings PackVaryings(Varyings input)
                            {
                                PackedVaryings output = (PackedVaryings)0;
                                output.positionCS = input.positionCS;
                                output.interp00.xyzw = input.texCoord0;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                output.instanceID = input.instanceID;
                                #endif
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                output.cullFace = input.cullFace;
                                #endif
                                return output;
                            }

                            // Unpacked Type: Varyings
                            Varyings UnpackVaryings(PackedVaryings input)
                            {
                                Varyings output = (Varyings)0;
                                output.positionCS = input.positionCS;
                                output.texCoord0 = input.interp00.xyzw;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                output.instanceID = input.instanceID;
                                #endif
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                output.cullFace = input.cullFace;
                                #endif
                                return output;
                            }

                            // --------------------------------------------------
                            // Build Graph Inputs

                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                            {
                                SurfaceDescriptionInputs output;
                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                                output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                                output.uv0 = input.texCoord0;
                                output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                            #else
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                            #endif
                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                return output;
                            }


                            // --------------------------------------------------
                            // Main

                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

                            ENDHLSL
                        }

        }
            CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
                                FallBack "Hidden/Shader Graph/FallbackError"
}
