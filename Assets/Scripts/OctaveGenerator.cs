using UnityEngine;
using System.Collections;

[System.Serializable]
public class OctaveGenerator {
    [Range( 1, 5 )]
    public float amplitude = 2;

    [Range( 1, 100 )]
    public float roughness = 3;

    [Range( 1, 10 )]
    public int octaves = 5;

    [Range( 1, 10 )]
    public float roughnessGrowth = 7;
    [Range( 0.1f, 1f )]
    public float scaleGrowth = 0.15f;

    public Vector2 noiseOffset = new Vector2( 100, 200 );

    public float generate( Vector2 v ) {
        float frequency = roughness;
        float scale = amplitude;
        float value = 0;
        float weight = 1;
        for ( int j = 0; j < octaves; j++ ) {
            Vector2 noisePos = noiseOffset + ( v * frequency );
            float noise = Mathf.PerlinNoise( noisePos.x, noisePos.y );
            noise = ( 1 - Mathf.Abs( noise ) );
            noise = noise * noise * weight;
            value += noise * scale * weight;
            weight = noise;

            frequency *= roughnessGrowth;
            scale *= scaleGrowth;
        }

        return value;
    }
}
