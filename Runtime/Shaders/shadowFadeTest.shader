Shader "Custom/shadowFadeTest"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _ShadowFade ("Shadow Fade", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = _Color;
            // o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Color.a;
        }
        ENDCG


//        Pass {
//            Name "ShadowCaster"
//            Tags { "LightMode" = "ShadowCaster" }
//
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #include "UnityCG.cginc"
//
//            float _ShadowFade; // 0 = fully faded, 1 = full shadow
//
//            struct appdata {
//                float4 vertex : POSITION;
//            };
//
//            struct v2f {
//                float4 pos : SV_POSITION;
//            };
//
//            v2f vert(appdata v) {
//                v2f o;
//                o.pos = UnityObjectToClipPos(v.vertex);
//                return o;
//            }
//
//            fixed4 frag(v2f i) : SV_Target {
//                return fixed4(0, 0, 0, _ShadowFade); // Control opacity of shadow
//            }
//            ENDCG
//        }

    }
    FallBack "Diffuse"
}
