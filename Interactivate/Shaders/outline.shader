Shader "Nightbyte/Interactivate/Outline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0.44,1,1)
        _OutlineWidth ("Outline Width (px)", Float) = 4            // default thicker
        _PixelFactor ("World units per pixel", Float) = 0.0012     // <- was 0.001
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay+1" }
        Cull Front
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Outline"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float  _OutlineWidth;
            float  _PixelFactor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 smooth : TEXCOORD3;      // populated if you’ve baked smooth normals
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                fixed4 col : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;

                // choose smooth normal if present
                float3 nrm = any(v.smooth) ? normalize(v.smooth) : v.normal;

                // to view space
                float3 viewPos = mul(UNITY_MATRIX_MV, v.vertex).xyz;
                float3 viewNrm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, nrm));

                // push outwards
                float px = abs(viewPos.z) * _PixelFactor;
                viewPos += viewNrm * _OutlineWidth * px;

                o.pos = mul(UNITY_MATRIX_P, float4(viewPos,1));
                o.col = _OutlineColor;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target { return i.col; }
            ENDCG
        }
    }
    Fallback Off
}