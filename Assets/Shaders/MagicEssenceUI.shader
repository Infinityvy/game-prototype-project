Shader "Unlit/MagicEssenceUI"
{
    Properties
    {
        _Color("Color", Color) = (0.1539539, 1, 0, 1)
        _RippleColor("RippleColor", Color) = (1, 1, 1, 1)
        _RippleDensity("RippleDensity", Float) = 80
        _RippleSlimness("RippleSlimness", Float) = 5
        _RippleSpeed("RippleSpeed", Float) = 1
        _UV_Scale_Factor("UV Scale Factor", Vector) = (1, 1, 0, 0)
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Geometry"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
Name"Universal Forward"
            Tags
{
                // LightMode: <None>
}
        
        // Render State
Cull Back

Blend One
Zero
        ZTest
LEqual
        ZWrite
On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        // GraphKeywords: <None>
        
        // Defines
        
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_POSITION_WS
#define VARYINGS_NEED_NORMAL_WS
#define VARYINGS_NEED_TEXCOORD0
#define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
#define SHADERPASS SHADERPASS_UNLIT
#define _FOG_FRAGMENT 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
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
struct Varyings
{
    float4 positionCS : SV_POSITION;
    float3 positionWS;
    float3 normalWS;
    float4 texCoord0;
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
    float3 TimeParameters;
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
    float4 texCoord0 : INTERP0;
    float3 positionWS : INTERP1;
    float3 normalWS : INTERP2;
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
        
PackedVaryings PackVaryings(Varyings input)
{
    PackedVaryings output;
    ZERO_INITIALIZE(PackedVaryings, output);
    output.positionCS = input.positionCS;
    output.texCoord0.xyzw = input.texCoord0;
    output.positionWS.xyz = input.positionWS;
    output.normalWS.xyz = input.normalWS;
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
        
Varyings UnpackVaryings(PackedVaryings input)
{
    Varyings output;
    output.positionCS = input.positionCS;
    output.texCoord0 = input.texCoord0.xyzw;
    output.positionWS = input.positionWS.xyz;
    output.normalWS = input.normalWS.xyz;
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
float4 _Color;
float4 _RippleColor;
float _RippleDensity;
float _RippleSlimness;
float _RippleSpeed;
float2 _UV_Scale_Factor;
        CBUFFER_END
        
        
        // Object and Global properties
        
        // Graph Includes
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
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
        
void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}
        
void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}
        
void Unity_Multiply_float_float(float A, float B, out float Out)
{
    Out = A * B;
}
        
float2 Unity_Voronoi_RandomVector_Deterministic_float(float2 UV, float offset)
{
    Hash_Tchou_2_2_float(UV, UV);
    return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
}
        
void Unity_Voronoi_Deterministic_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
{
    float2 g = floor(UV * CellDensity);
    float2 f = frac(UV * CellDensity);
    float t = 8.0;
    float3 res = float3(8.0, 0.0, 0.0);
    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x, y);
            float2 offset = Unity_Voronoi_RandomVector_Deterministic_float(lattice + g, AngleOffset);
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
        
void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}
        
void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
    Out = A + B;
}
        
        // Custom interpolators pre vertex
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
    VertexDescription description = (VertexDescription) 0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}
        
        // Custom interpolators, pre surface
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
};
        
SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription) 0;
    float4 _Property_44a7e18e5fb541298a87cf97fb3a0419_Out_0_Vector4 = _Color;
    float4 _UV_10bad61f7c8a4030aa08c15c0f18975a_Out_0_Vector4 = IN.uv0;
    float2 _Property_48fdea6600e0438895d67721b1af7a9c_Out_0_Vector2 = _UV_Scale_Factor;
    float2 _Multiply_d468949bd4b943d181986daa42c19e81_Out_2_Vector2;
    Unity_Multiply_float2_float2((_UV_10bad61f7c8a4030aa08c15c0f18975a_Out_0_Vector4.xy), _Property_48fdea6600e0438895d67721b1af7a9c_Out_0_Vector2, _Multiply_d468949bd4b943d181986daa42c19e81_Out_2_Vector2);
    float _Float_7ebeefe73a1a404793eb364c769210ae_Out_0_Float = 10;
    float _Add_ddf33814fc5e48eeb2fb7da6b1e96fc2_Out_2_Float;
    Unity_Add_float(_Float_7ebeefe73a1a404793eb364c769210ae_Out_0_Float, IN.TimeParameters.x, _Add_ddf33814fc5e48eeb2fb7da6b1e96fc2_Out_2_Float);
    float _Property_967fe1e115f0494d91ce1bf2386d2897_Out_0_Float = _RippleSpeed;
    float _Multiply_b740481fe5644bdfa1062be284cecea8_Out_2_Float;
    Unity_Multiply_float_float(_Add_ddf33814fc5e48eeb2fb7da6b1e96fc2_Out_2_Float, _Property_967fe1e115f0494d91ce1bf2386d2897_Out_0_Float, _Multiply_b740481fe5644bdfa1062be284cecea8_Out_2_Float);
    float _Property_60a0adc851a7441db41931fcd1a90b38_Out_0_Float = _RippleDensity;
    float _Voronoi_dab2a8327c8e447a8ba1c7733d4c4de9_Out_3_Float;
    float _Voronoi_dab2a8327c8e447a8ba1c7733d4c4de9_Cells_4_Float;
    Unity_Voronoi_Deterministic_float(_Multiply_d468949bd4b943d181986daa42c19e81_Out_2_Vector2, _Multiply_b740481fe5644bdfa1062be284cecea8_Out_2_Float, _Property_60a0adc851a7441db41931fcd1a90b38_Out_0_Float, _Voronoi_dab2a8327c8e447a8ba1c7733d4c4de9_Out_3_Float, _Voronoi_dab2a8327c8e447a8ba1c7733d4c4de9_Cells_4_Float);
    float _Property_8c5811e5632343bd857cce9b06f8017d_Out_0_Float = _RippleSlimness;
    float _Power_a2273ad72bc5489c8a2380e5c87d4eaf_Out_2_Float;
    Unity_Power_float(_Voronoi_dab2a8327c8e447a8ba1c7733d4c4de9_Out_3_Float, _Property_8c5811e5632343bd857cce9b06f8017d_Out_0_Float, _Power_a2273ad72bc5489c8a2380e5c87d4eaf_Out_2_Float);
    float4 _Property_4e256232985049cb8e4952b558b8061b_Out_0_Vector4 = _RippleColor;
    float4 _Multiply_30678e1a4a4542c98252b0bdf940e413_Out_2_Vector4;
    Unity_Multiply_float4_float4((_Power_a2273ad72bc5489c8a2380e5c87d4eaf_Out_2_Float.xxxx), _Property_4e256232985049cb8e4952b558b8061b_Out_0_Vector4, _Multiply_30678e1a4a4542c98252b0bdf940e413_Out_2_Vector4);
    float _Split_a352e22a8201497e825a623d05e03e38_R_1_Float = _Property_4e256232985049cb8e4952b558b8061b_Out_0_Vector4[0];
    float _Split_a352e22a8201497e825a623d05e03e38_G_2_Float = _Property_4e256232985049cb8e4952b558b8061b_Out_0_Vector4[1];
    float _Split_a352e22a8201497e825a623d05e03e38_B_3_Float = _Property_4e256232985049cb8e4952b558b8061b_Out_0_Vector4[2];
    float _Split_a352e22a8201497e825a623d05e03e38_A_4_Float = _Property_4e256232985049cb8e4952b558b8061b_Out_0_Vector4[3];
    float4 _Multiply_dd2d91daca1344c980b5f1e4325b49aa_Out_2_Vector4;
    Unity_Multiply_float4_float4(_Multiply_30678e1a4a4542c98252b0bdf940e413_Out_2_Vector4, (_Split_a352e22a8201497e825a623d05e03e38_A_4_Float.xxxx), _Multiply_dd2d91daca1344c980b5f1e4325b49aa_Out_2_Vector4);
    float4 _Add_c6f0a7ed92bf4f46b04ce0846fbcf465_Out_2_Vector4;
    Unity_Add_float4(_Property_44a7e18e5fb541298a87cf97fb3a0419_Out_0_Vector4, _Multiply_dd2d91daca1344c980b5f1e4325b49aa_Out_2_Vector4, _Add_c6f0a7ed92bf4f46b04ce0846fbcf465_Out_2_Vector4);
    surface.BaseColor = (_Add_c6f0a7ed92bf4f46b04ce0846fbcf465_Out_2_Vector4.xyz);
    return surface;
}
        
        // --------------------------------------------------
        // Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES Attributes
#define VFX_SRP_VARYINGS Varyings
#define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
#endif
VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;
        
    return output;
}
SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
#ifdef HAVE_VFX_MODIFICATION
#if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
#endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
#endif
        
            
        
        
        
        
        
        
#if UNITY_UV_STARTS_AT_TOP
#else
#endif
        
        
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
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
CustomEditor"UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline"UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
FallBack"Hidden/Shader Graph/FallbackError"
}
