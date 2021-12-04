using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualGenerator : MeshGenerator {
   
    private int width, height, length;
    private DualContouring mb;
    
    public DualGenerator(int width, int height, int length) {
        this.width = width;
        this.height = height;
        this.length = length;
        mb = new DualContouring(width, height, length);
    }
    
    public override void Generate(Chunk chunk, MeshEmitter meshEmitter) {
        float[] justTest = new float[chunk.Width * chunk.Height * chunk.Length];
        for (int i = 0; i < justTest.Length; i++) {
            justTest[i] = chunk.Get(i) / 1000f;
        }
        List<Vector3> vertex = new List<Vector3>();
        List<int> index = new List<int>();
	    
        mb.Generate(justTest, width, height, length, vertex, index);

        meshEmitter.AddLists(vertex, index);
    }

    public override Mesh Regenerate(Chunk chunk, Mesh mesh, int x, int y, int z, int width, int height, int length) {
        throw new System.NotImplementedException();
    }
    
}

public struct IndexedVertex {
    public Vector3 pos;
    public int index;
}
public class DualContouring {

    private float[] cube = new float[8];
    private IndexedVertex[,,] verPos;
    private int width;
    private int height;
    private int length;
    
    public DualContouring(int width, int height, int length) {
        this.width = width;
        this.height = height;
        this.length = length;
        verPos = new IndexedVertex[width, height, length];
    }
    
    protected static readonly int[,] VertexOffset = new int[,] {
        {0, 0, 0}, {1, 0, 0}, 
        {1, 1, 0}, {0, 1, 0},
        {0, 0, 1}, {1, 0, 1},
        {1, 1, 1}, {0, 1, 1}
    };

    protected static readonly int[,] EdgeOrder = new int[,] {
        {0, 1}, {1, 2}, {2, 3}, {3, 0},
        {4, 5}, {5, 6}, {6, 7}, {7, 4},
        {0, 4}, {1, 5}, {2, 6}, {3, 7},
    };

    protected static readonly int[,] EdgeOffset = new int[,] {
        {0, 0, 0}, {0, 0, 1}, {0, 1, 0}, {1, 0, 0},
    };
    
    public void Generate(float[] voxels, int width, int height, int length, List<Vector3> verts, List<int> indices) {

        int n = 0;
        for (int x = 0; x < width - 1; x++) {
            for (int y = 0; y < height - 1; y++) {
                for (int z = 0; z < length - 1; z++) {
                    //Get the values in the 8 neighbours which make up a cube
                    Vector3 p = new Vector3();
                    int xx = 0, yy = 0, zz = 0;
                    int found = 0;
                    for (int i = 0; i < 12; i++) {
                        int x1 = x + VertexOffset[EdgeOrder[i, 0], 0];
                        int y1 = y + VertexOffset[EdgeOrder[i, 0], 1];
                        int z1 = z + VertexOffset[EdgeOrder[i, 0], 2];

                        int x2 = x + VertexOffset[EdgeOrder[i, 1], 0];
                        int y2 = y + VertexOffset[EdgeOrder[i, 1], 1];
                        int z2 = z + VertexOffset[EdgeOrder[i, 1], 2];

                        float d1 = voxels[x1 + y1 * width + z1 * width * height];
                        float d2 = voxels[x2 + y2 * width + z2 * width * height];
                        if (d1 > 0.5 != d2 > 0.5) {
                            float delta = d2 - d1;
                            float d =  (delta == 0.0f) ? 0.5f : ( 0.5f - d1) / (delta);
                            if (x1 != x2) {
                                xx++;
                                p.x += Mathf.Lerp(x1, x2, d);
                            }
                            if (y1 != y2) {
                                yy++;
                                p.y += Mathf.Lerp(y1, y2, d);
                            }
                            if (z1 != z2) {
                                zz++;
                                p.z += Mathf.Lerp(z1, z2, d);
                            }
                            //p += Vector3.Lerp(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2), d);
                            found++;
                        }
                    }

                    if (found > 0) {
                        var pos = new Vector3(xx == 0 ? x : p.x / xx, yy == 0 ? y : p.y / yy, zz == 0 ? z : p.z / zz);
                        //var pos = p /= found;
                            
                        verPos[x, y, z] = new IndexedVertex {
                            pos = pos, 
                            index = n++
                        };
                        verts.Add(pos);
                    }
                }
            }
        }
        
        for (int x = 0; x < width - 1; x++) {
            for (int y = 0; y < height - 1; y++) {
                for (int z = 0; z < length - 1; z++) {

                    for (int i = 0; i < 4; i++) {
                        int ix = x + EdgeOffset[i, 0];
                        int iy = y + EdgeOffset[i, 1];
                        int iz = z + EdgeOffset[i, 2];
                        cube[i] = voxels[ix + iy * width + iz * width * height];
                    }

                    //Perform algorithm
                    MarchTriangles(x, y, z, cube, verts, indices);
                }
            }
        }
    }

    protected void MarchPoints(int x, int y, int z, float[] voxels) {
        
    }
    
    protected void MarchTriangles(int x, int y, int z, float[] cube, IList<Vector3> vertList, IList<int> indexList) {
        bool solid1 = cube[0] > 0.5f;
        
        if (x > 0 && y > 0) {
            bool solid2 = cube[1] > 0.5f;
            if (solid1 != solid2) {
                addQuad(
                    verPos[x - 1, y - 1, z].index,
                    verPos[x - 0, y - 1, z].index,
                    verPos[x - 0, y - 0, z].index,
                    verPos[x - 1, y - 0, z].index,
                    solid2, indexList);
                
                // solid2 make winding inverse
            }
        }

        if (x > 0 && z > 0) {
            bool solid2 = cube[2] > 0.5f;
            if (solid1 != solid2) {
                addQuad(
                    verPos[x - 1, y, z - 1].index,
                    verPos[x - 0, y, z - 1].index,
                    verPos[x - 0, y, z - 0].index,
                    verPos[x - 1, y, z - 0].index,
                    solid1, indexList);

                // solid1 make winding inverse
            }
        }

        if (y > 0 && z > 0) {
            bool solid2 = cube[3] > 0.5f;
            if (solid1 != solid2) {
                addQuad(
                    verPos[x, y - 1, z - 1].index,
                    verPos[x, y - 0, z - 1].index,
                    verPos[x, y - 0, z - 0].index,
                    verPos[x, y - 1, z - 0].index,
                    solid2, indexList);

                // solid2 make winding inverse
            }
        }
    }

    protected void addQuad(int i0, int i1, int i2, int i3, bool winding, IList<int> indices) {
        if (!winding) {
            indices.Add(i0);
            indices.Add(i1);
            indices.Add(i2);

            indices.Add(i0);
            indices.Add(i2);
            indices.Add(i3);
        } else {
            indices.Add(i0);
            indices.Add(i2);
            indices.Add(i1);

            indices.Add(i0);
            indices.Add(i3);
            indices.Add(i2);
        }
    }
}