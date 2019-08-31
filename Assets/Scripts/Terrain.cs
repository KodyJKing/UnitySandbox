using UnityEngine;
using System.Collections;
public class Terrain : MonoBehaviour {

    [Range( 10, 200 )]
    public int resolution = 100;

    [Range( 1, 100 )]
    public float width = 10;

    [Range( 0, 2 )]
    public float seaLevel = 0.5f;

    public OctaveGenerator noise;

    private void OnValidate() {
        generateMesh();
    }

    int coordToIndex( int x, int z ) {
        return x + z * resolution;
    }

    void generateMesh() {
        var center = transform.position;

        var vertexCount = resolution * resolution;

        var vertices = new Vector3[ vertexCount ];
        var triangles = new int[ ( resolution - 1 ) * ( resolution - 1 ) * 6 ];

        int i = 0;

        for ( int x = 0; x < resolution; x++ ) {
            for ( int z = 0; z < resolution; z++ ) {
                Vector3 pos = ( new Vector3( x, 0, z ) / resolution ) - new Vector3( 0.5f, 0, 0.5f );

                float height = noise.generate( new Vector2( pos.x, pos.z ) );
                height = Mathf.Max( 0, height - seaLevel ) + seaLevel;

                Vector3 offset = pos * width;
                vertices[ coordToIndex( x, z ) ] = center + offset + Vector3.up * height;

                if ( x + 1 == resolution || z + 1 == resolution )
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
        mesh.uv = new Vector2[ vertexCount ];
        mesh.RecalculateNormals();

        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;

        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;
    }
}
