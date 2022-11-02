Shader "Skybox/Skybox Mars Night"
{
    Properties
    {
        _SkyTint("Sky Tint", Color) = (0.5, 0.5, 0.5, 0)
        _HorizonFogExponent("Horizon Fog", Range(0,15)) = 1

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

            samplerCUBE _Tex;
            //half4 _Tex_HDR;
            uniform float _starVis;
            half4 _Tint;
            half _Exposure, _Saturation;
            uniform half3 _SkyTint;
            uniform half _HorizonFogExponent;
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

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float3 texcoord : TEXCOORD0; 
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(RotateInDegrees(v.vertex, 10.0, _Time[1], 0.0)); //mul(v.vertex, _Rotation));
                o.texcoord = v.vertex.xyz;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 nightTex = texCUBE(_Tex, i.texcoord); // night sky texture
                //half3 col = DecodeHDR(nightTex, _Tex_HDR);
                half p = i.texcoord.y;
                float p1 = pow(min(1.0, 1.0 - p), _HorizonFogExponent);
                float p2 = 1.0 - p1;

                half3 col = nightTex * _starVis;
                col += _SkyTint;
                //col += unity_FogColor * p1; // Horizon fog
                col = col * _Exposure;
                return half4(col,1);
            }
            ENDCG
        }
    }
}
