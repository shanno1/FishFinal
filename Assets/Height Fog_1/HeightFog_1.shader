Shader "Custom/HeightFog_1"
{
	Properties 
	{
_FogColor("FogColor", Color) = (0.9058824,0.4627451,0.04313726,1)
_FogHeight("FogHeight", Float) = 0
_FogHeightFade("FogHeightFade", Float) = 0
_FogDensity("FogDensity", Range(0.01,1) ) = 1

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Blend SrcAlpha OneMinusSrcAlpha
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


float4 _FogColor;
float _FogHeight;
float _FogHeightFade;
float _FogDensity;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float3 worldPos;
float4 screenPos;

			};

			void vert (inout appdata_full v, out Input o) {
float4 Cross0=float4( cross( float4( v.normal.x, v.normal.y, v.normal.z, 1.0 ).xyz, float4( 0,0,1,-1).xyz ), 1.0 );
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
v.tangent = Cross0;


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Splat1=float4( IN.worldPos.x, IN.worldPos.y,IN.worldPos.z,1.0 ).y;
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Splat1;
float4 Multiply3=_FogHeight.xxxx * float4( 0.01,0.01,0.01,0.01 );
float4 Multiply2=Invert0 * Multiply3;
float4 Multiply5=Invert0 * _FogHeightFade.xxxx;
float4 Add0=Multiply2 + Multiply5;
float4 SmoothStep0=smoothstep(float4( 1.0, 1.0, 1.0, 1.0 ),float4( 0.0, 0.0, 0.0, 0.0 ),Add0);
float4 Dot0=dot( IN.screenPos.xyzw, IN.screenPos.xyzw ).xxxx;
float4 Ceil0=ceil(Dot0);
float4 Splat0=Ceil0.y;
float4 Multiply4=_FogDensity.xxxx * float4( 0.01,0.01,0.01,0.01 );
float4 Multiply1=Splat0 * Multiply4;
float4 Multiply0=SmoothStep0 * Multiply1;
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = _FogColor;
o.Alpha = Multiply0;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}