Shader "My shaders/Simple Mars" {
	Properties {
        _Color ("Main Color", Color) = (1,1,1,0.5)
        _MainTex ("Texture", 2D) = "white" { }
	}
	SubShader {		
        Pass {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex; 
            float4 _MainTex_ST;
            fixed4 _Color;

            // vertex shader inputs
            struct VertexInput {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // vertex shader outputs
            struct VertexOutput {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; //clip space position
            };

            VertexOutput vert (appdata_base v) {
                VertexOutput o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                return o;
            }
 
            /*
            Pixel shader:
            A pixel shader, also known as a fragment shader, is a program that dictates the color, 
            brightness, contrast, and other characteristics of a single pixel
            */
            fixed4 frag(VertexOutput i) : SV_Target {
                fixed4 texcol = tex2D (_MainTex, i.uv);
                return texcol * _Color;
            }
		ENDCG
		}
	}
}
