Shader "Unlit/water"
{
    Properties
    {   
        _DepthGradientShallow("浅滩水颜色",Color) = (0.325,0.807,0.971,0.725)
        _DepthGradientDeep("深水区颜色",Color) = (0.806,0.407,1,0.749)
        _DepthMaxDistance("最大水深",Float) = 1

        [Header(Noise)]
        _SurfaceNoise("水面波纹",2D) = "white"{}
        _SurfaceNoiseCutOff("波纹数量阈值",Range(0,1)) = 0.777
        _FoamMaxDistance("最大波纹偏移",Float) = 0.4
        _FoamMinDistance("最小波纹偏移",Float) = 0.04
        _SurfaceNoiseScroll("波纹流动",Vector) = (0.03,0.03,0,0)
        _SurfaceDistortion("波纹干扰图",2D) = "White" {}
        _SurfaceDistortionAmount("波纹干扰数量",Range(0,1)) = 0.27 

        _FoamColor("波纹颜色",Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags{"Queue" = "Transparent"}//add transparent

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define SMOOTHSTEP_AA 0.01  // AA

            fixed4 _DepthGradientDeep;
            fixed4 _DepthGradientShallow;
            float _DepthMaxDistance;

            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;
            float _SurfaceNoiseCutOff;
            float _FoamMaxDistance;
            float _FoamMinDistance;
            float2 _SurfaceNoiseScroll;
            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;
            float _SurfaceDistortionAmount;
            fixed4 _FoamColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 noiseUV :TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float2 distortionUV : TEXCOORD3;
                float3 viewNormal : NORMAL;

            };

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.noiseUV = TRANSFORM_TEX(v.uv,_SurfaceNoise);
                o.distortionUV = TRANSFORM_TEX(v.uv,_SurfaceDistortion);
                o.viewNormal = COMPUTE_VIEW_NORMAL;
                return o;
            }

            //customlized Blend
            float4 alphaBlend(float4 top, float4 bottom){
                float3 Color = top.rgb * top.a + bottom.rgb * (1 - top.a);
                float Alpha = top.a + bottom.a *(1 - top.a);
                return float4(Color,Alpha);
            }

            float4 frag (v2f i) : SV_Target
            {
                //Depth
                float ULDepth = tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r;
                float LDepth = LinearEyeDepth(ULDepth);
                float depthGap = LDepth - i.screenPos.w;

                //Color
                float depthGap01 = saturate(depthGap/_DepthMaxDistance);
                fixed4 waterColor = lerp(_DepthGradientShallow,_DepthGradientDeep,depthGap01);

                //Noise
                float2 distortion = (tex2D(_SurfaceDistortion,i.distortionUV).xy * 2 - 1) * _SurfaceDistortionAmount;//add distortion
                float2 noiseUV = float2(i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x + distortion.x,i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y + distortion.y);
                float SurfaceNoise = tex2D(_SurfaceNoise,noiseUV).r;


                float3 normal = tex2Dproj(_CameraNormalsTexture,UNITY_PROJ_COORD(i.screenPos));
                float3 normalDot = saturate(dot(normal,i.viewNormal));
                float foamDistance = lerp(_FoamMaxDistance,_FoamMinDistance,normalDot);
                float depthFoam01 = saturate(depthGap/foamDistance); 
                float SurfaceNoiseCutOff = _SurfaceNoiseCutOff * depthFoam01;
                //AA
                float SurfaceWaveAlpha = smoothstep(SurfaceNoiseCutOff - SMOOTHSTEP_AA,SurfaceNoiseCutOff + SMOOTHSTEP_AA,SurfaceNoise);
                float4 SurfaceWaveColor = _FoamColor;
                SurfaceWaveColor.a *= SurfaceWaveAlpha;

                //return depthGap;
                return alphaBlend(SurfaceWaveColor,waterColor);
            }
            ENDCG
        }
    }
}