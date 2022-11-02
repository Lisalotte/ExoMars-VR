Shader "Skybox/Skybox Mars"
{
    Properties
    {
        _SkyTint("Sky Tint", Color) = (0.5, 0.5, 0.5, 1)
        _HorizonFogExponent("Horizon Fog", Range(0,15)) = 1

        [Header(Sun)]
        _SunSize("    Size", Range(0,1)) = 0.04
        _SunColor("    Color", Color) = (1,1,1,1)
        _SunHardness("    Hardness", float) = 0.1
        _SunGlareStrength("    Glare Strength", Range(0,1)) = 0.5

        //[Header(Moon)]
        //_MoonColor("    Color", Color) = (1, 1, 1, 1)
        //_MoonPosition("    Position", Vector) = (0, 0, 1)
        //_MoonSize("    Size", Range(0,1)) = 0.04
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #define HARDNESS_EXPONENT_BASE 0.125

            struct appdata
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float3 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            uniform half4 _MoonPosition;
            uniform half3 _SkyTint, _MoonColor, _SunColor;
            uniform half _SunSize, _HorizonFogExponent, _SunHardness, _SunGlareStrength;

            half calculateSpot(half3 sunDirPos, half3 skyDirPos, half size, out half dist) {
                half3 delta = sunDirPos - skyDirPos;
                dist = float(length(delta));
                half spot = 1.0 - smoothstep(0.0, _SunSize, dist);
                return 1.0 - pow(HARDNESS_EXPONENT_BASE, spot * _SunHardness);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half p = i.texcoord.y;

                half sunDist;
                half3 sunMie;
                sunMie = calculateSpot(_WorldSpaceLightPos0.xyz, i.texcoord.xyz, _SunSize, sunDist);
                //half3 moonMie = calculateSpot(_MoonPosition.xyz, i.texcoord.xyz, _MoonSize, sunDist);

                float p1 = pow(min(1.0, 1.0 - p), _HorizonFogExponent);
                float p2 = 1.0 - p1;

                sunDist = 1 - sunDist;

                half glareMultiplier = saturate((sunDist - _SunGlareStrength) / (1 - _SunGlareStrength));

                half3 col = 0;
                col += sunMie * p2 * _LightColor0.rgb; //Sun
                half4 fogColor = unity_FogColor - (0.1, 0.0, 0.0, 0.2);
                col += lerp(_SkyTint, _SunColor, glareMultiplier * glareMultiplier) * p2 * (1 - sunMie); //sun glare
                //col += moonMie * p2 * _MoonColor;
                col += unity_FogColor * p1; // Horizon fog
                //col += fogColor * p1; // Horizon fog
                return half4(col, 1);
            }
            ENDCG
        }
    }
}
