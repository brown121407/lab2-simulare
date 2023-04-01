using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MultiSquare : MonoBehaviour
{
    [SerializeField]
    private int size = 10;

    [SerializeField] 
    private float sideLength = 1.0f;
    
    private void OnEnable()
    {
        var mesh = new Mesh
        {
            name = "Procedural Mesh"
        };

        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        for (var row = 0; row <= size; row++)
        {
            for (var column = 0; column <= size; column++)
            {
                vertices.Add(new Vector3(column * sideLength, row * sideLength, 0));
            }
        }

        for (var row = 0; row < size; row++)
        {
            for (var squareId = 0; squareId < size; squareId++)
            {
                // bottom-left triangle
                triangles.Add(row * (size + 1) + squareId);
                triangles.Add((row + 1) * (size + 1) + squareId);
                triangles.Add(row * (size + 1) + squareId + 1);
                // top-right triangle
                triangles.Add(row * (size + 1) + squareId + 1);
                triangles.Add((row + 1) * (size + 1) + squareId);
                triangles.Add((row + 1) * (size + 1) + squareId + 1);
            }
        }
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
