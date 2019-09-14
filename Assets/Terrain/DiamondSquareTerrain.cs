using UnityEngine;
using System.Collections;

// reference: https://www.youtube.com/watch?v=1HV8GbFnCik
public class DiamondSquareTerrain : MonoBehaviour {
    
    /**************** attributes ****************/

    // range for height of a vertice in the map
    public float mapHeightRange;

    [Range(-1.0f, 0.0f)]
    public float minAverageY;
    
    [Range(0.0f, 1.0f)]
    public float maxAverageY;

    // number of divisions in map 
    public int mapDivisions;

    // size of map
    public float mapSize;

    public AnimationCurve terrainAnimationCurve;

    MeshCollider meshCollider;

    public struct Map
    {
        public Vector3[] vertices;
        public Vector2[] uvs;
        public int[] triangles;
    }
    /***************************************************/
    
    // initialization
    void Start() {
        meshCollider = this.gameObject.AddComponent<MeshCollider>();
        CreateTerrain();
    }

    // create the terrain
    void CreateTerrain() {

        // generate heightmap
        Map map = generateMapWithHeights();
        
        // Making sure average height in the map is not too extreme
        while (GetAverageY(ref map.vertices) > maxAverageY || GetAverageY(ref map.vertices) < minAverageY) {

            // generate heightmap
            map = generateMapWithHeights();
            Debug.Log("Regenerating Terrain.. Please be patient");
        }

        // Create array to store new vertices
        Vector3[] curvedVertices = new Vector3[map.vertices.Length];
        for (int i = 0; i < map.vertices.Length; i++) {

            // Adjust y-value of vertices of the map based on animation curve. 
            curvedVertices[i] = new Vector3(map.vertices[i].x,
                                            terrainAnimationCurve.Evaluate(map.vertices[i].y),
                                            map.vertices[i].z);
        }
        map.vertices = curvedVertices;

        // create mash based on the map
        Mesh mesh = CreateMeshBasedOnMap(map);

        // Add the mesh to the mesh collider to allow collision
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    Map generateMapWithHeights() {
        // create initial map
        Map map = createMap(mapDivisions, mapSize);

        // add different heights on the map
        generateMapHeights(ref map.vertices, mapDivisions, mapSize, mapHeightRange);

        return map;
    }

    // return tuple consisting of array of vertices, uvs, and triangles based on number of map divisions and map size
    Map createMap(int mapDivisions, float mapSize) {

        // number of vertices in the map
        int nVertices = (mapDivisions + 1) * (mapDivisions + 1);

        // half size of map
        float mapHalfSize = mapSize * 0.5f;

        // half number of divisions in map
        float divisionSize = mapSize / mapDivisions;

        // array of vertices and uvs in the map
        Vector3[] vertices = new Vector3[nVertices];
        Vector2[] uvs = new Vector2[nVertices];

        // triangles formed by 3 vertices, where 2 of them are created for each division
        int[] triangles = new int [mapDivisions * mapDivisions * 3 * 2];

        int triOffset = 0;
        for (int i = 0; i <= mapDivisions; i++) {

            for (int j = 0; j <= mapDivisions; j++) {

                // initialise the vertices and uvs in the 1D arrays
                vertices[i * (mapDivisions + 1)+ j] = new Vector3(-mapHalfSize + j*divisionSize, 0.0f, mapHalfSize-i*divisionSize);
                uvs[i * (mapDivisions + 1)+ j] = new Vector2((float)i/mapDivisions, (float)j/mapDivisions);

                // create the triangles in the map within certain boundaries
                if (i < mapDivisions && j < mapDivisions) {
                    
                    // locate the vertices for the triangles after every division
                    int topLeft = i * (mapDivisions+1) + j;
                    int bottomLeft = (i+1) * (mapDivisions+1) + j;
                    
                    // initialise the triangles with the vertices
                    triangles[triOffset] = topLeft;
                    triangles[triOffset+1] = topLeft+1;
                    triangles[triOffset+2] = bottomLeft+1;

                    triangles[triOffset+3] = topLeft;
                    triangles[triOffset+4] = bottomLeft+1;
                    triangles[triOffset+5] = bottomLeft;

                    // updates offset to move on to other triangles
                    triOffset += 6;
                 }
            }
        }

        Map map = new Map
        {
            vertices = vertices,
            uvs = uvs,
            triangles = triangles
        };

        return map;
    }

    // assign y values to the vertices on the map
    void generateMapHeights(ref Vector3[] vertices, int mapDivisions, float mapSize, float mapHeightRange) {

        // assign y values for four initial points on the map required for Diamond Square Algorithm
        vertices[0].y = Random.Range(-mapHeightRange, mapHeightRange);
        vertices[mapDivisions].y = Random.Range(-mapHeightRange, mapHeightRange);
        vertices[vertices.Length-1].y = Random.Range(-mapHeightRange, mapHeightRange);
        vertices[(vertices.Length-1) - mapDivisions].y = Random.Range(-mapHeightRange, mapHeightRange);

        // Calculate number of iterations and squares required for Diamond Square Algorithm
        int iterations = (int)Mathf.Log(mapDivisions, 2);
        int numSquares = 1;

        // size of the square on the map is equivalent to the number of map divisions
        int squareSize = mapDivisions;

        // iterations in Diamond Square Algorithm to divide the map into squares
        for (int i = 0; i < iterations; i++) {
            
            // iterates through each row on the map
            int row = 0;
            for (int j = 0; j< numSquares; j++) {
                
                // iterates through each column on the map
                int col = 0;
                for(int k = 0; k < numSquares; k++) {

                    // goes through 
                    DiamondSquare(ref vertices, row, col, squareSize, mapHeightRange);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            mapHeightRange *= 0.5f;
        }
    }
        
    // assigns y values to square formed by vertices located at specific row and col
    void DiamondSquare(ref Vector3[] vertices, int row, int col, int size, float offset) {
        
        // half size of map
        int mapHalfSize = (int)(size*0.5f);

        // locations of the vertices of the square
        int topLeft = row*(mapDivisions+1)+col;
        int top = topLeft + mapHalfSize;
        int topRight = topLeft+size;

        int bottomLeft = (row+size) * (mapDivisions+1) + col;
        int bottom = bottomLeft + mapHalfSize;
        int bottomRight = bottomLeft+size;

        int mid = (int)(row+mapHalfSize) * (mapDivisions+1) + (int)(col+mapHalfSize);
        int left = mid - mapHalfSize;
        int right = mid + mapHalfSize;

        // Square step occurs - y value of vertices at mid point uses average value from y value of 4 corners and then added with random offset
        vertices[mid].y = (vertices[topLeft].y + vertices[topLeft+size].y + vertices[bottomLeft].y + vertices[bottomLeft+size].y) * 0.25f + Random.Range(-offset, offset);

        // Diamond step occurs - y value of vertices at the specific locations uses average value from y value of 3 corners (left, mid, and right) and then added with random offset
        vertices[top].y = (vertices[topLeft].y + vertices[topLeft+size].y + vertices[mid].y)/3 + Random.Range(-offset, offset);
        vertices[left].y = (vertices[topLeft].y + vertices[bottomLeft].y + vertices[mid].y)/3 + Random.Range(-offset, offset);
        vertices[right].y = (vertices[topLeft+size].y + vertices[bottomLeft+size].y + vertices[mid].y)/3 + Random.Range(-offset, offset);
        vertices[bottom].y = (vertices[bottomLeft].y + vertices[bottomLeft+size].y + vertices[mid].y)/3 + Random.Range(-offset, offset);
    }

    // get average y values of vertices of terrain
    private float GetAverageY(ref Vector3[] vertices)
    {
        var totalY = 0.0;
        for (int i = 0; i < vertices.Length; i++)
        {
            totalY += vertices[i].y;
        }

        // the average of vertices
        var averageY = totalY / vertices.Length;

        return (float) averageY;
    }

    // create mesh based on map
    Mesh CreateMeshBasedOnMap(Map map) {

        // create mesh
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // assign vertices, uvs, and triangles to mesh
        mesh.vertices = map.vertices;
        mesh.uv = map.uvs;
        mesh.triangles = map.triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }
}

// -38.48796
// 41.6155
// -36.5209

// 35.025
// 45.85

// -37.06147
// 27.3552
// -38.4673

// 24.675
// 44.725