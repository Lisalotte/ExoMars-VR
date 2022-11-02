// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Mars 2"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        _Glossiness ("Smoothness", Range(0,1)) = 0.5

        _Metallic ("Metallic", Range(0,1)) = 0.0

        _Gamma ("Gamma", Range(0,1)) = 0.5

		//_SpecularMap ("SpecularMap", 2D) = "black" {}
        //_SpecularMapIntensity ("SpecularMapIntensity", Range(0, 1)) = 0.4

		_BumpMap ("NormalMap", 2D) = "bump" {}

        _NormalMapIntensity ("NormalMapIntensity", Float ) = 1

        _SecondBumpMap ("Second NormalMap", 2D) = "bump" {}
        //_NormalMapIntensity ("NormalMapIntensity", Float ) = 1

		_SpecMap("Specular Map", 2D) = "white" {}
        _SpecColor("Specular Color", Color) = (1,1,1,1)

		_Shininess("Shininess", Range(1, 20)) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        //float4 _MainTex_ST;
        sampler2D _BumpMap;
        //float4 _BumpMap_ST;
        sampler2D _SecondBumpMap;
        sampler2D _SpecularMap;
        float4 _SpecularMap_ST;

        struct Input
        {
            float2 uv_MainTex;
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
        float _NormalMapIntensity;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        // Editing vertices
        void vert(inout appdata_full v) {
            //v.vertex.xyz += v.normal.xyz * _Color * sin(_Time.y);
        }

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
            fixed3 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

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
            
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables

            o.Alpha = _Gamma;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
