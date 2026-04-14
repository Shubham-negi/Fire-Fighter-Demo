Shader "Unlit/ArrowLineShader_Glow"
{
    Properties
    {
        _Color ("Arrow Color", Color) = (0,1,0,1)
        _GlowColor ("Glow Color", Color) = (0,1,1,1)
        _Tiling ("Tiling", Float) = 5
        _Thickness ("Thickness", Range(0.05, 1)) = 0.3
        _HeadSize ("Head Size", Range(0.1, 0.9)) = 0.35
        _Gap ("Gap Between Arrows", Range(0.1, 0.9)) = 0.3
        _GlowStrength ("Glow Strength", Range(0, 5)) = 2
        _Offset ("Offset", Float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };

            fixed4 _Color;
            fixed4 _GlowColor;
            float _Tiling;
            float _Thickness;
            float _HeadSize;
            float _Gap;
            float _GlowStrength;
            float _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float u = frac(i.uv.x * _Tiling + _Offset);
                float v = i.uv.y;

                float centerV = abs(v - 0.5);

                // Gap mask – blank space between arrows
                float gapMask = step(_Gap, u);

                // Arrow body
                float body = step(centerV, _Thickness * 0.5);

                // Head
                float headStart = 1 - _HeadSize;
                float inHead = step(headStart, u);

                float headU = saturate((u - headStart) / _HeadSize);
                float maxV = (1 - headU) * 0.5;
                float head = step(centerV, maxV);

                float arrowMask = max(body * (1 - inHead), head * inHead);

                // Combine arrow with gap
                arrowMask *= gapMask;

                // Fresnel glow
                float fresnel = pow(1 - saturate(dot(normalize(i.worldNormal), normalize(i.viewDir))), 2);

                // Glow effect
                float glow = fresnel * _GlowStrength;

                // Final color
                fixed3 col = lerp(_Color.rgb, _GlowColor.rgb, glow);

                float alpha = saturate(arrowMask + glow * 0.5);

                return fixed4(col, alpha);
            }
            ENDCG
        }
    }
}
