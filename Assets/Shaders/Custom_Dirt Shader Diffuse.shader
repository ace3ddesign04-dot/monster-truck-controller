Shader "Custom/Dirt Shader Diffuse"
{
  Properties
  {
    _BaseColor ("Base Color", Color) = (1,1,1,1)
    _DecalLayer1 ("Diffuse", 2D) = "white" {}
    _DecalLayer1Color ("Diffuse Color", Color) = (1,1,1,0)
    _DirtLayer1 ("Dirt Layer 1", 2D) = "white" {}
    _DirtLayer1Color ("Dirt Layer 1 Color", Color) = (1,1,1,0)
    _DirtAlphaCutOff ("Dirt Layer 1 Cut Off", float) = 1
    _DiffuseLightModifier ("Diffuse Light Modifier", Range(0, 1)) = 1
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
      uniform float _DirtAlphaCutOff;
      uniform float _DiffuseLightModifier;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
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
          float3 tmpvar_4;
          float4 tmpvar_5;
          float4 tmpvar_6;
          tmpvar_3.xy = TRANSFORM_TEX(in_v.texcoord.xy, _DecalLayer1);
          float4x4 m_7;
          m_7 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_8;
          float4 tmpvar_9;
          float4 tmpvar_10;
          float4 tmpvar_11;
          tmpvar_8.x = conv_mxt4x4_0(m_7).x;
          tmpvar_8.y = conv_mxt4x4_1(m_7).x;
          tmpvar_8.z = conv_mxt4x4_2(m_7).x;
          tmpvar_8.w = conv_mxt4x4_3(m_7).x;
          tmpvar_9.x = conv_mxt4x4_0(m_7).y;
          tmpvar_9.y = conv_mxt4x4_1(m_7).y;
          tmpvar_9.z = conv_mxt4x4_2(m_7).y;
          tmpvar_9.w = conv_mxt4x4_3(m_7).y;
          tmpvar_10.x = conv_mxt4x4_0(m_7).z;
          tmpvar_10.y = conv_mxt4x4_1(m_7).z;
          tmpvar_10.z = conv_mxt4x4_2(m_7).z;
          tmpvar_10.w = conv_mxt4x4_3(m_7).z;
          tmpvar_11.x = conv_mxt4x4_0(m_7).w;
          tmpvar_11.y = conv_mxt4x4_1(m_7).w;
          tmpvar_11.z = conv_mxt4x4_2(m_7).w;
          tmpvar_11.w = conv_mxt4x4_3(m_7).w;
          float4 v_12;
          v_12.x = tmpvar_8.x;
          v_12.y = tmpvar_9.x;
          v_12.z = tmpvar_10.x;
          v_12.w = tmpvar_11.x;
          float3 tmpvar_13;
          tmpvar_13 = normalize(in_v.normal);
          tmpvar_3.z = dot(normalize(v_12.xyz), tmpvar_13);
          float4x4 m_14;
          m_14 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_15;
          float4 tmpvar_16;
          float4 tmpvar_17;
          float4 tmpvar_18;
          tmpvar_15.x = conv_mxt4x4_0(m_14).x;
          tmpvar_15.y = conv_mxt4x4_1(m_14).x;
          tmpvar_15.z = conv_mxt4x4_2(m_14).x;
          tmpvar_15.w = conv_mxt4x4_3(m_14).x;
          tmpvar_16.x = conv_mxt4x4_0(m_14).y;
          tmpvar_16.y = conv_mxt4x4_1(m_14).y;
          tmpvar_16.z = conv_mxt4x4_2(m_14).y;
          tmpvar_16.w = conv_mxt4x4_3(m_14).y;
          tmpvar_17.x = conv_mxt4x4_0(m_14).z;
          tmpvar_17.y = conv_mxt4x4_1(m_14).z;
          tmpvar_17.z = conv_mxt4x4_2(m_14).z;
          tmpvar_17.w = conv_mxt4x4_3(m_14).z;
          tmpvar_18.x = conv_mxt4x4_0(m_14).w;
          tmpvar_18.y = conv_mxt4x4_1(m_14).w;
          tmpvar_18.z = conv_mxt4x4_2(m_14).w;
          tmpvar_18.w = conv_mxt4x4_3(m_14).w;
          float4 v_19;
          v_19.x = tmpvar_15.y;
          v_19.y = tmpvar_16.y;
          v_19.z = tmpvar_17.y;
          v_19.w = tmpvar_18.y;
          tmpvar_3.w = dot(normalize(v_19.xyz), tmpvar_13);
          tmpvar_3.zw = ((tmpvar_3.zw * 0.5) + 0.5);
          tmpvar_5.xy = TRANSFORM_TEX(in_v.texcoord.xy, _DirtLayer1);
          float4x4 m_20;
          m_20 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_21;
          float4 tmpvar_22;
          float4 tmpvar_23;
          float4 tmpvar_24;
          tmpvar_21.x = conv_mxt4x4_0(m_20).x;
          tmpvar_21.y = conv_mxt4x4_1(m_20).x;
          tmpvar_21.z = conv_mxt4x4_2(m_20).x;
          tmpvar_21.w = conv_mxt4x4_3(m_20).x;
          tmpvar_22.x = conv_mxt4x4_0(m_20).y;
          tmpvar_22.y = conv_mxt4x4_1(m_20).y;
          tmpvar_22.z = conv_mxt4x4_2(m_20).y;
          tmpvar_22.w = conv_mxt4x4_3(m_20).y;
          tmpvar_23.x = conv_mxt4x4_0(m_20).z;
          tmpvar_23.y = conv_mxt4x4_1(m_20).z;
          tmpvar_23.z = conv_mxt4x4_2(m_20).z;
          tmpvar_23.w = conv_mxt4x4_3(m_20).z;
          tmpvar_24.x = conv_mxt4x4_0(m_20).w;
          tmpvar_24.y = conv_mxt4x4_1(m_20).w;
          tmpvar_24.z = conv_mxt4x4_2(m_20).w;
          tmpvar_24.w = conv_mxt4x4_3(m_20).w;
          float4 v_25;
          v_25.x = tmpvar_21.x;
          v_25.y = tmpvar_22.x;
          v_25.z = tmpvar_23.x;
          v_25.w = tmpvar_24.x;
          tmpvar_5.z = dot(normalize(v_25.xyz), tmpvar_13);
          float4x4 m_26;
          m_26 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_27;
          float4 tmpvar_28;
          float4 tmpvar_29;
          float4 tmpvar_30;
          tmpvar_27.x = conv_mxt4x4_0(m_26).x;
          tmpvar_27.y = conv_mxt4x4_1(m_26).x;
          tmpvar_27.z = conv_mxt4x4_2(m_26).x;
          tmpvar_27.w = conv_mxt4x4_3(m_26).x;
          tmpvar_28.x = conv_mxt4x4_0(m_26).y;
          tmpvar_28.y = conv_mxt4x4_1(m_26).y;
          tmpvar_28.z = conv_mxt4x4_2(m_26).y;
          tmpvar_28.w = conv_mxt4x4_3(m_26).y;
          tmpvar_29.x = conv_mxt4x4_0(m_26).z;
          tmpvar_29.y = conv_mxt4x4_1(m_26).z;
          tmpvar_29.z = conv_mxt4x4_2(m_26).z;
          tmpvar_29.w = conv_mxt4x4_3(m_26).z;
          tmpvar_30.x = conv_mxt4x4_0(m_26).w;
          tmpvar_30.y = conv_mxt4x4_1(m_26).w;
          tmpvar_30.z = conv_mxt4x4_2(m_26).w;
          tmpvar_30.w = conv_mxt4x4_3(m_26).w;
          float4 v_31;
          v_31.x = tmpvar_27.y;
          v_31.y = tmpvar_28.y;
          v_31.z = tmpvar_29.y;
          v_31.w = tmpvar_30.y;
          tmpvar_5.w = dot(normalize(v_31.xyz), tmpvar_13);
          tmpvar_5.zw = ((tmpvar_5.zw * 0.5) + 0.5);
          float4 tmpvar_32;
          tmpvar_32.w = 1;
          tmpvar_32.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_33;
          tmpvar_33[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_33[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_33[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_34;
          tmpvar_34 = normalize(mul(in_v.normal, tmpvar_33));
          worldNormal_2 = tmpvar_34;
          float tmpvar_35;
          tmpvar_35 = max(0, dot(worldNormal_2, _WorldSpaceLightPos0.xyz));
          nl_1 = tmpvar_35;
          tmpvar_6 = (nl_1 * _LightColor0);
          float4 tmpvar_36;
          tmpvar_36.w = 1;
          tmpvar_36.xyz = float3(worldNormal_2);
          float3 res_37;
          float3 x_38;
          x_38.x = dot(unity_SHAr, tmpvar_36);
          x_38.y = dot(unity_SHAg, tmpvar_36);
          x_38.z = dot(unity_SHAb, tmpvar_36);
          float3 x1_39;
          float4 tmpvar_40;
          tmpvar_40 = (worldNormal_2.xyzz * worldNormal_2.yzzx);
          x1_39.x = dot(unity_SHBr, tmpvar_40);
          x1_39.y = dot(unity_SHBg, tmpvar_40);
          x1_39.z = dot(unity_SHBb, tmpvar_40);
          res_37 = (x_38 + (x1_39 + (unity_SHC.xyz * ((worldNormal_2.x * worldNormal_2.x) - (worldNormal_2.y * worldNormal_2.y)))));
          float3 tmpvar_41;
          float _tmp_dvx_3 = max(((1.055 * pow(max(res_37, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_41 = float3(_tmp_dvx_3, _tmp_dvx_3, _tmp_dvx_3);
          res_37 = tmpvar_41;
          tmpvar_6.xyz = (tmpvar_6.xyz + tmpvar_41);
          out_v.xlv_TEXCOORD0 = tmpvar_3;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_32));
          out_v.xlv_TEXCOORD1 = tmpvar_4;
          out_v.xlv_TEXCOORD2 = tmpvar_5;
          out_v.xlv_COLOR0 = tmpvar_6;
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
          tmpvar_3 = tex2D(_DecalLayer1, in_f.xlv_TEXCOORD0.xy);
          float4 tmpvar_4;
          tmpvar_4 = (tmpvar_3 * _DecalLayer1Color);
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_DirtLayer1, in_f.xlv_TEXCOORD2.xy);
          float4 tmpvar_6;
          tmpvar_6 = (tmpvar_5 * _DirtLayer1Color);
          float3 tmpvar_7;
          tmpvar_7 = lerp(_BaseColor.xyz, tmpvar_4.xyz, tmpvar_4.www);
          finalColor_2 = tmpvar_7;
          if((tmpvar_6.w>_DirtAlphaCutOff))
          {
              finalColor_2 = lerp(tmpvar_7, tmpvar_6.xyz, tmpvar_6.www);
          }
          tmpvar_1 = (in_f.xlv_COLOR0 * (in_f.xlv_COLOR0 * in_f.xlv_COLOR0));
          finalColor_2 = (((finalColor_2 * tmpvar_1.xyz) + finalColor_2) * _DiffuseLightModifier);
          float4 tmpvar_8;
          tmpvar_8.w = 1;
          tmpvar_8.xyz = float3(finalColor_2);
          out_f.color = tmpvar_8;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
