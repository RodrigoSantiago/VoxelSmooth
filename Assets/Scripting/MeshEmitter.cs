
using System.Collections.Generic;
using UnityEngine;

public class MeshEmitter {
    
    private List<Vector3> points = new List<Vector3>();
    private List<int> triangles = new List<int>();
    public MeshEmitter() {
        
    }
    
    public void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3) {
        triangles.Add(points.Count);
        points.Add(p1);
        triangles.Add(points.Count);
        points.Add(p2);
        triangles.Add(points.Count);
        points.Add(p3);
    }

    public Mesh Build() {
        Mesh mesh = new Mesh();
        mesh.vertices = points.ToArray();
        mesh.triangles = triangles.ToArray();
        
        points.Clear();
        triangles.Clear();
        return mesh;
    }
        
}