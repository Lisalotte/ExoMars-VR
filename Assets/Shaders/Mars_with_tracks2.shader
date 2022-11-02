// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Mars with Tracks 2"
{
    Properties
    {        
        _Tess ("Tessellation", Range(1,128)) = 4
        _Splat ("SplatMap", 2D) = "black" {}
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _TrackColor ("Tracks Color", Color) = (1,1,1,1)
        _TrackTex ("Tracks (RGB)", 2D) = "white" {}

        _PaintColor ("Paint Color", Color) = (1,1,1,1)
        _Paint ("PainMap", 2D) = "black" {}

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Gamma ("Gamma", Range(0,1)) = 0.5
		_BumpMap ("NormalMap", 2D) = "bump" {}
        _NormalMapIntensity ("NormalMapIntensity", Float ) = 1
        _SecondBumpMap ("Second NormalMap", 2D) = "bump" {}
		_Shininess("Shininess", Range(1, 20)) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow fullforwardshadows vertex:disp tessellate:tessDistance

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 4.6
        #include "UnityCG.cginc"
        #include "Tessellation.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };

        float _Tess;
        
        float4 tessDistance (appdata v0, appdata v1, appdata v2) {
            float minDist = 10.0;
            float maxDist = 25.0;
            return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
        }

        sampler2D _Splat;
        sampler2D _Paint;
        float _Displacement;

        void disp (inout appdata v)
        {
            float d = tex2Dlod(_Splat, float4(v.texcoord.xy,0,0)).r * _Displacement;
            v.vertex.xyz -= v.normal * d;
            v.vertex.xyz += v.normal * _Displacement; // displacement registered as actual terrain height
        }
        
        sampler2D _MainTex;
        sampler2D _TrackTex;
        sampler2D _BumpMap;
        sampler2D _SecondBumpMap;
        sampler2D _SpecularMap;
        float4 _SpecularMap_ST;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_TrackTex;
            float2 uv_Paint;
            float2 uv_Splat;
            float2 uv_BumpMap;
            float2 uv_SecondBumpMap;
            float2 uv_SpecularMap;
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        half _Shininess;
        half _Gamma;
        fixed4 _Color;
        fixed4 _TrackColor;
        fixed4 _PaintColor;
        float _NormalMapIntensity;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float random (float2 uv)
        {
            return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
            o.Normal += UnpackNormal(tex2D(_SecondBumpMap, IN.uv_SecondBumpMap));
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            // Albedo comes from a texture tinted by color
            //fixed3 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            half amount = tex2Dlod(_Splat, float4(IN.uv_Splat,0,0)).r;
            fixed3 c = lerp(tex2D (_MainTex, IN.uv_MainTex) * _Color, tex2D (_TrackTex, IN.uv_TrackTex) * _TrackColor, amount);
            c += tex2D(_Paint, IN.uv_Paint) * _PaintColor;

            float3 normalDirection = normalize( mul ((float3x3)UNITY_MATRIX_IT_MV, o.Normal));
            float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            //float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
            //_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v).xyz;

            float3 spec = _SpecColor.rgb * 
                pow(
                    max(0, 
                        dot(
                            reflect(-lightDirection, normalDirection), IN.viewDir
                        )
                    ), _Shininess
                ); 

            c += spec * (tex2D (_SpecularMap, IN.uv_MainTex) * _Color,_Gamma);
            //c *= tex2D(_BumpMap, IN.uv_BumpMap);
            c *= (o.Normal.r+o.Normal.b+o.Normal.g);

            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables

            o.Alpha = _Gamma;
        }
        ENDCG
    }
    FallBack "Diffuse"
}