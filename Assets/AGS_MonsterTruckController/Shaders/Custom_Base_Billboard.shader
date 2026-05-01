Shader "Custom/Base_Billboard"
{
  Properties
  {
    _Diffuse ("Diffuse", 2D) = "white" {}
    _Diffcolor ("Diff color", Color) = (1,1,1,1)
    _OpacityClip ("Opacity Clip", float) = 1
    [HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "AlphaTest"
      "RenderType" = "TransparentCutout"
    }
    LOD 200
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "AlphaTest"
        "RenderType" = "TransparentCutout"
        "SHADOWSUPPORT" = "true"
      }
      LOD 200
      Blend SrcAlpha OneMinusSrcAlpha
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
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4 glstate_lightmodel_ambient;
      uniform float4 _LightColor0;
      uniform sampler2D _Diffuse;
      uniform float4 _Diffuse_ST;
      uniform float4 _Diffcolor;
      uniform float _OpacityClip;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3x3 tmpvar_1;
          tmpvar_1[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_1[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_1[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float4 tmpvar_2;
          tmpvar_2.w = 1;
          tmpvar_2.xyz = in_v.vertex.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_2));
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          out_v.xlv_TEXCOORD1 = mul(unity_ObjectToWorld, in_v.vertex);
          out_v.xlv_TEXCOORD2 = normalize(mul(in_v.normal, tmpvar_1));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 finalRGBA_2;
          float3 lightDirection_3;
          float4 _Diffuse_var_4;
          float4 tmpvar_5;
          float2 P_6;
          P_6 = TRANSFORM_TEX(in_f.xlv_TEXCOORD0, _Diffuse);
          tmpvar_5 = tex2D(_Diffuse, P_6);
          _Diffuse_var_4 = tmpvar_5;
          float x_7;
          x_7 = ((_Diffuse_var_4.w * _OpacityClip) - 0.5);
          if((x_7<0))
          {
              discard;
          }
          float3 tmpvar_8;
          tmpvar_8 = normalize(_WorldSpaceLightPos0.xyz);
          lightDirection_3 = tmpvar_8;
          float4 tmpvar_9;
          tmpvar_9.w = 1;
          tmpvar_9.xyz = (((max(0, dot(normalize(in_f.xlv_TEXCOORD2), lightDirection_3)) * _LightColor0.xyz) + (glstate_lightmodel_ambient * 2).xyz) * (_Diffcolor.xyz + _Diffuse_var_4.xyz));
          finalRGBA_2 = tmpvar_9;
          tmpvar_1 = finalRGBA_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD_DELTA
    {
      Name "FORWARD_DELTA"
      Tags
      { 
        "LIGHTMODE" = "FORWARDADD"
        "QUEUE" = "AlphaTest"
        "RenderType" = "TransparentCutout"
        "SHADOWSUPPORT" = "true"
      }
      LOD 200
      Blend One One
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
      uniform float4x4 unity_WorldToLight;
      //uniform float4 _WorldSpaceLightPos0;
      uniform sampler2D _LightTexture0;
      uniform float4 _LightColor0;
      uniform sampler2D _Diffuse;
      uniform float4 _Diffuse_ST;
      uniform float4 _Diffcolor;
      uniform float _OpacityClip;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3x3 tmpvar_1;
          tmpvar_1[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_1[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_1[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float4 tmpvar_2;
          tmpvar_2.w = 1;
          tmpvar_2.xyz = in_v.vertex.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_2));
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          float4 tmpvar_3;
          tmpvar_3 = mul(unity_ObjectToWorld, in_v.vertex);
          out_v.xlv_TEXCOORD1 = tmpvar_3;
          out_v.xlv_TEXCOORD2 = normalize(mul(in_v.normal, tmpvar_1));
          out_v.xlv_TEXCOORD3 = mul(unity_WorldToLight, tmpvar_3).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 finalRGBA_2;
          float attenuation_3;
          float4 _Diffuse_var_4;
          float4 tmpvar_5;
          float2 P_6;
          P_6 = TRANSFORM_TEX(in_f.xlv_TEXCOORD0, _Diffuse);
          tmpvar_5 = tex2D(_Diffuse, P_6);
          _Diffuse_var_4 = tmpvar_5;
          float x_7;
          x_7 = ((_Diffuse_var_4.w * _OpacityClip) - 0.5);
          if((x_7<0))
          {
              discard;
          }
          float tmpvar_8;
          tmpvar_8 = dot(in_f.xlv_TEXCOORD3, in_f.xlv_TEXCOORD3);
          float tmpvar_9;
          tmpvar_9 = tex2D(_LightTexture0, float2(tmpvar_8, tmpvar_8)).w;
          attenuation_3 = tmpvar_9;
          float4 tmpvar_10;
          tmpvar_10.w = 0;
          tmpvar_10.xyz = ((max(0, dot(normalize(in_f.xlv_TEXCOORD2), normalize(lerp(_WorldSpaceLightPos0.xyz, (_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD1.xyz), _WorldSpaceLightPos0.www)))) * (attenuation_3 * _LightColor0.xyz)) * (_Diffcolor.xyz + _Diffuse_var_4.xyz));
          finalRGBA_2 = tmpvar_10;
          tmpvar_1 = finalRGBA_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: SHADOWCASTER
    {
      Name "SHADOWCASTER"
      Tags
      { 
        "LIGHTMODE" = "SHADOWCASTER"
        "QUEUE" = "AlphaTest"
        "RenderType" = "TransparentCutout"
        "SHADOWSUPPORT" = "true"
      }
      LOD 200
      Offset 1, 1
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile SHADOWS_DEPTH
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 unity_LightShadowBias;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _Diffuse;
      uniform float4 _Diffuse_ST;
      uniform float _OpacityClip;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2.w = 1;
          tmpvar_2.xyz = in_v.vertex.xyz;
          tmpvar_1 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_2));
          float4 clipPos_3;
          clipPos_3.xyw = tmpvar_1.xyw;
          clipPos_3.z = (tmpvar_1.z + clamp((unity_LightShadowBias.x / tmpvar_1.w), 0, 1));
          clipPos_3.z = lerp(clipPos_3.z, max(clipPos_3.z, (-tmpvar_1.w)), unity_LightShadowBias.y);
          out_v.vertex = clipPos_3;
          out_v.xlv_TEXCOORD1 = in_v.texcoord.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 _Diffuse_var_1;
          float4 tmpvar_2;
          float2 P_3;
          P_3 = TRANSFORM_TEX(in_f.xlv_TEXCOORD1, _Diffuse);
          tmpvar_2 = tex2D(_Diffuse, P_3);
          _Diffuse_var_1 = tmpvar_2;
          float x_4;
          x_4 = ((_Diffuse_var_1.w * _OpacityClip) - 0.5);
          if((x_4<0))
          {
              discard;
          }
          out_f.color = float4(0, 0, 0, 0);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
