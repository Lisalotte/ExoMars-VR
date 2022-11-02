Shader "Custom/DustStorm"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
        _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0

        _ForceFieldRadius("Force Field Radius", Float) = 10.0
        _ForceFieldPosition("Force Field Position", Vector) = (0.0, 0.0, 0.0, 0.0)

        _ColourA("Color A", Color) = (0.0, 0.0, 0.0, 0.0)
        _ColourB("Color B", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        LOD 100
        Blend One OneMinusSrcColor
        Zwrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_particles

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
            };

            struct v2f
            {
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _ForceFieldRadius;
            float3 _ForceFieldPosition;

            float4 _ColourA;
            float4 _ColourB;

            float4 GetParticleOffset(float3 particleCenter) 
            {
                float distanceToParticle = distance(particleCenter, _ForceFieldPosition);
                float forceFieldRadiusAbs = abs(_ForceFieldRadius);
                
                float3 directionToParticle = normalize(particleCenter - _ForceFieldPosition);
                //float3 directionToParticle = normalize(float3(0.0,1.0,0.0));

                float distanceToForceFieldRadius = max(forceFieldRadiusAbs - distanceToParticle, 0.0);
                distanceToForceFieldRadius *= sign(_ForceFieldRadius); // allow inverted distances

                float4 particleOffset;
                particleOffset.xyz = directionToParticle * distanceToForceFieldRadius;
                particleOffset.w = distanceToForceFieldRadius / (_ForceFieldRadius + 0.0001); // prevent divide by zero

                return particleOffset;     
            }

            v2f vert (appdata v)
            {
                v2f o;

                float3 particleCenter = float3(v.texcoord0.zw, v.texcoord0.x);
                float3 vertexOffset = GetParticleOffset(particleCenter);
                

                v.vertex.xyz += vertexOffset;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.color = v.color;
                o.texcoord0.xy = TRANSFORM_TEX(v.texcoord0,_MainTex);

                o.texcoord0.zw = v.texcoord0.zw;
                o.texcoord1 = v.texcoord1;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float _InvFade;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 col = saturate(i.color * tex2D(_MainTex, i.texcoord0));
                float3 particleCenter = float3(i.texcoord0.zw, i.texcoord1.x);
                float particleOffsetNormalizedLength = GetParticleOffset(particleCenter).w;
                _ForceFieldRadius = particleCenter.x;

                col = lerp(col * _ColourA, col * _ColourB, particleOffsetNormalizedLength);
                
                //fixed4 col = tex2D(_MainTex, i.texcoord);
                col.rgb *= col.a;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }    
    Fallback Off
}
