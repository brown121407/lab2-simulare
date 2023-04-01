using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube : MonoBehaviour
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

        var vertices = new List<List<Vector3>>();
        var triangles = new List<int>();
        for (var face = 0; face < 6; face++)
        {
            var faceVertices = new List<Vector3>();
            for (var row = 0; row <= size; row++)
            {
                for (var column = 0; column <= size; column++)
                {
                    faceVertices.Add(new Vector3(column * sideLength, row * sideLength, 0));
                }
            }
            vertices.Add(faceVertices);
        }

        var rotations = new Quaternion[]
        {
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, 180, 0),
            Quaternion.Euler(0, 270, 0),
            Quaternion.Euler(90, 0, 0),
            Quaternion.Euler(-90, 0, 0),
        };
        
        var offsets = new Vector3[]
        {
            new Vector3(0, 0, -size * sideLength),
            new Vector3(0, 0, 0),
            new Vector3(size * sideLength, 0, 0),
            new Vector3(size * sideLength, 0, -size * sideLength),
            new Vector3(0, size * sideLength, -size * sideLength),
            new Vector3(0, 0, 0),
        };

        vertices = vertices
            .Select((faceVertices, index) => faceVertices.Select(v => rotations[index] * v + offsets[index]).ToList())
            .ToList();
        
        for (var face = 0; face < 6; face++)
        {
            var faceStart = face * (size + 1) * (size + 1);
            for (var row = 0; row < size; row++)
            {
                for (var squareId = 0; squareId < size; squareId++)
                {
                    // bottom-left triangle
                    triangles.Add( faceStart + row * (size + 1) + squareId);
                    triangles.Add(faceStart + (row + 1) * (size + 1) + squareId);
                    triangles.Add(faceStart + row * (size + 1) + squareId + 1);
                    // top-right triangle
                    triangles.Add(faceStart + row * (size + 1) + squareId + 1);
                    triangles.Add(faceStart + (row + 1) * (size + 1) + squareId);
                    triangles.Add(faceStart + (row + 1) * (size + 1) + squareId + 1);
                }
            }
        }

        mesh.vertices = vertices.SelectMany(x => x).ToArray();
        mesh.triangles = triangles.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
