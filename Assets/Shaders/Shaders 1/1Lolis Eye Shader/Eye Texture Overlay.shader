// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader ".Loli_Explorer.exe/Eye Shader"
{
	Properties
	{
		_AmplifyMaintex("Amplify Main tex", Float) = 2
		_Desaturation("Desaturation", Float) = 0
		_Main("Main", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1,1,1,0)
		_GlowPower("Glow Power", Float) = 3
		_NoiseRamp("Noise Ramp", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_RampSpeed("Ramp Speed", Vector) = (0,0.1,0,0)
		_NoiseSpeed("Noise Speed", Vector) = (0,0.1,0,0)
		_NoisePower("Noise Power", Float) = 3
		_NoiseScale("Noise Scale", Float) = 1
		_RampColor("Ramp Color", Color) = (1,1,1,0)
		_PanningTex("Panning Tex", 2D) = "white" {}
		_PanningTexSpeed("Panning Tex Speed", Vector) = (0,0,0,0)
		_PanningTexPower("Panning Tex Power", Float) = 1
		_PanningTexOpacity("Panning Tex Opacity", Range( 0 , 1)) = 0.5
		_DistortionTex("Distortion Tex", 2D) = "bump" {}
		_Ammount("Ammount", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _Main;
		uniform float4 _Main_ST;
		uniform float _Desaturation;
		uniform float4 _MainColor;
		uniform float _GlowPower;
		uniform float _AmplifyMaintex;
		uniform sampler2D _Noise;
		uniform float2 _NoiseSpeed;
		uniform float _NoiseScale;
		uniform float _NoisePower;
		uniform sampler2D _NoiseRamp;
		uniform float2 _RampSpeed;
		uniform float4 _RampColor;
		uniform float _PanningTexOpacity;
		uniform sampler2D _PanningTex;
		uniform float2 _PanningTexSpeed;
		uniform sampler2D _DistortionTex;
		uniform float4 _DistortionTex_ST;
		uniform float _Ammount;
		uniform float _PanningTexPower;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Main = i.uv_texcoord * _Main_ST.xy + _Main_ST.zw;
			float3 desaturateInitialColor58 = tex2D( _Main, uv_Main ).rgb;
			float desaturateDot58 = dot( desaturateInitialColor58, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar58 = lerp( desaturateInitialColor58, desaturateDot58.xxx, _Desaturation );
			float4 temp_cast_2 = (_AmplifyMaintex).xxxx;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNDotV36 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode36 = ( 0.0 + 1.04 * pow( 1.0 - fresnelNDotV36, _NoiseScale ) );
			float2 temp_cast_3 = (fresnelNode36).xx;
			float2 panner34 = ( _Time.y * _NoiseSpeed + temp_cast_3);
			float fresnelNDotV20 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode20 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNDotV20, 1.0 ) );
			float2 temp_cast_4 = (fresnelNode20).xx;
			float2 panner8 = ( _Time.y * _RampSpeed + temp_cast_4);
			float2 panner60 = ( _Time.y * _PanningTexSpeed + float2( 0,0 ));
			float2 uv_TexCoord62 = i.uv_texcoord + panner60;
			float2 uv_DistortionTex = i.uv_texcoord * _DistortionTex_ST.xy + _DistortionTex_ST.zw;
			o.Emission = ( pow( ( float4( desaturateVar58 , 0.0 ) * _MainColor * _GlowPower ) , temp_cast_2 ) * ( ( tex2D( _Noise, panner34 ) * _NoisePower ) * tex2D( _NoiseRamp, panner8 ) * _RampColor ) * ( saturate( ( ( 1.0 - _PanningTexOpacity ) + tex2D( _PanningTex, ( float3( uv_TexCoord62 ,  0.0 ) + ( (UnpackNormal( tex2D( _DistortionTex, uv_DistortionTex ) )).xyz * _Ammount ) ).xy ).r ) ) * _PanningTexPower ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
-1673;55;1666;953;1202.949;1293.084;1.292649;True;True
Node;AmplifyShaderEditor.SamplerNode;70;-1629.838,-1075.667;Float;True;Property;_DistortionTex;Distortion Tex;16;0;Create;True;0;0;False;0;None;100221baada53b74489b25ac8b7e0120;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;64;-1248.434,-1177.381;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;63;-1293.433,-1360.382;Float;False;Property;_PanningTexSpeed;Panning Tex Speed;13;0;Create;True;0;0;False;0;0,0;0,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;60;-1000.338,-1442.382;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1312.081,-790.812;Float;False;Property;_Ammount;Ammount;17;0;Create;True;0;0;False;0;0;0.127;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;71;-1280.881,-974.1106;Float;False;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-842.7809,-896.1118;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;62;-657.8329,-1347.981;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;-2018.298,-565.6118;Float;False;Property;_NoiseScale;Noise Scale;10;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-528.1804,-903.9118;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FresnelNode;36;-1783.89,-550.5869;Float;True;Tangent;4;0;FLOAT3;0,0,1;False;1;FLOAT;0;False;2;FLOAT;1.04;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;32;-1844.672,-279.8709;Float;False;Property;_NoiseSpeed;Noise Speed;8;0;Create;True;0;0;False;0;0,0.1;0,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TimeNode;33;-1859.672,-139.8712;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;67;-21.07124,-791.7353;Float;True;Property;_PanningTexOpacity;Panning Tex Opacity;15;0;Create;True;0;0;False;0;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-946.4661,-477.7568;Float;True;Property;_Main;Main;2;0;Create;True;0;0;False;0;None;a32658702d9cdaa4fb4ffd7fd8de8360;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;20;-2029.767,99.05414;Float;True;Tangent;4;0;FLOAT3;0,0,1;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;61;-53.0332,-490.4811;Float;True;Property;_PanningTex;Panning Tex;12;0;Create;True;0;0;False;0;None;131609ca3b52bd047b472c35f10a30a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;69;262.4288,-647.8353;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-592.816,-445.368;Float;False;Property;_Desaturation;Desaturation;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;34;-1459.672,-293.3708;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;10;-1750,288.5;Float;False;Property;_RampSpeed;Ramp Speed;7;0;Create;True;0;0;False;0;0,0.1;0,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DesaturateOpNode;58;-343.8456,-556.3587;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;8;-1365,273.0001;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;344.4288,-469.8353;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-900.4656,-243.7569;Float;False;Property;_MainColor;Main Color;3;0;Create;True;0;0;False;0;1,1,1,0;0,1,0.5034484,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-848.4657,-31.75692;Float;False;Property;_GlowPower;Glow Power;4;0;Create;True;0;0;False;0;3;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1003.97,370.0175;Float;False;Property;_NoisePower;Noise Power;9;0;Create;True;0;0;False;0;3;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1108.6,113.9;Float;True;Property;_Noise;Noise;6;0;Create;True;0;0;False;0;bca178b4ea45a034ab11a1334ae00023;3d3f7302c1edc9044852d2424e603197;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-468.8971,74.73041;Float;False;Property;_AmplifyMaintex;Amplify Main tex;0;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-905.5199,605.472;Float;True;Property;_NoiseRamp;Noise Ramp;5;0;Create;True;0;0;False;0;None;b0e742580e544844ea4612602a9ea125;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;75;589.9471,-388.0794;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-777.5708,259.8173;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1.3069,-223.464;Float;False;Property;_PanningTexPower;Panning Tex Power;14;0;Create;True;0;0;False;0;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-550.4658,-273.7569;Float;True;3;3;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;44;-742.9235,893.4353;Float;False;Property;_RampColor;Ramp Color;11;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;56;-234.8976,-31.86897;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-287.7773,670.4022;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;769.8931,-310.664;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;260.9,133.1;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;742,35;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;.Loli_Explorer.exe/Eye Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;60;2;63;0
WireConnection;60;1;64;2
WireConnection;71;0;70;0
WireConnection;72;0;71;0
WireConnection;72;1;73;0
WireConnection;62;1;60;0
WireConnection;74;0;62;0
WireConnection;74;1;72;0
WireConnection;36;3;40;0
WireConnection;61;1;74;0
WireConnection;69;0;67;0
WireConnection;34;0;36;0
WireConnection;34;2;32;0
WireConnection;34;1;33;2
WireConnection;58;0;1;0
WireConnection;58;1;59;0
WireConnection;8;0;20;0
WireConnection;8;2;10;0
WireConnection;8;1;33;2
WireConnection;68;0;69;0
WireConnection;68;1;61;1
WireConnection;5;1;34;0
WireConnection;19;1;8;0
WireConnection;75;0;68;0
WireConnection;18;0;5;0
WireConnection;18;1;17;0
WireConnection;4;0;58;0
WireConnection;4;1;2;0
WireConnection;4;2;3;0
WireConnection;56;0;4;0
WireConnection;56;1;57;0
WireConnection;30;0;18;0
WireConnection;30;1;19;0
WireConnection;30;2;44;0
WireConnection;66;0;75;0
WireConnection;66;1;65;0
WireConnection;7;0;56;0
WireConnection;7;1;30;0
WireConnection;7;2;66;0
WireConnection;0;2;7;0
ASEEND*/
//CHKSM=600DD5DCC9631ADAA3F30311087D26FD5ED4FC69