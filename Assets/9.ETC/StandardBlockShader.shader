	Shader "Minecraft/Blocks" {

	Properties{
		_MainTex("Block Texture Atlas", 2D) = "white" {}
	}

		SubShader{

			Tags {"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
			LOD 100


			Pass {

				CGPROGRAM
					#pragma vertex vertFunction
					#pragma fragment fragFunction
					#pragma target 2.0


					//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
					//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
					#include "UnityCG.cginc"

					struct appdata {

						float4 vertex : POSITION;
						float2 uv : TEXCOORD0;
						float4 color : COLOR;

					};

					struct v2f {

						float4 vertex : SV_POSITION;
						float2 uv : TEXCOORD0;
						float3 normal : TEXCOORD1;
						float4 color : COLOR;

					};

					sampler2D _MainTex;
					float GlobalLightLevel;
					float minGlobalLightLevel;
					float maxGlobalLightLevel;

					v2f vertFunction(appdata v) {

						v2f o;

						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv = v.uv;
						o.color = v.color;

						return o;

					}

					fixed4 fragFunction(v2f i) : SV_Target {

						fixed4 col = tex2D(_MainTex, i.uv);

						float shade = (maxGlobalLightLevel - minGlobalLightLevel) * GlobalLightLevel + minGlobalLightLevel;
						shade *= i.color.a;
						shade = clamp(1 - shade, minGlobalLightLevel, maxGlobalLightLevel);
						

						
						
						col = lerp(col, float4(0, 0, 0, 1), shade);

						return col;

					}

					ENDCG

			}
			UsePass "Universal Render Pipeline/Lit/ShadowCaster"

	}

}