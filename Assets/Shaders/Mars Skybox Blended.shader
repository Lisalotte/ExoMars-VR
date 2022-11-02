Shader "Skybox/Mars Skybox Blended"
{
    Properties
    {
        _SkyTint("Sky Tint", Color) = (0.5, 0.5, 0.5, 0)
        _HorizonFogExponent("Horizon Fog", Range(0,15)) = 1

        [Header(Sun)]
        _SunSize("    Size", Range(0,1)) = 0.04
        _SunColor("    Color", Color) = (1,1,1,1)
        _SunHardness("    Hardness", float) = 0.1
        _SunGlareStrength("    Glare Strength", Range(0,1)) = 0.5

        [Header(Stars)]
        [NoScaleOffset] _Tex("Cube", Cube) = "grey"{}
        _Tint("    Tint", Color) = (.5, .5, .5)
        _StarSize("    Size", Range(0,1)) = 0.005

        _Euler("Euler", Vector) = (0, 0, 0)
        [HideInInspector] _Rotation1("-", Vector) = (1, 0, 0)
        [HideInInspector] _Rotation2("-", Vector) = (0, 1, 0)
        [HideInInspector] _Rotation3("-", Vector) = (0, 0, 1)

        [Gamma] _Exposure("Exposure", Range(0, 8)) = 1
        _Saturation("Saturation", Range(0, 2)) = 1

        [HideInInspector] _CustomTime("-", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Skybox" "Queue"="Background" }
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #define HARDNESS_EXPONENT_BASE 0.125

            samplerCUBE _Tex;
            //half4 _Tex_HDR;
            uniform float _starVis;
            half4 _Tint;
            half _Exposure, _Saturation;

            uniform half4 _MoonPosition;
            uniform half3 _SkyTint, _MoonColor, _SunColor;
            uniform half _SunSize, _HorizonFogExponent, _SunHardness, _SunGlareStrength;
            int isNight;
            uniform float _CustomTime;

            // Extrinsic rotation with Euler angles a, b, g around axes x, y, z
            // See Wikipedia for info on matrices and stuff
            float4 RotateInDegrees(float4 vertex, float degreesX, float degreesY, float degreesZ) {
                float alpha = degreesX * UNITY_PI / 180.0;
                float beta = degreesY * UNITY_PI / 180.0;
                float gamma = degreesZ * UNITY_PI / 180.0;
                float sina, cosa, sinb, cosb, sing, cosg;
                sincos(alpha, sina, cosa);
                sincos(beta, sinb, cosb);
                sincos(gamma, sing, cosg);
                float3x3 Rz = float3x3( cosg, -sing, 0, 
                                        sing, cosg, 0,
                                        0, 0, 1);
                float3x3 Ry = float3x3( cosb, 0, sinb,
                                        0, 1, 0,
                                        -sinb, 0, cosb);
                float3x3 Rx = float3x3( 1, 0, 0,
                                        0, cosa, -sina,
                                        0, sina, cosa);
                float3x3 R = mul(mul(Rz,Ry),Rx);
                return float4(mul(R, vertex.xyz), vertex.w);
            }

            float3 RotateInDegreesTexcoord(float3 texcoord, float degreesX, float degreesY, float degreesZ) {
                float alpha = degreesX * UNITY_PI / 180.0;
                float beta = degreesY * UNITY_PI / 180.0;
                float gamma = degreesZ * UNITY_PI / 180.0;
                float sina, cosa, sinb, cosb, sing, cosg;
                sincos(alpha, sina, cosa);
                sincos(beta, sinb, cosb);
                sincos(gamma, sing, cosg);
                float3x3 Rz = float3x3( cosg, -sing, 0, 
                                        sing, cosg, 0,
                                        0, 0, 1);
                float3x3 Ry = float3x3( cosb, 0, sinb,
                                        0, 1, 0,
                                        -sinb, 0, cosb);
                float3x3 Rx = float3x3( 1, 0, 0,
                                        0, cosa, -sina,
                                        0, sina, cosa);
                float3x3 R = mul(mul(Rz,Ry),Rx);
                return float3(mul(R, texcoord));
            }

            half calculateSpot(half3 sunDirPos, half3 skyDirPos, half size, out half dist) {
                half3 delta = sunDirPos - skyDirPos;
                dist = float(length(delta));
                half spot = 1.0 - smoothstep(0.0, _SunSize, dist);
                return 1.0 - pow(HARDNESS_EXPONENT_BASE, spot * _SunHardness);
            }

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
                float3 texcoord1 : TEXCOORD1;
            };

            struct v2f
            {
                float3 texcoord : TEXCOORD0; 
                float3 texcoord1 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                //o.vertex = UnityObjectToClipPos(RotateInDegrees(v.vertex, 3.0, _Time[1], 0.0)); //mul(v.vertex, _Rotation));

                //o.texcoord = v.vertex.xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = RotateInDegreesTexcoord(v.vertex.xyz, _CustomTime, 0.0, 65.0); // _Time[1], 0.0, 65.0); //
                o.texcoord1 = v.texcoord1;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 nightTex = texCUBE(_Tex, i.texcoord); // night sky texture
                //half3 col = DecodeHDR(nightTex, _Tex_HDR);

                half3 col = nightTex * _starVis;
                //col += _SkyTint;
                //col += unity_FogColor * p1; // Horizon fog
                col = col * _Exposure;

                half p = i.texcoord1.y;

                half sunDist;
                half3 sunMie;
                sunMie = calculateSpot(_WorldSpaceLightPos0.xyz, i.texcoord1.xyz, _SunSize, sunDist);
                //half3 moonMie = calculateSpot(_MoonPosition.xyz, i.texcoord.xyz, _MoonSize, sunDist);

                float p1 = pow(min(1.0, 1.0 - p), _HorizonFogExponent);
                float p2 = 1.0 - p1;

                sunDist = 1 - sunDist;

                half glareMultiplier = saturate((sunDist - _SunGlareStrength) / (1 - _SunGlareStrength));

                col += sunMie * p2 * _LightColor0.rgb; //Sun
                half4 fogColor = unity_FogColor - (0.1, 0.0, 0.0, 0.2);
                col += lerp(_SkyTint, _SunColor, glareMultiplier * glareMultiplier) * p2 * (1 - sunMie); //sun glare
                //col += moonMie * p2 * _MoonColor;
                col += unity_FogColor * p1; // Horizon fog
                //col += fogColor * p1; // Horizon fog                

                return half4(col,1);
            }
            ENDCG
        }
    }
}
