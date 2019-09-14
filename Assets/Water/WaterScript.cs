using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{

    public int dimension = 10;
    public GameObject pointLight;

    public GameObject terrain;

    // Start is called before the first frame update
    void Start()
    {
        MeshFilter plane = this.gameObject.AddComponent<MeshFilter>();
        MeshCollider collider = this.gameObject.AddComponent<MeshCollider>();
        plane.mesh = createPlaneMesh();
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected Mesh createPlaneMesh()
    {
        Mesh m = new Mesh();
        m.name = "water";

        m.vertices = generateVert();
        m.colors = generateColor(m.vertices.Length);

        m.triangles = generateTri(m.vertices.Length);

        m.RecalculateNormals();
        return m;
    }

    protected Vector3[] generateVert()
    {
        var verts = new Vector3[(dimension + 1) * (dimension + 1)];

        int dim = (dimension + 1) / 2;

        // equally distributed vertices
        for (int i = 0; i <= dimension; i++)
        {
            for (int j = 0; j <= dimension; j++)
            {
                verts[index(i, j)] = new Vector3(dim - i, 0, dim - j);
            }
        }

        return verts;
    }

    private int index(int i, int j)
    {
        return i * (dimension + 1) + j;
    }

    protected Color[] generateColor(int length)
    {
        var colors = new Color[length];

        for (int i = 0; i < length; i++)
        {
            colors[i] = Color.blue;
        }
        return colors;
    }

    protected int[] generateTri(int length)
    {
        var tries = new int[length * 6];

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                tries[index(i, j) * 6 + 0] = index(i, j);
                tries[index(i, j) * 6 + 1] = index(i + 1, j + 1);
                tries[index(i, j) * 6 + 2] = index(i + 1, j);
                tries[index(i, j) * 6 + 3] = index(i, j);
                tries[index(i, j) * 6 + 4] = index(i, j + 1);
                tries[index(i, j) * 6 + 5] = index(i + 1, j + 1);
            }
        }
        return tries;
    }

}
