
using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshEmitter {
    
    private List<Vector3> points = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private bool reduce;

    public MeshEmitter() {
        this.reduce = false;
    }
    public MeshEmitter(bool reduce) {
        this.reduce = reduce;
    }

    private int contains(List<Vector3> points, Vector3 point) {
        for (int i = 0; i < points.Count; i++) {
            if (Math.Abs(point.x - points[i].x) < 0.001f &&
                Math.Abs(point.y - points[i].y) < 0.001f &&
                Math.Abs(point.z - points[i].z) < 0.001f) {
                return i;
            }
        }

        return -1;
    }
    
    public void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 n1, Vector3 n2, Vector3 n3) {
        triangles.Add(points.Count);
        points.Add(p1);
        normals.Add(n1);
        triangles.Add(points.Count);
        points.Add(p2);
        normals.Add(n2);
        triangles.Add(points.Count);
        points.Add(p3);
        normals.Add(n3);
    }
    
    public void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3) {
        triangles.Add(points.Count);
        points.Add(p1);
        triangles.Add(points.Count);
        points.Add(p2);
        triangles.Add(points.Count);
        points.Add(p3);
    }

    public void AddLists(List<Vector3> vertex, List<int> indicex, List<Vector3> normals) {
        this.points = vertex;
        this.triangles = indicex;
        this.normals = normals;
    }

    public void AddLists(List<Vector3> vertex, List<int> indicex) {
        this.points = vertex;
        this.triangles = indicex;
    }

    public Mesh Build() {
        Mesh mesh = new Mesh();
        if (normals.Count > 0) {
            var ver = points.ToArray();
            var tri = triangles.ToArray();
            var nor = normals.ToArray();
            mesh.vertices = ver;
            mesh.triangles = tri;
            mesh.normals = nor;
            mesh.RecalculateBounds();
        } else if (reduce) {
            List<Vector3> npoints = new List<Vector3>();
            List<int> ntriangles = new List<int>();
            for (int i = 0; i < triangles.Count; i++) {
                int index = contains(npoints, points[triangles[i]]);
                if (index == -1) {
                    npoints.Add(points[triangles[i]]);
                    ntriangles.Add(npoints.Count - 1);
                } else {
                    ntriangles.Add(index);
                }
            }
            var ver = npoints.ToArray();
            var tri = ntriangles.ToArray();
            mesh.vertices = ver;
            mesh.triangles = tri;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        } else {
            var ver = points.ToArray();
            var tri = triangles.ToArray();
            mesh.vertices = ver;
            mesh.triangles = tri;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
        mesh.RecalculateTangents();
        
        points.Clear();
        triangles.Clear();
        return mesh;
    }
    
    
}