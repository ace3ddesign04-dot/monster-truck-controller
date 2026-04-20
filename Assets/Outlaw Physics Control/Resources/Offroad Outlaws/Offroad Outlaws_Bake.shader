Shader "Offroad Outlaws/Bake"
{
  Properties
  {
    _PaintColor ("Paint color", Color) = (1,1,1,1)
    _Wrap0 ("Wrap0", 2D) = "black" {}
    _WrapColor0 ("Wrap0 Color", Color) = (1,1,1,0)
    _Wrap1 ("Wrap1", 2D) = "black" {}
    _WrapColor1 ("Wrap1 Color", Color) = (1,1,1,0)
    _Wrap2 ("Wrap2", 2D) = "black" {}
    _WrapColor2 ("Wrap2 Color", Color) = (1,1,1,0)
    _Wrap3 ("Wrap3", 2D) = "black" {}
    _WrapColor3 ("Wrap3 Color", Color) = (1,1,1,0)
    _Wrap4 ("Wrap4", 2D) = "black" {}
    _WrapColor4 ("Wrap4 Color", Color) = (1,1,1,0)
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
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Wrap0_ST;
      uniform float4 _Wrap1_ST;
      uniform float4 _Wrap2_ST;
      uniform float4 _Wrap3_ST;
      uniform float4 _Wrap4_ST;
      uniform sampler2D _Wrap0;
      uniform sampler2D _Wrap1;
      uniform sampler2D _Wrap2;
      uniform sampler2D _Wrap3;
      uniform sampler2D _Wrap4;
      uniform float4 _WrapColor0;
      uniform float4 _WrapColor1;
      uniform float4 _WrapColor2;
      uniform float4 _WrapColor3;
      uniform float4 _WrapColor4;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float2 xlv_TEXCOORD3 :TEXCOORD3;
          float2 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float2 xlv_TEXCOORD3 :TEXCOORD3;
          float2 xlv_TEXCOORD4 :TEXCOORD4;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          float2 tmpvar_2;
          float2 tmpvar_3;
          float2 tmpvar_4;
          float2 tmpvar_5;
          tmpvar_1 = TRANSFORM_TEX(in_v.texcoord.xy, _Wrap0);
          tmpvar_2 = TRANSFORM_TEX(in_v.texcoord.xy, _Wrap1);
          tmpvar_3 = TRANSFORM_TEX(in_v.texcoord.xy, _Wrap2);
          tmpvar_4 = TRANSFORM_TEX(in_v.texcoord.xy, _Wrap3);
          tmpvar_5 = TRANSFORM_TEX(in_v.texcoord.xy, _Wrap4);
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = in_v.vertex.xyz;
          out_v.xlv_TEXCOORD0 = tmpvar_1;
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = tmpvar_3;
          out_v.xlv_TEXCOORD3 = tmpvar_4;
          out_v.xlv_TEXCOORD4 = tmpvar_5;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_6));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 finalColor_1;
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_Wrap0, in_f.xlv_TEXCOORD0);
          float4 tmpvar_3;
          tmpvar_3 = (tmpvar_2 * _WrapColor0);
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_Wrap1, in_f.xlv_TEXCOORD1);
          float4 tmpvar_5;
          tmpvar_5 = (tmpvar_4 * _WrapColor1);
          float4 tmpvar_6;
          tmpvar_6 = tex2D(_Wrap2, in_f.xlv_TEXCOORD2);
          float4 tmpvar_7;
          tmpvar_7 = (tmpvar_6 * _WrapColor2);
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_Wrap3, in_f.xlv_TEXCOORD3);
          float4 tmpvar_9;
          tmpvar_9 = (tmpvar_8 * _WrapColor3);
          float4 tmpvar_10;
          tmpvar_10 = tex2D(_Wrap4, in_f.xlv_TEXCOORD4);
          float4 tmpvar_11;
          tmpvar_11 = (tmpvar_10 * _WrapColor4);
          finalColor_1.xyz = lerp(lerp(lerp(lerp((tmpvar_3 * tmpvar_3.wwww), tmpvar_5, tmpvar_5.wwww), tmpvar_7, tmpvar_7.wwww), tmpvar_9, tmpvar_9.wwww), tmpvar_11, tmpvar_11.wwww).xyz;
          finalColor_1.w = ((tmpvar_3.w + tmpvar_5.w) + ((tmpvar_7.w + tmpvar_9.w) + tmpvar_11.w));
          out_f.color = finalColor_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
