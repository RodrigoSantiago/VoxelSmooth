using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMesh : MonoBehaviour {

    
    public static Vector3[] defPoints = {
        new Vector3(-1, -1, -1),
        new Vector3( 0, -1, -1),
        new Vector3( 1, -1, -1),
        new Vector3(-1,  0, -1),
        new Vector3( 0,  0, -1),
        new Vector3( 1,  0, -1),
        new Vector3(-1,  1, -1),
        new Vector3( 0,  1, -1),
        new Vector3( 1,  1, -1),
        
        new Vector3(-1, -1,  0),
        new Vector3( 0, -1,  0),
        new Vector3( 1, -1,  0),
        new Vector3(-1,  0,  0),
        new Vector3( 0,  0,  0),
        new Vector3( 1,  0,  0),
        new Vector3(-1,  1,  0),
        new Vector3( 0,  1,  0),
        new Vector3( 1,  1,  0),
        
        new Vector3(-1, -1,  1),
        new Vector3( 0, -1,  1),
        new Vector3( 1, -1,  1),
        new Vector3(-1,  0,  1),
        new Vector3( 0,  0,  1),
        new Vector3( 1,  0,  1),
        new Vector3(-1,  1,  1),
        new Vector3( 0,  1,  1),
        new Vector3( 1,  1,  1),
    };
    
    [HideInInspector]
    public Vector3[] points = {
        new Vector3(-1, -1, -1),
        new Vector3( 0, -1, -1),
        new Vector3( 1, -1, -1),
        new Vector3(-1,  0, -1),
        new Vector3( 0,  0, -1),
        new Vector3( 1,  0, -1),
        new Vector3(-1,  1, -1),
        new Vector3( 0,  1, -1),
        new Vector3( 1,  1, -1),
        
        new Vector3(-1, -1,  0),
        new Vector3( 0, -1,  0),
        new Vector3( 1, -1,  0),
        new Vector3(-1,  0,  0),
        new Vector3( 0,  0,  0),
        new Vector3( 1,  0,  0),
        new Vector3(-1,  1,  0),
        new Vector3( 0,  1,  0),
        new Vector3( 1,  1,  0),
        
        new Vector3(-1, -1,  1),
        new Vector3( 0, -1,  1),
        new Vector3( 1, -1,  1),
        new Vector3(-1,  0,  1),
        new Vector3( 0,  0,  1),
        new Vector3( 1,  0,  1),
        new Vector3(-1,  1,  1),
        new Vector3( 0,  1,  1),
        new Vector3( 1,  1,  1),
    };

    public Material mat;
    public Material mat2;
    public Transform point;
    
    public List<Vector3[]> extraTriangles = new List<Vector3[]>();
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnRenderObject() {
        render(true);
        render(false);
    }

    private void render(bool triangles) {
        GL.PushMatrix();
        if (triangles) {
            mat.SetPass(0);
        } else {
            mat2.SetPass(0);
        }

        GL.MultMatrix(Matrix4x4.identity);
        GL.Begin(triangles ? GL.TRIANGLES : GL.LINES);
        GL.Color(triangles ? Color.white : Color.black);
        

        if (!DebugController.blocks) {
            GL.Color(triangles ? Color.white : Color.black);
            foreach (var triangle in extraTriangles) {
                RenderTriangle(triangles, triangle[0], triangle[1], triangle[2]);
            }
        } else {
            RenderTriangle(triangles, 0, 1, 4);
            RenderTriangle(triangles, 0, 4, 3);

            RenderTriangle(triangles, 1, 2, 4);
            RenderTriangle(triangles, 2, 5, 4);

            RenderTriangle(triangles, 3, 4, 6);
            RenderTriangle(triangles, 4, 7, 6);

            RenderTriangle(triangles, 4, 8, 7);
            RenderTriangle(triangles, 4, 5, 8);


            RenderTriangle(triangles, 12, 21, 18);
            RenderTriangle(triangles, 12, 18, 9);

            RenderTriangle(triangles, 12, 9, 0);
            RenderTriangle(triangles, 12, 0, 3);

            RenderTriangle(triangles, 12, 3, 6);
            RenderTriangle(triangles, 12, 6, 15);

            RenderTriangle(triangles, 12, 15, 24);
            RenderTriangle(triangles, 12, 24, 21);


            RenderTriangle(triangles, 16, 15, 6);
            RenderTriangle(triangles, 16, 6, 7);

            RenderTriangle(triangles, 16, 7, 8);
            RenderTriangle(triangles, 16, 8, 17);

            RenderTriangle(triangles, 16, 17, 26);
            RenderTriangle(triangles, 16, 26, 25);

            RenderTriangle(triangles, 16, 25, 24);
            RenderTriangle(triangles, 16, 24, 15);


            RenderTriangle(triangles, 14, 23, 26);
            RenderTriangle(triangles, 14, 26, 17);

            RenderTriangle(triangles, 14, 17, 8);
            RenderTriangle(triangles, 14, 8, 5);

            RenderTriangle(triangles, 14, 5, 2);
            RenderTriangle(triangles, 14, 2, 11);

            RenderTriangle(triangles, 14, 11, 20);
            RenderTriangle(triangles, 14, 20, 23);


            RenderTriangle(triangles, 10, 2, 1);
            RenderTriangle(triangles, 10, 1, 0);

            RenderTriangle(triangles, 10, 0, 9);
            RenderTriangle(triangles, 10, 9, 18);

            RenderTriangle(triangles, 10, 18, 19);
            RenderTriangle(triangles, 10, 19, 20);

            RenderTriangle(triangles, 10, 20, 11);
            RenderTriangle(triangles, 10, 11, 2);


            RenderTriangle(triangles, 22, 24, 25);
            RenderTriangle(triangles, 22, 25, 26);

            RenderTriangle(triangles, 22, 26, 23);
            RenderTriangle(triangles, 22, 23, 20);

            RenderTriangle(triangles, 22, 20, 19);
            RenderTriangle(triangles, 22, 19, 18);

            RenderTriangle(triangles, 22, 18, 21);
            RenderTriangle(triangles, 22, 21, 24);
        }

        GL.End();
        GL.PopMatrix();
    }

    private void RenderTriangle(bool t, Vector3 point1, Vector3 point2, Vector3 point3) {
        var points = DebugController.blocks ? defPoints : this.points;
        
        Vector3 pos = point.position;
        Vector3 p1 = point1 * 0.5f * DebugController.meshScale + pos;
        Vector3 p2 = point2 * 0.5f * DebugController.meshScale + pos;
        Vector3 p3 = point3 * 0.5f * DebugController.meshScale + pos;
        if (!t) {
            GL.Vertex3(p1.x, p1.y, p1.z);
            GL.Vertex3(p3.x, p3.y, p3.z);
            GL.Vertex3(p3.x, p3.y, p3.z);
            GL.Vertex3(p2.x, p2.y, p2.z);
            GL.Vertex3(p2.x, p2.y, p2.z);
            GL.Vertex3(p1.x, p1.y, p1.z);
        } else {
            GL.Vertex3(p1.x, p1.y, p1.z);
            GL.Vertex3(p3.x, p3.y, p3.z);
            GL.Vertex3(p2.x, p2.y, p2.z);
        }
    }
    
    private void RenderTriangle(bool t, int point1, int point2, int point3) {
        var points = DebugController.blocks ? defPoints : this.points;
        
        Vector3 pos = point.position;
        Vector3 p1 = points[point1] * 0.5f * DebugController.meshScale + pos;
        Vector3 p2 = points[point2] * 0.5f * DebugController.meshScale + pos;
        Vector3 p3 = points[point3] * 0.5f * DebugController.meshScale + pos;
        if (!t) {
            GL.Vertex3(p1.x, p1.y, p1.z);
            GL.Vertex3(p3.x, p3.y, p3.z);
            GL.Vertex3(p3.x, p3.y, p3.z);
            GL.Vertex3(p2.x, p2.y, p2.z);
            GL.Vertex3(p2.x, p2.y, p2.z);
            GL.Vertex3(p1.x, p1.y, p1.z);
        } else {
            GL.Vertex3(p1.x, p1.y, p1.z);
            GL.Vertex3(p3.x, p3.y, p3.z);
            GL.Vertex3(p2.x, p2.y, p2.z);
        }
    }
}
