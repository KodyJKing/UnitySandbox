using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour {

    [Range( 10, 200 )]
    public int width = 100;
    [Range( 1, 100 )]
    public float hScale = 10;
    [Range( 1, 100 )]
    public float vScale = 1;
    [Range( 1, 100 )]
    public float roughness = 1;
    public Vector2 noiseOffset = new Vector2( 1000, 2000 );

    private void OnValidate() {
        generateMesh( width );
    }

    int coordToIndex( int x, int z ) {
        return x + z * width;
    }

    void generateMesh( int width ) {
        var center = transform.position;

        var vertexCount = width * width;

        var vertices = new Vector3[ vertexCount ];
        var triangles = new int[ ( width - 1 ) * ( width - 1 ) * 6 ];
        var uvs = new Vector2[ vertexCount ];

        int i = 0;

        for ( int x = 0; x < width; x++ ) {
            for ( int z = 0; z < width; z++ ) {
                Vector3 pos = ( new Vector3( x, 0, z ) / width ) - new Vector3( 0.5f, 0, 0.5f );
                Vector2 noisePos = noiseOffset + ( new Vector2( pos.x, pos.z ) * roughness );
                float height = Mathf.PerlinNoise( noisePos.x, noisePos.y ) * vScale;

                Vector3 offset = pos * hScale;
                vertices[ coordToIndex( x, z ) ] = center + offset + Vector3.up * height;

                if ( x + 1 == width || z + 1 == width )
                    continue;

                triangles[ i++ ] = coordToIndex( x + 1, z + 1 );
                triangles[ i++ ] = coordToIndex( x + 1, z );
                triangles[ i++ ] = coordToIndex( x, z );

                triangles[ i++ ] = coordToIndex( x, z );
                triangles[ i++ ] = coordToIndex( x, z + 1 );
                triangles[ i++ ] = coordToIndex( x + 1, z + 1 );
            }
        }

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;

        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;
    }
}
