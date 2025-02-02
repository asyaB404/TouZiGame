Shader "Unlit/sky"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SunColor("SunColor",COLOR)=(1,1,1,1)
        _SunSize("SunSize",float)=0.1
         _MoonColor("MoonColor",COLOR)=(1,1,1,1)
        _MoonSize("MoonSize",float)=0.1
        _CrescentOffset("CrescentOfffset",float)=1
        _CreScentSize("CreScentSize",float)=0.06
        _DayBottomColor("DayBottomColor",COLOR)=(1,1,1,1)
         _DayTopColor("DayTopColor",COLOR)=(1,1,1,1)
         _NightBottomColor("NightBottomColor",COLOR)=(1,1,1,1)
         _NightTopColor("NightTopColor",COLOR)=(1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Background" 
               "Queue"="Background"
        }
        Cull Off 
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _SunColor;
            float _SunSize;
            float4 _MoonColor;
            float _MoonSize;
            float  _CrescentOffset;
            float _CreScentSize;
            float4 _DayBottomColor;
            float4 _DayTopColor;
            float4 _NightBottomColor;
            float4 _NightTopColor;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv=v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            fixed3 skycolor(v2f i)
            {
            fixed3 gradientday=lerp(_DayBottomColor,_DayTopColor,saturate(i.uv.y));
             fixed3 gradientnight=lerp(_NightBottomColor,_NightTopColor,saturate(i.uv.y));
            fixed3 gradientsky=lerp(gradientday,gradientnight,saturate(_WorldSpaceLightPos0.y));
            return gradientsky;
            }
            fixed4 frag (v2f i) : SV_Target
            {
               float moon = distance(i.uv.xyz,-_WorldSpaceLightPos0.xyz);
                float moondis = saturate((1 - moon / _MoonSize)*50);
                 float crescent = distance(float3(i.uv.x+_CrescentOffset,i.uv.y,i.uv.z), _WorldSpaceLightPos0.xyz);
                 float crescentdis=saturate((1 - crescent / _CreScentSize)*50);
                fixed4 endmoon=(moon-crescent)*_MoonColor;
                float sun=distance(i.uv.xyz, -_WorldSpaceLightPos0.xyz);
                  float sundis = saturate((1 - sun / _SunSize)*50);
                  //sundis*_SunColor+
                  float4 col=moondis*_MoonColor+sundis;
                  float4 daycolor=float4(skycolor(i),1);
                return col+daycolor;
            }
            ENDCG
        }
    }
}