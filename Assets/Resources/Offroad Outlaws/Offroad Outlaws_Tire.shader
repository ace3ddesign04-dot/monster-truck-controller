Shader "Offroad Outlaws/Tire"
{
  Properties
  {
    [PerRendererData] _BaseColor ("Tire Color", Color) = (1,1,1,1)
    [PerRendererData] _Dirt ("Dirt", 2D) = "black" {}
    [PerRendererData] _DirtColor ("Dirt Color", Color) = (0.88,0.62,0.3,0)
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
      uniform float4 _Dirt_ST;
      uniform sampler2D _Dirt;
      uniform float4 _BaseColor;
      uniform float4 _DirtColor;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_COLOR0 :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 tmpvar_1;
          tmpvar_1 = in_v.normal;
          float3 worldN_2;
          float3 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = in_v.vertex.xyz;
          float3 norm_5;
          norm_5 = tmpvar_1;
          float3x3 tmpvar_6;
          tmpvar_6[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_6[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_6[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_7;
          tmpvar_7 = normalize(mul(norm_5, tmpvar_6));
          worldN_2 = tmpvar_7;
          float4 tmpvar_8;
          tmpvar_8.w = 1;
          tmpvar_8.xyz = float3(worldN_2);
          float3 res_9;
          float3 x_10;
          x_10.x = dot(unity_SHAr, tmpvar_8);
          x_10.y = dot(unity_SHAg, tmpvar_8);
          x_10.z = dot(unity_SHAb, tmpvar_8);
          float3 x1_11;
          float4 tmpvar_12;
          tmpvar_12 = (worldN_2.xyzz * worldN_2.yzzx);
          x1_11.x = dot(unity_SHBr, tmpvar_12);
          x1_11.y = dot(unity_SHBg, tmpvar_12);
          x1_11.z = dot(unity_SHBb, tmpvar_12);
          res_9 = (x_10 + (x1_11 + (unity_SHC.xyz * ((worldN_2.x * worldN_2.x) - (worldN_2.y * worldN_2.y)))));
          float3 tmpvar_13;
          float _tmp_dvx_13 = max(((1.055 * pow(max(res_9, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_13 = float3(_tmp_dvx_13, _tmp_dvx_13, _tmp_dvx_13);
          res_9 = tmpvar_13;
          float tmpvar_14;
          tmpvar_14 = max(0, dot(worldN_2, _WorldSpaceLightPos0.xyz));
          tmpvar_3 = (tmpvar_13 + (_LightColor0.xyz * tmpvar_14));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _Dirt);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_4));
          out_v.xlv_COLOR0 = tmpvar_3;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 targetDirt_2;
          float4 finalColor_3;
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_Dirt, in_f.xlv_TEXCOORD0);
          float4 tmpvar_5;
          tmpvar_5 = (tmpvar_4 * _DirtColor);
          targetDirt_2.xyz = max(_BaseColor, tmpvar_5).xyz;
          targetDirt_2.w = tmpvar_5.w;
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = in_f.xlv_COLOR0;
          float _tmp_dvx_14 = (1 - _DirtColor.w);
          finalColor_3 = (lerp(_BaseColor, (targetDirt_2 * 0.8), float4(_tmp_dvx_14, _tmp_dvx_14, _tmp_dvx_14, _tmp_dvx_14)) * tmpvar_6);
          tmpvar_1 = finalColor_3;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
