Shader "HologramShader"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _HologramTex("HologramTex", 2D) = "white" {}
        _HologramSpeed("HologramSpeed", Float) = 0.5
        _Color("Color", Color) = (0.5230064, 0.6813684, 0.6886792, 0)
        _UnscaledTime("UnscaledTime", Float) = 0
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"=""
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEUNLIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _HologramTex_TexelSize;
        float4 _HologramTex_ST;
        float _HologramSpeed;
        float4 _Color;
        float _UnscaledTime;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_HologramTex);
        SAMPLER(sampler_HologramTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_RandomRange_float(float2 Seed, float Min, float Max, out float Out)
        {
             float randomno =  frac(sin(dot(Seed, float2(12.9898, 78.233)))*43758.5453);
             Out = lerp(Min, Max, randomno);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_20d9f4f6100e4971ab98116745cdac31_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0 = SAMPLE_TEXTURE2D(_Property_20d9f4f6100e4971ab98116745cdac31_Out_0.tex, _Property_20d9f4f6100e4971ab98116745cdac31_Out_0.samplerstate, _Property_20d9f4f6100e4971ab98116745cdac31_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_R_4 = _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0.r;
            float _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_G_5 = _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0.g;
            float _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_B_6 = _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0.b;
            float _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_A_7 = _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0.a;
            float4 _Property_c2b97d22135a4b03bfef14c08dfca3f5_Out_0 = _Color;
            float4 _Multiply_d8d836fabcec4a139945dedd9c50bce4_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0, _Property_c2b97d22135a4b03bfef14c08dfca3f5_Out_0, _Multiply_d8d836fabcec4a139945dedd9c50bce4_Out_2);
            float _Property_34d70ca2a9924326a932a1e6d673d0e9_Out_0 = _UnscaledTime;
            float _RandomRange_204c89a0b601428d897ac15c42a43c18_Out_3;
            Unity_RandomRange_float((_Property_34d70ca2a9924326a932a1e6d673d0e9_Out_0.xx), 0.85, 1, _RandomRange_204c89a0b601428d897ac15c42a43c18_Out_3);
            float4 _Multiply_c81f7bed3cec433789f5704b215748d2_Out_2;
            Unity_Multiply_float4_float4(_Multiply_d8d836fabcec4a139945dedd9c50bce4_Out_2, (_RandomRange_204c89a0b601428d897ac15c42a43c18_Out_3.xxxx), _Multiply_c81f7bed3cec433789f5704b215748d2_Out_2);
            UnityTexture2D _Property_4d0b6310937c47198ead2162977d2411_Out_0 = UnityBuildTexture2DStruct(_HologramTex);
            float _Property_e4186ac8e31743e1a1492730ccbce1f1_Out_0 = _UnscaledTime;
            float _Property_1a35d3bee0f94720bfaa6403db2dc794_Out_0 = _HologramSpeed;
            float _Multiply_637093f6b4704c8c8e62796a87804237_Out_2;
            Unity_Multiply_float_float(_Property_e4186ac8e31743e1a1492730ccbce1f1_Out_0, _Property_1a35d3bee0f94720bfaa6403db2dc794_Out_0, _Multiply_637093f6b4704c8c8e62796a87804237_Out_2);
            float2 _TilingAndOffset_a579c8c1b3604339afbeaa423fd049f0_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_637093f6b4704c8c8e62796a87804237_Out_2.xx), _TilingAndOffset_a579c8c1b3604339afbeaa423fd049f0_Out_3);
            float4 _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4d0b6310937c47198ead2162977d2411_Out_0.tex, _Property_4d0b6310937c47198ead2162977d2411_Out_0.samplerstate, _Property_4d0b6310937c47198ead2162977d2411_Out_0.GetTransformedUV(_TilingAndOffset_a579c8c1b3604339afbeaa423fd049f0_Out_3));
            float _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_R_4 = _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0.r;
            float _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_G_5 = _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0.g;
            float _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_B_6 = _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0.b;
            float _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_A_7 = _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0.a;
            float4 _Multiply_d96c420461024c4b8257d56386f20912_Out_2;
            Unity_Multiply_float4_float4((_SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_A_7.xxxx), _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0, _Multiply_d96c420461024c4b8257d56386f20912_Out_2);
            surface.BaseColor = (_Multiply_c81f7bed3cec433789f5704b215748d2_Out_2.xyz);
            surface.Alpha = (_Multiply_d96c420461024c4b8257d56386f20912_Out_2).x;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
            ENDHLSL
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEFORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _HologramTex_TexelSize;
        float4 _HologramTex_ST;
        float _HologramSpeed;
        float4 _Color;
        float _UnscaledTime;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_HologramTex);
        SAMPLER(sampler_HologramTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_RandomRange_float(float2 Seed, float Min, float Max, out float Out)
        {
             float randomno =  frac(sin(dot(Seed, float2(12.9898, 78.233)))*43758.5453);
             Out = lerp(Min, Max, randomno);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_20d9f4f6100e4971ab98116745cdac31_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0 = SAMPLE_TEXTURE2D(_Property_20d9f4f6100e4971ab98116745cdac31_Out_0.tex, _Property_20d9f4f6100e4971ab98116745cdac31_Out_0.samplerstate, _Property_20d9f4f6100e4971ab98116745cdac31_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_R_4 = _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0.r;
            float _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_G_5 = _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0.g;
            float _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_B_6 = _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0.b;
            float _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_A_7 = _SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0.a;
            float4 _Property_c2b97d22135a4b03bfef14c08dfca3f5_Out_0 = _Color;
            float4 _Multiply_d8d836fabcec4a139945dedd9c50bce4_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_RGBA_0, _Property_c2b97d22135a4b03bfef14c08dfca3f5_Out_0, _Multiply_d8d836fabcec4a139945dedd9c50bce4_Out_2);
            float _Property_34d70ca2a9924326a932a1e6d673d0e9_Out_0 = _UnscaledTime;
            float _RandomRange_204c89a0b601428d897ac15c42a43c18_Out_3;
            Unity_RandomRange_float((_Property_34d70ca2a9924326a932a1e6d673d0e9_Out_0.xx), 0.85, 1, _RandomRange_204c89a0b601428d897ac15c42a43c18_Out_3);
            float4 _Multiply_c81f7bed3cec433789f5704b215748d2_Out_2;
            Unity_Multiply_float4_float4(_Multiply_d8d836fabcec4a139945dedd9c50bce4_Out_2, (_RandomRange_204c89a0b601428d897ac15c42a43c18_Out_3.xxxx), _Multiply_c81f7bed3cec433789f5704b215748d2_Out_2);
            UnityTexture2D _Property_4d0b6310937c47198ead2162977d2411_Out_0 = UnityBuildTexture2DStruct(_HologramTex);
            float _Property_e4186ac8e31743e1a1492730ccbce1f1_Out_0 = _UnscaledTime;
            float _Property_1a35d3bee0f94720bfaa6403db2dc794_Out_0 = _HologramSpeed;
            float _Multiply_637093f6b4704c8c8e62796a87804237_Out_2;
            Unity_Multiply_float_float(_Property_e4186ac8e31743e1a1492730ccbce1f1_Out_0, _Property_1a35d3bee0f94720bfaa6403db2dc794_Out_0, _Multiply_637093f6b4704c8c8e62796a87804237_Out_2);
            float2 _TilingAndOffset_a579c8c1b3604339afbeaa423fd049f0_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_637093f6b4704c8c8e62796a87804237_Out_2.xx), _TilingAndOffset_a579c8c1b3604339afbeaa423fd049f0_Out_3);
            float4 _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4d0b6310937c47198ead2162977d2411_Out_0.tex, _Property_4d0b6310937c47198ead2162977d2411_Out_0.samplerstate, _Property_4d0b6310937c47198ead2162977d2411_Out_0.GetTransformedUV(_TilingAndOffset_a579c8c1b3604339afbeaa423fd049f0_Out_3));
            float _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_R_4 = _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0.r;
            float _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_G_5 = _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0.g;
            float _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_B_6 = _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0.b;
            float _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_A_7 = _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0.a;
            float4 _Multiply_d96c420461024c4b8257d56386f20912_Out_2;
            Unity_Multiply_float4_float4((_SampleTexture2D_781486b3a49e4dc8b9d8d366d57585d6_A_7.xxxx), _SampleTexture2D_fc7eb90f1aad4f7cb41eeb46d78b8768_RGBA_0, _Multiply_d96c420461024c4b8257d56386f20912_Out_2);
            surface.BaseColor = (_Multiply_c81f7bed3cec433789f5704b215748d2_Out_2.xyz);
            surface.Alpha = (_Multiply_d96c420461024c4b8257d56386f20912_Out_2).x;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}