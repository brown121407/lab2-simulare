using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NoisySphere : MonoBehaviour
{
    [SerializeField]
    private int size = 10;

    [SerializeField] 
    private float sideLength = 1.0f;

    [SerializeField] 
    private float displacement = 0.5f;
    
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
                    faceVertices.Add(new Vector3(-size * sideLength / 2 +  column * sideLength, -size * sideLength / 2 + row * sideLength, 0));
                }
            }
            vertices.Add(faceVertices);
        }

        var rotations = new[]
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
            new(0, 0, -size * sideLength / 2),
            new(-size * sideLength / 2, 0, 0),
            new(0, 0, size * sideLength / 2),
            new(size * sideLength / 2, 0, 0),
            new(0, size * sideLength / 2),
            new(0, -size * sideLength / 2, 0),
        };

        vertices = vertices
            .Select((faceVertices, index) => 
                faceVertices
                    .Select(v => rotations[index] * v + offsets[index])
                    .Select(Vector3.Normalize)
                    .Select(vertex => vertex * (1.0f + Perlin.Noise(vertex.x, vertex.y, vertex.z) * displacement))
                    .ToList())
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
