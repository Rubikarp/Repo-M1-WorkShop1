Shader "Unlit/HandUnlitShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _CodeVariable("TextVariable", Range(0,1)) = 0.5
        //Type (ici float dans un slider*/) = 0.5 /*default variable
        _FresneColor("FresneColor", Color) = (.25, .5, .5, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        //ZWrite On/Off écriture dans le Z buffer
        //ZTest LEqual/GEqual/Always comment lire dans le Z buffer

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CodeVariable;
            float4 _FresneColor;

            v2f vert (appdata v)
            {
                v2f o;

                v.vertex.xyz += v.normals * _CodeVariable * sin(_Time.y);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                float3 view = normalize(WorldSpaceViewDir(v.vertex));
                o.color = (1-abs(dot(view, v.normals))) * _FresneColor;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
