Shader "Unlit/skybox"
 {
	Properties {
		_TopColor ("Top Color", Color) = (1, 0.3, 0.3, 0)
		_MiddleColor ("MiddleColor", Color) = (1.0, 1.0, 0.8)
		_BottomColor ("Bottom Color", Color) = (0.3, 0.3, 1, 0)
		_Left ("Left", Vector) = (1, 0, 0)
		_Exp ("Exp", Range(0, 16)) = 1
	}
	SubShader {
		Tags {
			"RenderType" = "Background"
			"Queue" = "Background"
			"PreviewType" = "Skybox"
		}
		Pass {
			ZWrite Off
			Cull Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			fixed3 _TopColor, _BottomColor, _MiddleColor;
			float3 _Left;
			float _Exp;

			struct appdata {
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 texcoord : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag (v2f i) : SV_TARGET {
				float3 texcoord = normalize(i.texcoord);
				float3 left = normalize(_Left);
				float d = dot(texcoord, left);
				float s = sign(d);
				return fixed4(lerp(_MiddleColor, s < 0.0 ? _BottomColor : _TopColor, pow(abs(d), _Exp)), 1);
			}

			ENDCG
		}
	}
	CustomEditor "GradientSkybox.LinearThreeColorGradientSkyboxGUI"
}