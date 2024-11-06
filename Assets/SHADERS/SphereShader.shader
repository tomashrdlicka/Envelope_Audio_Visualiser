Shader "Custom/BackgroundSphere"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0, 0, 0, 1)
        _EmissionColor ("Emission Color", Color) = (0.96, 0.87, 0.70, 1)
        _EmissionIntensity ("Emission Intensity", Float) = 1.0
        _DistortionStrength ("Distortion Strength", Float) = 0.1
        _PulseSpeed ("Pulse Speed", Float) = 1.0
        _NoiseScale ("Noise Scale", Float) = 0.5
        _TimeMultiplier ("Time Multiplier", Float) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Cull Front
        ZWrite Off
        Lighting Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float _DistortionStrength;
            float _PulseSpeed;
            float _NoiseScale;
            float _TimeMultiplier;
            float _EmissionIntensity;

            fixed4 _BaseColor;
            fixed4 _EmissionColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            // Function to generate smooth noise
            float smoothNoise(float3 pos)
            {
                return frac(sin(dot(pos, float3(12.9898, 78.233, 45.164))) * 43758.5453) * 2.0 - 1.0;
            }

            float3 distortPosition(float3 position, float time)
            {
                float noiseValue = smoothNoise(position * _NoiseScale + float3(time, time, time) * _TimeMultiplier);
                float distortion = noiseValue * _DistortionStrength;
                return position + distortion * normalize(position);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float time = _Time.y * _PulseSpeed;
                float intensity = abs(sin(time)); // Pulsating effect

                // Calculate distorted position based on noise
                float3 distortedPosition = distortPosition(i.worldPos, time);

                // Calculate the base color with pulse effect
                fixed4 baseColor = lerp(_BaseColor, _BaseColor * 0.5, intensity);

                // Emission color calculation with intensity control
                fixed4 emission = _EmissionColor * intensity * _EmissionIntensity;

                // Final color output combining base color and emission
                fixed4 color = baseColor + emission;
                return color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Color"
}

