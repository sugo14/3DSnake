Shader "Custom/GlowOutline"
{
    Properties
    {
        _Color ("Glow Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Pass
        {
            Cull Front  // Render only back faces to create the outline
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                return _Color; // Solid glow color
            }
            ENDCG
        }
    }
}
