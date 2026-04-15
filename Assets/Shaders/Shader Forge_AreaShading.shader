Shader "Shader Forge/AreaShading"
{
  Properties
  {
    _Slope ("Slope", Range(-1, 1)) = 0.5152014
    _Color ("Color", Color) = (0,1,1,1)
    _Transparency ("Transparency", Range(0, 1)) = 0
    _GradientScale ("GradientScale", Range(0, 1)) = 1
    [MaterialToggle] _UseGradient ("UseGradient", float) = 0
    [HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
    _StencilComp ("Stencil Comparison", float) = 8
    _Stencil ("Stencil ID", float) = 0
    _StencilOp ("Stencil Operation", float) = 0
    _StencilWriteMask ("Stencil Write Mask", float) = 255
    _StencilReadMask ("Stencil Read Mask", float) = 255
    _ColorMask ("Color Mask", float) = 15
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "ALWAYS"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
        "SHADOWSUPPORT" = "true"
      }
      ZWrite Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float _Slope;
      uniform float4 _Color;
      uniform float _Transparency;
      uniform float _GradientScale;
      uniform float _UseGradient;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
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
          tmpvar_1.w = 1;
          tmpvar_1.xyz = in_v.vertex.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 finalRGBA_2;
          float node_5552_if_leB_3;
          float node_5552_if_leA_4;
          float tmpvar_5;
          tmpvar_5 = clamp(_Slope, (-1), 1);
          float tmpvar_6;
          tmpvar_6 = float((0>=tmpvar_5));
          float tmpvar_7;
          tmpvar_7 = float((tmpvar_5>=0));
          float2x2 tmpvar_8;
          conv_mxt2x2_0(tmpvar_8).x = (-1);
          conv_mxt2x2_0(tmpvar_8).y = (-3.258414E-07);
          conv_mxt2x2_1(tmpvar_8).x = 3.258414E-07;
          conv_mxt2x2_1(tmpvar_8).y = (-1);
          float2 tmpvar_9;
          tmpvar_9.x = (-tmpvar_5);
          tmpvar_9.y = 1;
          float2x2 tmpvar_10;
          conv_mxt2x2_0(tmpvar_10).x = 0.7071068;
          conv_mxt2x2_0(tmpvar_10).y = 0.7071068;
          conv_mxt2x2_1(tmpvar_10).x = (-0.7071068);
          conv_mxt2x2_1(tmpvar_10).y = 0.7071068;
          float2 tmpvar_11;
          tmpvar_11.x = 1;
          tmpvar_11.y = tmpvar_5;
          float2x2 tmpvar_12;
          conv_mxt2x2_0(tmpvar_12).x = (-1);
          conv_mxt2x2_0(tmpvar_12).y = (-3.258414E-07);
          conv_mxt2x2_1(tmpvar_12).x = 3.258414E-07;
          conv_mxt2x2_1(tmpvar_12).y = (-1);
          float2 tmpvar_13;
          tmpvar_13.x = tmpvar_5;
          tmpvar_13.y = 1;
          float2x2 tmpvar_14;
          conv_mxt2x2_0(tmpvar_14).x = 0.7071068;
          conv_mxt2x2_0(tmpvar_14).y = 0.7071068;
          conv_mxt2x2_1(tmpvar_14).x = (-0.7071068);
          conv_mxt2x2_1(tmpvar_14).y = 0.7071068;
          float tmpvar_15;
          tmpvar_15 = float((0.5>=(mul((((mul((in_f.xlv_TEXCOORD0 - tmpvar_11), tmpvar_12) + tmpvar_11) * tmpvar_13) - float2(0.5, 0.5)), tmpvar_14) + float2(0.5, 0.5)).y));
          float x_16;
          x_16 = (lerp(((tmpvar_6 * float((0.5>=(mul((((mul((in_f.xlv_TEXCOORD0 - float2(0.5, 0.5)), tmpvar_8) + float2(0.5, 0.5)) * tmpvar_9) - float2(0.5, 0.5)), tmpvar_10) + float2(0.5, 0.5)).x))) + (tmpvar_7 * tmpvar_15)), tmpvar_15, (tmpvar_6 * tmpvar_7)) - 0.5);
          if((x_16<0))
          {
              discard;
          }
          float tmpvar_17;
          tmpvar_17 = float((1>=_UseGradient));
          node_5552_if_leA_4 = tmpvar_17;
          float tmpvar_18;
          tmpvar_18 = float((_UseGradient>=1));
          node_5552_if_leB_3 = tmpvar_18;
          float2x2 tmpvar_19;
          conv_mxt2x2_0(tmpvar_19).x = (-1);
          conv_mxt2x2_0(tmpvar_19).y = (-3.258414E-07);
          conv_mxt2x2_1(tmpvar_19).x = 3.258414E-07;
          conv_mxt2x2_1(tmpvar_19).y = (-1);
          float2 tmpvar_20;
          tmpvar_20.x = 1;
          tmpvar_20.y = _GradientScale;
          float4 tmpvar_21;
          tmpvar_21.xyz = _Color.xyz;
          float _tmp_dvx_16 = (node_5552_if_leA_4 * node_5552_if_leB_3);
          tmpvar_21.w = (lerp((float3(node_5552_if_leA_4, node_5552_if_leA_4, node_5552_if_leA_4) + float3(node_5552_if_leB_3, node_5552_if_leB_3, node_5552_if_leB_3)), ((mul((in_f.xlv_TEXCOORD0 - float2(0.5, 0.5)), tmpvar_19) + float2(0.5, 0.5)) * tmpvar_20).yyy, float3(_tmp_dvx_16, _tmp_dvx_16, _tmp_dvx_16)) * (float3(1, 1, 1) - float3(_Transparency, _Transparency, _Transparency))).x;
          finalRGBA_2 = tmpvar_21;
          tmpvar_1 = finalRGBA_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
