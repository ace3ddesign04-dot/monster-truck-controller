// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

Shader "Custom/EVP Tire Marks"
{
  Properties
  {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    _Glossiness ("Smoothness", Range(0, 1)) = 0.5
    _Metallic ("Metallic", Range(0, 1)) = 0
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Geometry+1"
      "RenderType" = "Opaque"
    }
    LOD 200
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Geometry+1"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 200
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform samplerCUBE unity_SpecCube0;
      //uniform float4 unity_SpecCube0_HDR;
      uniform float4 _LightColor0;
      uniform sampler2D unity_NHxRoughness;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform float _Glossiness;
      uniform float _Metallic;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float2 xlv_TEXCOORD5 :TEXCOORD5;
          float4 xlv_TEXCOORD7 :TEXCOORD7;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1 = in_v.color;
          float3 worldNormal_2;
          float3 tmpvar_3;
          float4 tmpvar_4;
          float2 tmpvar_5;
          float4 tmpvar_6;
          tmpvar_4 = tmpvar_1;
          float4 tmpvar_7;
          tmpvar_7.w = 1;
          tmpvar_7.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_8;
          tmpvar_8[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_8[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_8[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_9;
          tmpvar_9 = normalize(mul(in_v.normal, tmpvar_8));
          worldNormal_2 = tmpvar_9;
          tmpvar_3 = worldNormal_2;
          float3 normal_10;
          normal_10 = worldNormal_2;
          float3 x1_11;
          float4 tmpvar_12;
          tmpvar_12 = (normal_10.xyzz * normal_10.yzzx);
          x1_11.x = dot(unity_SHBr, tmpvar_12);
          x1_11.y = dot(unity_SHBg, tmpvar_12);
          x1_11.z = dot(unity_SHBb, tmpvar_12);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_7));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_3;
          out_v.xlv_TEXCOORD2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_TEXCOORD3 = tmpvar_4;
          out_v.xlv_TEXCOORD4 = (x1_11 + (unity_SHC.xyz * ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y))));
          out_v.xlv_TEXCOORD5 = tmpvar_5;
          out_v.xlv_TEXCOORD7 = tmpvar_6;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 impl_low_textureCubeLodEXT(samplerCUBE sampler, float3 coord, float lod)
      {
          #if defined( GL_EXT_shader_texture_lod)
          {
              return texCUBE(sampler, float4(coord, lod));
              #else
              return texCUBE(sampler, coord, lod);
              #endif
          }
      
          OUT_Data_Frag frag(v2f in_f)
          {
              float3 tmpvar_1;
              float4 tmpvar_2;
              float3 tmpvar_3;
              float3 tmpvar_4;
              float4 c_5;
              float3 tmpvar_6;
              float3 tmpvar_7;
              float tmpvar_8;
              float3 worldViewDir_9;
              float3 lightDir_10;
              float4 tmpvar_11;
              tmpvar_11 = in_f.xlv_TEXCOORD3;
              float3 tmpvar_12;
              tmpvar_12 = _WorldSpaceLightPos0.xyz;
              lightDir_10 = tmpvar_12;
              float3 tmpvar_13;
              tmpvar_13 = normalize((_WorldSpaceCameraPos - in_f.xlv_TEXCOORD2));
              worldViewDir_9 = tmpvar_13;
              tmpvar_7 = in_f.xlv_TEXCOORD1;
              float4 tmpvar_14;
              tmpvar_14 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
              tmpvar_6 = tmpvar_14.xyz;
              tmpvar_8 = (tmpvar_14.w * tmpvar_11.w);
              tmpvar_3 = _LightColor0.xyz;
              tmpvar_4 = lightDir_10;
              tmpvar_1 = worldViewDir_9;
              tmpvar_2 = unity_SpecCube0_HDR;
              float3 Normal_15;
              Normal_15 = tmpvar_7;
              float tmpvar_16;
              tmpvar_16 = (1 - _Glossiness);
              float3 I_17;
              I_17 = (-tmpvar_1);
              float3 normalWorld_18;
              normalWorld_18 = tmpvar_7;
              float4 tmpvar_19;
              tmpvar_19.w = 1;
              tmpvar_19.xyz = float3(normalWorld_18);
              float3 x_20;
              x_20.x = dot(unity_SHAr, tmpvar_19);
              x_20.y = dot(unity_SHAg, tmpvar_19);
              x_20.z = dot(unity_SHAb, tmpvar_19);
              float4 hdr_21;
              hdr_21 = tmpvar_2;
              float4 tmpvar_22;
              tmpvar_22.xyz = float3((I_17 - (2 * (dot(Normal_15, I_17) * Normal_15))));
              tmpvar_22.w = ((tmpvar_16 * (1.7 - (0.7 * tmpvar_16))) * 6);
              float4 tmpvar_23;
              tmpvar_23 = impl_low_textureCubeLodEXT(unity_SpecCube0, tmpvar_22.xyz, tmpvar_22.w);
              float4 tmpvar_24;
              tmpvar_24 = tmpvar_23;
              float3 tmpvar_25;
              float3 viewDir_26;
              viewDir_26 = worldViewDir_9;
              float4 c_27;
              float3 tmpvar_28;
              tmpvar_28 = normalize(tmpvar_7);
              float3 tmpvar_29;
              float3 albedo_30;
              albedo_30 = tmpvar_6;
              float3 tmpvar_31;
              tmpvar_31 = lerp(float3(0.2209163, 0.2209163, 0.2209163), albedo_30, float3(_Metallic, _Metallic, _Metallic));
              float tmpvar_32;
              tmpvar_32 = (0.7790837 - (_Metallic * 0.7790837));
              tmpvar_29 = (albedo_30 * tmpvar_32);
              tmpvar_25 = tmpvar_29;
              float3 diffColor_33;
              diffColor_33 = tmpvar_25;
              float alpha_34;
              alpha_34 = tmpvar_8;
              tmpvar_25 = diffColor_33;
              float3 diffColor_35;
              diffColor_35 = tmpvar_25;
              float3 normal_36;
              normal_36 = tmpvar_28;
              float3 color_37;
              float2 tmpvar_38;
              tmpvar_38.x = dot((viewDir_26 - (2 * (dot(normal_36, viewDir_26) * normal_36))), tmpvar_4);
              tmpvar_38.y = (1 - clamp(dot(normal_36, viewDir_26), 0, 1));
              float2 tmpvar_39;
              tmpvar_39 = ((tmpvar_38 * tmpvar_38) * (tmpvar_38 * tmpvar_38));
              float2 tmpvar_40;
              tmpvar_40.x = tmpvar_39.x;
              tmpvar_40.y = tmpvar_16;
              float4 tmpvar_41;
              tmpvar_41 = tex2D(unity_NHxRoughness, tmpvar_40);
              color_37 = ((diffColor_35 + ((tmpvar_41.w * 16) * tmpvar_31)) * (tmpvar_3 * clamp(dot(normal_36, tmpvar_4), 0, 1)));
              float _tmp_dvx_10 = clamp((_Glossiness + (1 - tmpvar_32)), 0, 1);
              color_37 = (color_37 + ((max(((1.055 * pow(max(float3(0, 0, 0), (in_f.xlv_TEXCOORD4 + x_20)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0)) * diffColor_35) + (((hdr_21.x * ((hdr_21.w * (tmpvar_24.w - 1)) + 1)) * tmpvar_24.xyz) * lerp(tmpvar_31, float3(_tmp_dvx_10, _tmp_dvx_10, _tmp_dvx_10), tmpvar_39.yyy))));
              float4 tmpvar_42;
              tmpvar_42.w = 1;
              tmpvar_42.xyz = float3(color_37);
              c_27.xyz = tmpvar_42.xyz;
              c_27.w = alpha_34;
              c_5 = c_27;
              out_f.color = c_5;
          }
      
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDADD"
        "QUEUE" = "Geometry+1"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 200
      ZWrite Off
      Blend SrcAlpha One
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D unity_NHxRoughness;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform float _Glossiness;
      uniform float _Metallic;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float2 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1 = in_v.color;
          float3 worldNormal_2;
          float3 tmpvar_3;
          float4 tmpvar_4;
          float2 tmpvar_5;
          tmpvar_4 = tmpvar_1;
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_7;
          tmpvar_7[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_7[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_7[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_8;
          tmpvar_8 = normalize(mul(in_v.normal, tmpvar_7));
          worldNormal_2 = tmpvar_8;
          tmpvar_3 = worldNormal_2;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_6));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_3;
          out_v.xlv_TEXCOORD2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_TEXCOORD3 = tmpvar_4;
          out_v.xlv_TEXCOORD4 = tmpvar_5;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float4 c_3;
          float3 lightCoord_4;
          float3 tmpvar_5;
          float3 tmpvar_6;
          float tmpvar_7;
          float3 worldViewDir_8;
          float3 lightDir_9;
          float4 tmpvar_10;
          tmpvar_10 = in_f.xlv_TEXCOORD3;
          float3 tmpvar_11;
          tmpvar_11 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD2));
          lightDir_9 = tmpvar_11;
          float3 tmpvar_12;
          tmpvar_12 = normalize((_WorldSpaceCameraPos - in_f.xlv_TEXCOORD2));
          worldViewDir_8 = tmpvar_12;
          tmpvar_6 = in_f.xlv_TEXCOORD1;
          float4 tmpvar_13;
          tmpvar_13 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
          tmpvar_5 = tmpvar_13.xyz;
          tmpvar_7 = (tmpvar_13.w * tmpvar_10.w);
          float4 tmpvar_14;
          tmpvar_14.w = 1;
          tmpvar_14.xyz = in_f.xlv_TEXCOORD2;
          lightCoord_4 = mul(unity_WorldToLight, tmpvar_14).xyz;
          float tmpvar_15;
          tmpvar_15 = dot(lightCoord_4, lightCoord_4);
          float tmpvar_16;
          tmpvar_16 = tex2D(_LightTexture0, float2(tmpvar_15, tmpvar_15)).w;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_9;
          tmpvar_1 = (tmpvar_1 * tmpvar_16);
          float3 tmpvar_17;
          float3 viewDir_18;
          viewDir_18 = worldViewDir_8;
          float4 c_19;
          float3 tmpvar_20;
          tmpvar_20 = normalize(tmpvar_6);
          float3 tmpvar_21;
          float3 albedo_22;
          albedo_22 = tmpvar_5;
          tmpvar_21 = (albedo_22 * (0.7790837 - (_Metallic * 0.7790837)));
          tmpvar_17 = tmpvar_21;
          float3 diffColor_23;
          diffColor_23 = tmpvar_17;
          float alpha_24;
          alpha_24 = tmpvar_7;
          tmpvar_17 = diffColor_23;
          float3 diffColor_25;
          diffColor_25 = tmpvar_17;
          float3 normal_26;
          normal_26 = tmpvar_20;
          float2 tmpvar_27;
          tmpvar_27.x = dot((viewDir_18 - (2 * (dot(normal_26, viewDir_18) * normal_26))), tmpvar_2);
          tmpvar_27.y = (1 - clamp(dot(normal_26, viewDir_18), 0, 1));
          float2 tmpvar_28;
          tmpvar_28.x = ((tmpvar_27 * tmpvar_27) * (tmpvar_27 * tmpvar_27)).x;
          tmpvar_28.y = (1 - _Glossiness);
          float4 tmpvar_29;
          tmpvar_29 = tex2D(unity_NHxRoughness, tmpvar_28);
          float4 tmpvar_30;
          tmpvar_30.w = 1;
          tmpvar_30.xyz = ((diffColor_25 + ((tmpvar_29.w * 16) * lerp(float3(0.2209163, 0.2209163, 0.2209163), albedo_22, float3(_Metallic, _Metallic, _Metallic)))) * (tmpvar_1 * clamp(dot(normal_26, tmpvar_2), 0, 1)));
          c_19.xyz = tmpvar_30.xyz;
          c_19.w = alpha_24;
          c_3 = c_19;
          out_f.color = c_3;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: META
    {
      Name "META"
      Tags
      { 
        "LIGHTMODE" = "META"
        "QUEUE" = "Geometry+1"
        "RenderType" = "Opaque"
      }
      LOD 200
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _MainTex_ST;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform float4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1 = in_v.color;
          float4 tmpvar_2;
          tmpvar_2 = tmpvar_1;
          float4 vertex_3;
          vertex_3 = in_v.vertex;
          if(unity_MetaVertexControl.x)
          {
              vertex_3.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_4;
              if((in_v.vertex.z>0))
              {
                  tmpvar_4 = 0.0001;
              }
              else
              {
                  tmpvar_4 = 0;
              }
              vertex_3.z = tmpvar_4;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_3.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_5;
              if((vertex_3.z>0))
              {
                  tmpvar_5 = 0.0001;
              }
              else
              {
                  tmpvar_5 = 0;
              }
              vertex_3.z = tmpvar_5;
          }
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = vertex_3.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_6));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_TEXCOORD2 = tmpvar_2;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          tmpvar_3 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color).xyz;
          tmpvar_2 = tmpvar_3;
          float4 res_4;
          res_4 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_5;
              tmpvar_5.w = 1;
              tmpvar_5.xyz = float3(tmpvar_2);
              res_4.w = tmpvar_5.w;
              float3 tmpvar_6;
              float _tmp_dvx_11 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_6 = clamp(pow(tmpvar_2, float3(_tmp_dvx_11, _tmp_dvx_11, _tmp_dvx_11)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_4.xyz = float3(tmpvar_6);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_7;
              if(int(unity_UseLinearSpace))
              {
                  emission_7 = float3(0, 0, 0);
              }
              else
              {
                  emission_7 = float3(0, 0, 0);
              }
              float4 tmpvar_8;
              tmpvar_8.w = 1;
              tmpvar_8.xyz = float3(emission_7);
              res_4 = tmpvar_8;
          }
          tmpvar_1 = res_4;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
