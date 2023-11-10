void ToonShading_float(in float3 normal, in float rampSmoothness, in float rampOffset, in float3 positionCS, in float3 positionWS, in float4 rampTinting, out float3 rampOutput, out float3 direction)
{
    #ifdef SHADERGRAPH_PREVIEW
    rampOutput = float3(0.5, 0.5, 0);
    direction = float3(0.5, 0.5, 0);
    #else
    #if SHADOWS_SCREEN
    half4 shadowCoord = ComputeScreenPos(positionCS);
    #else
    half4 shadowCoord = TransformWorldToShadowCoord(positionWS);
    #endif

    #if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
Light light = GetMainLight(shadowCoord);
#else
Light light = GetMainLight();
#endif

half ndotl = dot(normal, light.direction) * 0.5 + 0.5;

half toonRamp = smoothstep(rampOffset, rampOffset + rampSmoothness, ndotl);

toonRamp *= light.shadowAttenuation;

rampOutput = light.color * (toonRamp + rampTinting);

direction = light.direction;
#endif
}