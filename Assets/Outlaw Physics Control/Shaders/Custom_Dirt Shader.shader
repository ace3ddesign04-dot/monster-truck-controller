Shader "Custom/Dirt Shader"
{
  Properties
  {
    _BaseColor ("Base Color", Color) = (1,1,1,1)
    _DecalLayer1 ("Decal Layer 1", 2D) = "white" {}
    _DecalLayer1Color ("Decal Layer 1 Color", Color) = (1,1,1,0)
    _DirtLayer1 ("Dirt Layer 1", 2D) = "white" {}
    _DirtLayer1Color ("Dirt Layer 1 Color", Color) = (1,1,1,0)
    _DirtAlphaCutOff ("Dirt Layer 1 Cut Off", float) = 1
    _MatCapLookup ("MatCap Lookup", 2D) = "white" {}
    _UseMatCap ("Use Mat Cap", Range(0, 1)) = 1
    _DiffuseLightModifier ("Diffuse Light Modifier", Range(0, 1)) = 1
    _ReflectionColor ("Reflection Color", Color) = (1,1,1,1)
    _ReflectionMap ("Reflection Map", Cube) = "" {}
    _ReflectionStrength ("Reflection Strength", Range(0, 1)) = 0.5
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
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
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
      //uniform float4x4 unity_MatrixInvV;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _LightColor0;
      uniform float4 _DecalLayer1_ST;
      uniform float4 _DirtLayer1_ST;
      uniform float4 _BaseColor;
      uniform float4 _DecalLayer1Color;
      uniform float4 _DirtLayer1Color;
      uniform sampler2D _DecalLayer1;
      uniform sampler2D _DirtLayer1;
      uniform sampler2D _MatCapLookup;
      uniform float4 _ReflectionColor;
      uniform samplerCUBE _ReflectionMap;
      uniform float _ReflectionStrength;
      uniform float _DirtAlphaCutOff;
      uniform float _DiffuseLightModifier;
      uniform int _UseMatCap;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_COLOR0 :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float nl_1;
          float3 worldNormal_2;
          float4 tmpvar_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          tmpvar_3.xy = TRANSFORM_TEX(in_v.texcoord.xy, _DecalLayer1);
          float4x4 m_6;
          m_6 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_7;
          float4 tmpvar_8;
          float4 tmpvar_9;
          float4 tmpvar_10;
          tmpvar_7.x = conv_mxt4x4_0(m_6).x;
          tmpvar_7.y = conv_mxt4x4_1(m_6).x;
          tmpvar_7.z = conv_mxt4x4_2(m_6).x;
          tmpvar_7.w = conv_mxt4x4_3(m_6).x;
          tmpvar_8.x = conv_mxt4x4_0(m_6).y;
          tmpvar_8.y = conv_mxt4x4_1(m_6).y;
          tmpvar_8.z = conv_mxt4x4_2(m_6).y;
          tmpvar_8.w = conv_mxt4x4_3(m_6).y;
          tmpvar_9.x = conv_mxt4x4_0(m_6).z;
          tmpvar_9.y = conv_mxt4x4_1(m_6).z;
          tmpvar_9.z = conv_mxt4x4_2(m_6).z;
          tmpvar_9.w = conv_mxt4x4_3(m_6).z;
          tmpvar_10.x = conv_mxt4x4_0(m_6).w;
          tmpvar_10.y = conv_mxt4x4_1(m_6).w;
          tmpvar_10.z = conv_mxt4x4_2(m_6).w;
          tmpvar_10.w = conv_mxt4x4_3(m_6).w;
          float4 v_11;
          v_11.x = tmpvar_7.x;
          v_11.y = tmpvar_8.x;
          v_11.z = tmpvar_9.x;
          v_11.w = tmpvar_10.x;
          float3 tmpvar_12;
          tmpvar_12 = normalize(in_v.normal);
          tmpvar_3.z = dot(normalize(v_11.xyz), tmpvar_12);
          float4x4 m_13;
          m_13 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_14;
          float4 tmpvar_15;
          float4 tmpvar_16;
          float4 tmpvar_17;
          tmpvar_14.x = conv_mxt4x4_0(m_13).x;
          tmpvar_14.y = conv_mxt4x4_1(m_13).x;
          tmpvar_14.z = conv_mxt4x4_2(m_13).x;
          tmpvar_14.w = conv_mxt4x4_3(m_13).x;
          tmpvar_15.x = conv_mxt4x4_0(m_13).y;
          tmpvar_15.y = conv_mxt4x4_1(m_13).y;
          tmpvar_15.z = conv_mxt4x4_2(m_13).y;
          tmpvar_15.w = conv_mxt4x4_3(m_13).y;
          tmpvar_16.x = conv_mxt4x4_0(m_13).z;
          tmpvar_16.y = conv_mxt4x4_1(m_13).z;
          tmpvar_16.z = conv_mxt4x4_2(m_13).z;
          tmpvar_16.w = conv_mxt4x4_3(m_13).z;
          tmpvar_17.x = conv_mxt4x4_0(m_13).w;
          tmpvar_17.y = conv_mxt4x4_1(m_13).w;
          tmpvar_17.z = conv_mxt4x4_2(m_13).w;
          tmpvar_17.w = conv_mxt4x4_3(m_13).w;
          float4 v_18;
          v_18.x = tmpvar_14.y;
          v_18.y = tmpvar_15.y;
          v_18.z = tmpvar_16.y;
          v_18.w = tmpvar_17.y;
          tmpvar_3.w = dot(normalize(v_18.xyz), tmpvar_12);
          tmpvar_3.zw = ((tmpvar_3.zw * 0.5) + 0.5);
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord1.xy, _DirtLayer1);
          float4x4 m_19;
          m_19 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_20;
          float4 tmpvar_21;
          float4 tmpvar_22;
          float4 tmpvar_23;
          tmpvar_20.x = conv_mxt4x4_0(m_19).x;
          tmpvar_20.y = conv_mxt4x4_1(m_19).x;
          tmpvar_20.z = conv_mxt4x4_2(m_19).x;
          tmpvar_20.w = conv_mxt4x4_3(m_19).x;
          tmpvar_21.x = conv_mxt4x4_0(m_19).y;
          tmpvar_21.y = conv_mxt4x4_1(m_19).y;
          tmpvar_21.z = conv_mxt4x4_2(m_19).y;
          tmpvar_21.w = conv_mxt4x4_3(m_19).y;
          tmpvar_22.x = conv_mxt4x4_0(m_19).z;
          tmpvar_22.y = conv_mxt4x4_1(m_19).z;
          tmpvar_22.z = conv_mxt4x4_2(m_19).z;
          tmpvar_22.w = conv_mxt4x4_3(m_19).z;
          tmpvar_23.x = conv_mxt4x4_0(m_19).w;
          tmpvar_23.y = conv_mxt4x4_1(m_19).w;
          tmpvar_23.z = conv_mxt4x4_2(m_19).w;
          tmpvar_23.w = conv_mxt4x4_3(m_19).w;
          float4 v_24;
          v_24.x = tmpvar_20.x;
          v_24.y = tmpvar_21.x;
          v_24.z = tmpvar_22.x;
          v_24.w = tmpvar_23.x;
          tmpvar_4.z = dot(normalize(v_24.xyz), tmpvar_12);
          float4x4 m_25;
          m_25 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_26;
          float4 tmpvar_27;
          float4 tmpvar_28;
          float4 tmpvar_29;
          tmpvar_26.x = conv_mxt4x4_0(m_25).x;
          tmpvar_26.y = conv_mxt4x4_1(m_25).x;
          tmpvar_26.z = conv_mxt4x4_2(m_25).x;
          tmpvar_26.w = conv_mxt4x4_3(m_25).x;
          tmpvar_27.x = conv_mxt4x4_0(m_25).y;
          tmpvar_27.y = conv_mxt4x4_1(m_25).y;
          tmpvar_27.z = conv_mxt4x4_2(m_25).y;
          tmpvar_27.w = conv_mxt4x4_3(m_25).y;
          tmpvar_28.x = conv_mxt4x4_0(m_25).z;
          tmpvar_28.y = conv_mxt4x4_1(m_25).z;
          tmpvar_28.z = conv_mxt4x4_2(m_25).z;
          tmpvar_28.w = conv_mxt4x4_3(m_25).z;
          tmpvar_29.x = conv_mxt4x4_0(m_25).w;
          tmpvar_29.y = conv_mxt4x4_1(m_25).w;
          tmpvar_29.z = conv_mxt4x4_2(m_25).w;
          tmpvar_29.w = conv_mxt4x4_3(m_25).w;
          float4 v_30;
          v_30.x = tmpvar_26.y;
          v_30.y = tmpvar_27.y;
          v_30.z = tmpvar_28.y;
          v_30.w = tmpvar_29.y;
          tmpvar_4.w = dot(normalize(v_30.xyz), tmpvar_12);
          tmpvar_4.zw = ((tmpvar_4.zw * 0.5) + 0.5);
          float4 tmpvar_31;
          tmpvar_31.w = 1;
          tmpvar_31.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_32;
          tmpvar_32[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_32[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_32[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_33;
          tmpvar_33 = normalize(mul(tmpvar_32, in_v.normal));
          float3 I_34;
          I_34 = (mul(unity_ObjectToWorld, in_v.vertex).xyz - _WorldSpaceCameraPos);
          float3x3 tmpvar_35;
          tmpvar_35[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_35[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_35[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_36;
          tmpvar_36 = normalize(mul(in_v.normal, tmpvar_35));
          worldNormal_2 = tmpvar_36;
          float tmpvar_37;
          tmpvar_37 = max(0, dot(worldNormal_2, _WorldSpaceLightPos0.xyz));
          nl_1 = tmpvar_37;
          tmpvar_5 = (nl_1 * _LightColor0);
          float4 tmpvar_38;
          tmpvar_38.w = 1;
          tmpvar_38.xyz = float3(worldNormal_2);
          float3 res_39;
          float3 x_40;
          x_40.x = dot(unity_SHAr, tmpvar_38);
          x_40.y = dot(unity_SHAg, tmpvar_38);
          x_40.z = dot(unity_SHAb, tmpvar_38);
          float3 x1_41;
          float4 tmpvar_42;
          tmpvar_42 = (worldNormal_2.xyzz * worldNormal_2.yzzx);
          x1_41.x = dot(unity_SHBr, tmpvar_42);
          x1_41.y = dot(unity_SHBg, tmpvar_42);
          x1_41.z = dot(unity_SHBb, tmpvar_42);
          res_39 = (x_40 + (x1_41 + (unity_SHC.xyz * ((worldNormal_2.x * worldNormal_2.x) - (worldNormal_2.y * worldNormal_2.y)))));
          float3 tmpvar_43;
          float _tmp_dvx_15 = max(((1.055 * pow(max(res_39, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_43 = float3(_tmp_dvx_15, _tmp_dvx_15, _tmp_dvx_15);
          res_39 = tmpvar_43;
          tmpvar_5.xyz = (tmpvar_5.xyz + tmpvar_43);
          out_v.xlv_TEXCOORD0 = tmpvar_3;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_31));
          out_v.xlv_TEXCOORD1 = (I_34 - (2 * (dot(tmpvar_33, I_34) * tmpvar_33)));
          out_v.xlv_TEXCOORD2 = tmpvar_4;
          out_v.xlv_COLOR0 = tmpvar_5;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1 = in_f.xlv_COLOR0;
          float3 finalColor_2;
          float4 tmpvar_3;
          tmpvar_3 = texCUBE(_ReflectionMap, in_f.xlv_TEXCOORD1);
          float3 tmpvar_4;
          tmpvar_4 = (tmpvar_3.xyz * _ReflectionColor.xyz);
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_DecalLayer1, in_f.xlv_TEXCOORD0.xy);
          float4 tmpvar_6;
          tmpvar_6 = (tmpvar_5 * _DecalLayer1Color);
          float4 tmpvar_7;
          tmpvar_7 = tex2D(_DirtLayer1, in_f.xlv_TEXCOORD2.xy);
          float4 tmpvar_8;
          tmpvar_8 = (tmpvar_7 * _DirtLayer1Color);
          float3 tmpvar_9;
          tmpvar_9 = lerp(_BaseColor.xyz, tmpvar_6.xyz, tmpvar_6.www);
          finalColor_2 = tmpvar_9;
          if((_UseMatCap>0))
          {
              float3 matCapColor_10;
              float3 tmpvar_11;
              tmpvar_11 = tex2D(_MatCapLookup, in_f.xlv_TEXCOORD0.zw).xyz;
              matCapColor_10 = tmpvar_11;
              float4 tmpvar_12;
              tmpvar_12.xyz = float3(((tmpvar_9 * matCapColor_10) * 2));
              tmpvar_12.w = _BaseColor.w;
              finalColor_2 = tmpvar_12.xyz;
          }
          float3 tmpvar_13;
          tmpvar_13 = lerp(finalColor_2, tmpvar_4, float3(_ReflectionStrength, _ReflectionStrength, _ReflectionStrength));
          finalColor_2 = tmpvar_13;
          if((tmpvar_8.w>_DirtAlphaCutOff))
          {
              finalColor_2 = lerp(tmpvar_13, tmpvar_8.xyz, tmpvar_8.www);
          }
          tmpvar_1 = (in_f.xlv_COLOR0 * (in_f.xlv_COLOR0 * in_f.xlv_COLOR0));
          finalColor_2 = (((finalColor_2 * tmpvar_1.xyz) + finalColor_2) * _DiffuseLightModifier);
          float4 tmpvar_14;
          tmpvar_14.w = 1;
          tmpvar_14.xyz = float3(finalColor_2);
          out_f.color = tmpvar_14;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
