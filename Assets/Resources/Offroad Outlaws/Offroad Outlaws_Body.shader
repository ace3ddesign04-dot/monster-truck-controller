Shader "Offroad Outlaws/Body"
{
  Properties
  {
    [PerRendererData] _PaintColor ("Paint color", Color) = (1,1,1,1)
    [PerRendererData] _BakedWrap ("Baked wrap", 2D) = "black" {}
    [PerRendererData] _Wrap ("Wrap", 2D) = "black" {}
    [PerRendererData] _WrapColor ("Wrap Color", Color) = (1,1,1,0)
    [PerRendererData] _Dirt ("Dirt", 2D) = "black" {}
    [PerRendererData] _DirtColor ("Dirt Color", Color) = (0.88,0.62,0.3,0)
    [PerRendererData] _ReflectionStrength ("Reflection Strength", Range(0, 1)) = 1
  }
  SubShader
  {
    Tags
    { 
      "LIGHTMODE" = "FORWARDBASE"
      "QUEUE" = "Geometry"
      "RenderType" = "Opaque"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Geometry"
        "RenderType" = "Opaque"
      }
      Fog
      { 
        Mode  Linear
      } 
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _LightColor0;
      uniform float4 _Wrap_ST;
      uniform float4 _BakedWrap_ST;
      uniform float4 _Dirt_ST;
      //uniform samplerCUBE unity_SpecCube0;
      uniform sampler2D _BakedWrap;
      uniform sampler2D _Wrap;
      uniform sampler2D _Dirt;
      uniform float4 _PaintColor;
      uniform float4 _WrapColor;
      uniform float4 _DirtColor;
      uniform float _ReflectionStrength;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_COLOR0 :COLOR0;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_COLOR0 :COLOR0;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2.w = 1;
          tmpvar_2.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_3;
          tmpvar_3[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_3[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_3[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_4;
          tmpvar_4 = normalize(mul(tmpvar_3, in_v.normal));
          float3 I_5;
          I_5 = (mul(unity_ObjectToWorld, in_v.vertex).xyz - _WorldSpaceCameraPos);
          float3x3 tmpvar_6;
          tmpvar_6[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_6[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_6[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_7;
          tmpvar_7 = normalize(mul(in_v.normal, tmpvar_6));
          float4 tmpvar_8;
          tmpvar_8.w = 1;
          tmpvar_8.xyz = float3(tmpvar_7);
          float4 normal_9;
          normal_9 = tmpvar_8;
          float3 res_10;
          float3 x_11;
          x_11.x = dot(unity_SHAr, normal_9);
          x_11.y = dot(unity_SHAg, normal_9);
          x_11.z = dot(unity_SHAb, normal_9);
          float3 x1_12;
          float4 tmpvar_13;
          tmpvar_13 = (normal_9.xyzz * normal_9.yzzx);
          x1_12.x = dot(unity_SHBr, tmpvar_13);
          x1_12.y = dot(unity_SHBg, tmpvar_13);
          x1_12.z = dot(unity_SHBb, tmpvar_13);
          res_10 = (x_11 + (x1_12 + (unity_SHC.xyz * ((normal_9.x * normal_9.x) - (normal_9.y * normal_9.y)))));
          float3 tmpvar_14;
          float _tmp_dvx_4 = max(((1.055 * pow(max(res_10, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_14 = float3(_tmp_dvx_4, _tmp_dvx_4, _tmp_dvx_4);
          res_10 = tmpvar_14;
          tmpvar_1 = tmpvar_14;
          tmpvar_1 = (tmpvar_1 + (_LightColor0.xyz * max(0, dot(tmpvar_7, _WorldSpaceLightPos0.xyz))));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _Wrap);
          out_v.xlv_TEXCOORD1 = TRANSFORM_TEX(in_v.texcoord.xy, _BakedWrap);
          out_v.xlv_TEXCOORD2 = TRANSFORM_TEX(in_v.texcoord1.xy, _Dirt);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_2));
          out_v.xlv_COLOR0 = tmpvar_1;
          out_v.xlv_TEXCOORD3 = (I_5 - (2 * (dot(tmpvar_4, I_5) * tmpvar_4)));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 reflectionColor_1;
          float4 dirtTex_2;
          float3 finalColor_3;
          float4 bakedWrapColor_4;
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_BakedWrap, in_f.xlv_TEXCOORD1);
          bakedWrapColor_4 = tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6 = tex2D(_Wrap, in_f.xlv_TEXCOORD0);
          float4 tmpvar_7;
          tmpvar_7 = (tmpvar_6 * _WrapColor);
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_Dirt, in_f.xlv_TEXCOORD2);
          dirtTex_2 = tmpvar_8;
          float tmpvar_9;
          tmpvar_9 = float((dirtTex_2.w>=clamp(_DirtColor.w, 0.05, 1)));
          float3 tmpvar_10;
          tmpvar_10 = texCUBE(unity_SpecCube0, in_f.xlv_TEXCOORD3).xyz;
          reflectionColor_1 = tmpvar_10;
          reflectionColor_1 = (reflectionColor_1 * ((((((reflectionColor_1.x * 0.3) + (reflectionColor_1.y * 0.59)) + (reflectionColor_1.z * 0.11)) * _ReflectionStrength) * 2) * (1 - tmpvar_9)));
          float _tmp_dvx_5 = (dirtTex_2.w * tmpvar_9);
          finalColor_3 = (lerp(lerp(lerp(_PaintColor.xyz, bakedWrapColor_4.xyz, bakedWrapColor_4.www), tmpvar_7.xyz, tmpvar_7.www), (dirtTex_2 * _DirtColor).xyz, float3(_tmp_dvx_5, _tmp_dvx_5, _tmp_dvx_5)) + reflectionColor_1);
          finalColor_3 = (finalColor_3 * in_f.xlv_COLOR0);
          float4 tmpvar_11;
          tmpvar_11.w = 1;
          tmpvar_11.xyz = float3(finalColor_3);
          out_f.color = tmpvar_11;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
