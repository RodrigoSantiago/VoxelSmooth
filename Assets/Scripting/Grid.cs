using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    
    private bool[] data;
    private Vector3[] tempVectors;
    private bool[] subBlock;
    
    public int width, height, length;

    public bool get(int x, int y, int z) {
        return x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < length &&
               data[x + y * width + z * height * width];
    }

    public void set(int x, int y, int z, bool value) {
        if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < length) {
            data[x + y * width + z * height * width] = value;
        }
    }

    public bool getNear(int x, int y, int z, Vector3Int off) {
        return get(x + off.x, y + off.y, z + off.z);
    }
    
    public bool getNear(int x, int y, int z, Off3D off) {
        return get(x + off.x, y + off.y, z + off.z);
    }
    
    public Transform owner;
    public Transform gridOwner;
    public GameObject prefabGrid;
    public GameObject prefabVertex;
    public GameObject prefabEndBlock;
    public GameObject[] prefabPoints;

    public void SetUp() {
        data = new bool[width * height * length];
        
        /* while (gridOwner.childCount > 0) {
             Destroy(gridOwner.GetChild(0).gameObject);
         }
         
         for (int x = 0; x < width; x++) {
             for (int y = 0; y < height; y++) {
                 for (int z = 0; z < length; z++) {
                     InstanceGrid(prefabGrid, new Vector3(x, y, z));
                 }
             }
         }*/
        
        var objects = GameObject.FindGameObjectsWithTag("Point");
        foreach (var obj in objects) {
            var p = obj.transform.position;
            set(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z), true);
        }
    }
    
    public void Start() {
        SetUp();
        
        Calculate();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SetUp();
            Calculate();
        }
    }

    public void markSubBlock(int px, int py, int pz, Off3D off) {
        int x = px * 2 + (off.x == -1 ? 0 : 1);
        int y = py * 2 + (off.y == -1 ? 0 : 1);
        int z = pz * 2 + (off.z == -1 ? 0 : 1);
        int index = x + y * (width * 2) + z * (width * 2) * (height * 2);
        if (index >= 0 && index < subBlock.Length) {
            subBlock[index] = true;
        }
    }

    public bool IsSubBlockMarked(int px, int py, int pz, Off3D off) {
        int x = px * 2 + (off.x == -1 ? 0 : 1);
        int y = py * 2 + (off.y == -1 ? 0 : 1);
        int z = pz * 2 + (off.z == -1 ? 0 : 1);
        int index = x + y * (width * 2) + z * (width * 2) * (height * 2);
        if (index >= 0 && index < subBlock.Length) {
            return subBlock[index];
        }

        return false;
    }
    
    public Vector3 getTempVertexOffset(int px, int py, int pz, Vector3Int relative, Off3D off) {
        return getTempVertex(px + relative.x, py + relative.y, pz + relative.z, off) + relative * 2;
    }
    
    public Vector3 getTempVertexOffset(Vector3Int pos, Vector3Int relative, Off3D off) {
        return getTempVertex(pos.x + relative.x, pos.y + relative.y, pos.z + relative.z, off) + relative * 2;
    }
    
    public Vector3 getTempVertex(Vector3Int pos, Off3D off) {
        return getTempVertex(pos.x, pos.y, pos.z, off);
    }

    public Vector3 getTempVertex(int px, int py, int pz, Off3D off) {
        int x = px * 2 + 1 + off.x;
        int y = py * 2 + 1 + off.y;
        int z = pz * 2 + 1 + off.z;
        var value = new Vector3(px, py, pz);
        int index = x + y * (width * 2 + 1) + z * (width * 2 + 1) * (height * 2 + 1);
        if (index >= 0 && index < tempVectors.Length) {
            value = tempVectors[index];
        }
        value.x = value.x + off.x;
        value.y = value.y + off.y;
        value.z = value.z + off.z;
        return value;
    }
    
    public void setTempVertex(int px, int py, int pz, Off3D off, Vector3 value) {
        int x = px * 2 + 1 + off.x;
        int y = py * 2 + 1 + off.y;
        int z = pz * 2 + 1 + off.z;
        int index = x + y * (width * 2 + 1) + z * (width * 2 + 1) * (height * 2 + 1);
        if (index >= 0 && index < tempVectors.Length) {
            value.x = value.x - off.x;
            value.y = value.y - off.y;
            value.z = value.z - off.z;
            tempVectors[index] = value;
        }
    }

    public void Calculate() {
        while (owner.childCount > 0) {
            Destroy(owner.GetChild(0).gameObject);
        }

        tempVectors = new Vector3[(width * 2 + 1) * (height * 2 + 1) * (length * 2 + 1)];
        subBlock = new bool[width * 2 * height * 2 * length * 2];
        
        Dictionary<Vector3Int, CubeMesh> meshes = new Dictionary<Vector3Int, CubeMesh>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < length; z++) {
                    if (!get(x, y, z)) continue;
                    
                    var endInst = Instance(prefabEndBlock, new Vector3(x, y, z));
                    var mesh = endInst.GetComponent<CubeMesh>();
                    genMesh(x, y, z, mesh);
                    meshes.Add(new Vector3Int(x, y, z), mesh);
                }
            }
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < length; z++) {
                    if (!get(x, y, z)) continue;
                    var mesh = meshes[new Vector3Int(x, y, z)];
                    genExtra(x, y, z, mesh);
                    //genExtraLow(x, y, z, mesh);
                }
            }
        }
    }

    public void genMesh(int x, int y, int z, CubeMesh mesh) {
        bool[] near = new bool[27];
        for (int i = 0; i < 27; i++) {
            near[i] = getNear(x, y, z, i);
        }

        for (int i = 0; i < 27; i++) {
            var off = new Off3D(i);
            var point = mesh.points[i];
            var roundPoint = point;
            
            if (getNear(x, y, z, off)) {
                // NEVER ROUND
                
            } else if (off.IsSide) {
                roundPoint = genSide(near, off);
                
            } else if (off.IsCorner) {
                roundPoint = genCorner(near, off);
                
            } else if (off.IsEdge) {
                roundPoint = genEdge(near, off);
                
            }

            mesh.points[i] = roundPoint;
            setTempVertex(x, y, z, off, roundPoint);
        }
    }

    private Vector3 genSide(bool[] near, Off3D off) {
        var point = off.vec3;
        
        var ax = off + off.HorAxis;
        var nax = off + off.VerAxis;
        var rax = off + off.IHorAxis;
        var rnax = off + off.IVerAxis;
                
        if (!near[off.HorAxis] || !near[off.IHorAxis] ||
            !near[off.VerAxis] || !near[off.IVerAxis]) {
            if (!near[ax] && !near[nax] &&
                !near[rax] && !near[rnax] &&
                !near[ax + nax] && !near[ax + rnax] &&
                !near[rax + nax] && !near[rax + rnax]) {
                
                return point * 0.7f;
            }
        }
        
        return off.vec3;
    }

    private Vector3 genCorner(bool[] near, Off3D off) {
        Vector3 point = off.vec;
        Vector3 roundPoint = off.vec;
        
        bool h = near[off.HorAxis];
        bool v = near[off.VerAxis];
        if (!h && !v) {
            // Diagonal Exception
            if (!near[off.Tangent + off] && !near[off.ITangent + off]) {

                // Round
                roundPoint = point * 0.5f;

                // Ramp Exception
                if (near[off.Tangent + off.VerAxis] || near[off.ITangent + off.VerAxis]) {
                    if (off.VerAxis.x != 0) roundPoint.x = point.x;
                    else if (off.VerAxis.y != 0) roundPoint.y = point.y;
                    else if (off.VerAxis.z != 0) roundPoint.z = point.z;
                }

                if (near[off.Tangent + off.HorAxis] || near[off.ITangent + off.HorAxis]) {
                    if (off.HorAxis.x != 0) roundPoint.x = point.x;
                    else if (off.HorAxis.y != 0) roundPoint.y = point.y;
                    else if (off.HorAxis.z != 0) roundPoint.z = point.z;
                }

                return roundPoint;
            }
        } else if (h != v) {
            var lineAx = v ? off.HorAxis : off.VerAxis;
            var lineNax = h ? off.HorAxis : off.VerAxis;

            // Includes Ramp Exception
            if (!near[lineAx] && !near[off.Tangent + lineAx] && !near[off.ITangent + lineAx] &&
                (!near[off.Tangent] || !near[off.ITangent])) {

                // Includes Neighbor Ramp Exception
                if (!near[lineAx + lineNax] &&
                    !near[off.Tangent + lineAx + lineNax] && !near[off.ITangent + lineAx + lineNax] &&
                    (!near[off.Tangent + lineNax] || !near[off.ITangent + lineNax])) {

                    if (lineAx.x != 0) roundPoint.x *= 0.7f;
                    else if (lineAx.y != 0) roundPoint.y *= 0.7f;
                    else if (lineAx.z != 0) roundPoint.z *= 0.7f;
                    
                    return roundPoint;
                }

            }
        }
        return roundPoint;
    }

    private Vector3 genEdge(bool[] near, Off3D off) {
        Vector3 point = off.vec;
        Vector3 roundPoint = off.vec;
        
        bool px = near[off.CloserX];
        bool py = near[off.CloserY];
        bool pz = near[off.CloserZ];
        bool pxy = near[off.CloserXY];
        bool pyz = near[off.CloserYZ];
        bool pzx = near[off.CloserZX];
        int n = (px ? 1 : 0) + (py ? 1 : 0) + (pz ? 1 : 0) + (pxy ? 1 : 0) + (pyz ? 1 : 0) + (pzx ? 1 : 0);
        if (n == 1) {
            roundPoint = point * ((px || py || pz) ? 0.5f : 0.7f);
            if (px || pxy || pzx) roundPoint.x = point.x;
            if (py || pxy || pyz) roundPoint.y = point.y;
            if (pz || pyz || pzx) roundPoint.z = point.z;
        } else if (n == 0) {
            if (near[off.CloserXY - off.CloserZ] ||
                near[off.CloserYZ - off.CloserX] ||
                near[off.CloserZX - off.CloserY]) {
                roundPoint = point * 0.5f;
            } else {
                roundPoint = point * 0.4f;
                        
                // Diagonal Ramp Exception
                bool dXY = near[off.CloserY - off.CloserX] && near[off.CloserX - off.CloserY];
                bool dYZ = near[off.CloserZ - off.CloserY] && near[off.CloserY - off.CloserZ];
                bool dZX = near[off.CloserX - off.CloserZ] && near[off.CloserZ - off.CloserX];
                if (dXY || dYZ || dZX) {
                    roundPoint = point * 0.5f;
                }
                if (dXY || dZX) roundPoint.x = point.x * 0.5f;
                if (dXY || dYZ) roundPoint.y = point.y * 0.5f;
                if (dYZ || dZX) roundPoint.z = point.z * 0.5f;
            }
        }

        return roundPoint;
    }

    public void genExtra(int x, int y, int z, CubeMesh mesh) {
        bool blocky = true;
        bool[] near = new bool[27];
        for (int i = 0; i < 27; i++) {
            if (near[i] = getNear(x, y, z, i)) {
                blocky = false;
            }
        }
        if (blocky) return;

        for (int i = 0; i < 27; i++) {
            Off3D off = i;
            if (!off.IsSide || getNear(x, y, z, off)) continue;

            Off3D[] axis = {off.HorAxis, off.VerAxis, off.IHorAxis, off.IVerAxis};
            for (int j = 0; j < 4; j++) {
                var ax = axis[j];
                var nax = axis[(j + 1) % 4];
                var dax = ax + nax;
                if (!IsSubBlockMarked(x + off.x, y + off.y, z + off.z, dax - off)) {
                    if (near[ax + off] && near[nax + off]) {
                        genExtraRampTriple(x, y, z, mesh, off, ax, nax);
                        
                    } else if (near[ax + off]) {
                        genExtraRamp(x, y, z, mesh, off, ax, nax);
                        
                    } else if (near[nax + off]) {
                        genExtraRamp(x, y, z, mesh, off, nax, ax);
                        
                    } else if (near[dax + off]) {
                        genExtraRampDiagonal(x, y, z, mesh, off, ax, nax);
                        
                    } else {
                        genExtraFloor(x, y, z, mesh, off, ax, nax);
                        
                    }

                    markSubBlock(x + off.x, y + off.y, z + off.z, dax - off);
                }
            }
        }
    }

    private bool IsSideRound(Vector3Int pos, Off3D off) {
        return IsSideRound(pos.x, pos.y, pos.z, off);
    }

    private bool IsSideRound(int x, int y, int z, Off3D off) {
        if (!get(x, y, z) || getNear(x, y, z, off)) return false;

        var point = off.vec3;

        var ax = off + off.HorAxis;
        var nax = off + off.VerAxis;
        var rax = off + off.IHorAxis;
        var rnax = off + off.IVerAxis;

        var nearHor = getNear(x, y, z, off.HorAxis);
        var nearIHor = getNear(x, y, z, off.IHorAxis);
        var nearVer = getNear(x, y, z, off.VerAxis);
        var nearIVer = getNear(x, y, z, off.IVerAxis);

        var nearAx = getNear(x, y, z, ax);
        var nearNax = getNear(x, y, z, nax);
        var nearRax = getNear(x, y, z, rax);
        var nearRnax = getNear(x, y, z, rnax);

        if (!nearHor || !nearIHor ||
            !nearVer || !nearIVer) {
            if (!nearAx && !nearNax &&
                !nearRax && !nearRnax &&
                !getNear(x, y, z, ax + nax) && !getNear(x, y, z, ax + rnax) &&
                !getNear(x, y, z, rax + nax) && !getNear(x, y, z, rax + rnax)) {

                return false;
            }
        }

        // Pre - Roudning
        int count = ((nearAx ? 1 : 0) + (nearRax ? 1 : 0) + (nearNax ? 1 : 0) + (nearRnax ? 1 : 0));
        if (count > 0 &&
            ((nearHor && nearRax) ||
             (nearIHor && nearAx) ||
             (nearVer && nearRnax) ||
             (nearIVer && nearNax) ||
             (nearAx && nearRax) ||
             (nearNax && nearRnax) || count >= 3)) {

            return true;
        }

        return false;
    }

    private bool IsCornerRound(Vector3Int pos, Off3D off, Off3D upAx, Off3D sideAx) {
        return IsSideRound(pos, upAx) && IsSideRound(pos + sideAx.vec, upAx);
    }

    private void genExtraFloor(int x, int y, int z, CubeMesh mesh, Off3D off, Off3D ax, Off3D nax) {
        var rAx = ax.Opposite;
        var rNax = nax.Opposite;
        
        Vector3Int pos = new Vector3Int(x, y, z);
        Vector3 p1 = getTempVertex(pos, off);
        Vector3 p2 = getTempVertex(pos, off + ax);
        Vector3 p3 = getTempVertex(pos, off + nax);
        Vector3 p4 = getTempVertex(pos, off + ax + nax);

        if (IsSideRound(pos, off)) {
            p1 += off.vec3 * 0.3f;
        }
        if (IsCornerRound(pos, 0, off, ax)) {
            p2 += off.vec3 * 0.3f;
        }
        if (IsCornerRound(pos, 0, off, nax)) {
            p3 += off.vec3 * 0.3f;
        }
        mesh.extraTriangles.Add(new []{p1, p4, p2});
        mesh.extraTriangles.Add(new []{p1, p3, p4});
    }

    private void genExtraRamp(int x, int y, int z, CubeMesh mesh, Off3D off, Off3D ax, Off3D nax) {
        var rAx = ax.Opposite;
        var rNax = nax.Opposite;
        Vector3Int pos = new Vector3Int(x, y, z);
        Vector3 p1 = getTempVertex(pos, off);
        Vector3 p2 = getTempVertex(pos, off + nax);
        Vector3 p3 = getTempVertexOffset(pos, (off + ax).vec, rAx);
        Vector3 p4 = getTempVertexOffset(pos, (off + ax).vec, rAx + nax);
        Vector3 p5 = (p1 + p3) / 2;
        Vector3 p6 = (p2 + p4) / 2;

        bool roundP2 = IsCornerRound(pos, 0, off, nax);
        bool roundP4 = IsCornerRound(pos + ax.vec + off.vec, 0, rAx, nax);
        Vector3 pp1 = IsSideRound(pos, off) ? p1 + off.vec3 * 0.3f : p1;
        Vector3 pp2 = roundP2 ? p2 + off.vec3 * 0.3f : p2;
        Vector3 pp3 = IsSideRound(pos + ax.vec + off.vec, rAx) ? p3 + rAx.vec3 * 0.3f : p3;
        Vector3 pp4 = roundP4 ? p4 + rAx.vec3 * 0.3f : p4;

        mesh.extraTriangles.Add(new []{pp1, pp2, p6});
        mesh.extraTriangles.Add(new []{pp1, p6, p5});
        mesh.extraTriangles.Add(new []{p5, p6, pp3});
        mesh.extraTriangles.Add(new []{p6, pp4, pp3});
        
        // Closure
        if (!getNear(x, y, z, off + ax + nax) && !getNear(x, y, z, nax)) {
            if (getNear(x, y, z, ax + nax)) {
                if (!IsSubBlockMarked(x + off.x + nax.x, y + off.y + nax.y, z + off.z + nax.z, ax + rNax - off)) {
                    Vector3 p7 = getTempVertexOffset(pos, (ax + nax).vec, off + rAx);
                    Vector3 p8 = (pp4 + p7) / 2; // ?que ? nao seria p4
                    Vector3 p9 = (pp2 + p7) / 2;
                    Vector3 p0 = (pp2 + p4 + p7) / 3;
                    mesh.extraTriangles.Add(new[] {pp2, p0, p6});
                    mesh.extraTriangles.Add(new[] {p6, p0, pp4});
                    mesh.extraTriangles.Add(new[] {pp4, p0, p8});
                    mesh.extraTriangles.Add(new[] {p8, p0, p7});
                    mesh.extraTriangles.Add(new[] {p7, p0, p9});
                    mesh.extraTriangles.Add(new[] {p9, p0, pp2});
                    markSubBlock(x + off.x + nax.x, y + off.y + nax.y, z + off.z + nax.z, ax + rNax - off);
                }
            } else {
                Vector3 p7 = getTempVertex(pos, off + ax + nax);
                mesh.extraTriangles.Add(new[] {pp2, p7, p6});
                if (roundP2) {
                    mesh.extraTriangles.Add(new[] {p2, p7, pp2});
                }
                mesh.extraTriangles.Add(new[] {p6, p7, pp4});
                if (roundP4) {
                    mesh.extraTriangles.Add(new[] {pp4, p7, p4});
                }
            }
        }
    }
    
    private void genExtraRampTriple(int x, int y, int z, CubeMesh mesh, Off3D off, Off3D ax, Off3D nax) {
        var rAx = ax.Opposite;
        var rNax = nax.Opposite;
        Vector3Int pos = new Vector3Int(x, y, z);
        Vector3 p1 = getTempVertex(pos, off);
        Vector3 p2 = getTempVertexOffset(pos, (off + ax).vec, rAx);
        Vector3 p3 = getTempVertexOffset(pos, (off + nax).vec, rNax);
        Vector3 p12 = (p1 + p2) / 2;
        Vector3 p23 = (p2 + p3) / 2;
        Vector3 p31 = (p3 + p1) / 2;
        Vector3 p0 = (p1 + p2 + p3) / 3;

        if (IsSideRound(pos, off)) {
            p1 += off.vec3 * 0.3f;
        }
        if (IsSideRound(pos + ax.vec + off.vec, rAx)) {
            p2 += rAx.vec3 * 0.3f;
        }
        if (IsSideRound(pos + nax.vec + off.vec, rNax)) {
            p3 += rNax.vec3 * 0.3f;
        }
        
        mesh.extraTriangles.Add(new []{p1, p0, p12});
        mesh.extraTriangles.Add(new []{p12, p0, p2});
        mesh.extraTriangles.Add(new []{p2, p0, p23});
        mesh.extraTriangles.Add(new []{p23, p0, p3});
        mesh.extraTriangles.Add(new []{p3, p0, p31});
        mesh.extraTriangles.Add(new []{p31, p0, p1});
    }
    
    private void genExtraRampDiagonal(int x, int y, int z, CubeMesh mesh, Off3D off, Off3D ax, Off3D nax) {
        var rAx = ax.Opposite;
        var rNax = nax.Opposite;
        Vector3Int pos = new Vector3Int(x, y, z);
        Vector3 p1 = getTempVertex(pos, off);
        Vector3 p2 = getTempVertex(pos, off + ax);
        Vector3 p3 = getTempVertex(pos, off + nax);
        Vector3 p4 = getTempVertexOffset(pos, (off + ax + nax).vec, rAx + rNax);
        Vector3 p5 = (p1 + p4) / 2;
        Vector3 p6 = (p2 + p4) / 2;
        Vector3 p7 = (p3 + p4) / 2;
        
        if (IsSideRound(pos, off)) {
            p1 += off.vec3 * 0.3f;
        }
        if (IsCornerRound(pos, 0, off, ax)) {
            p2 += off.vec3 * 0.3f;
        }
        if (IsCornerRound(pos, 0, off, nax)) {
            p3 += off.vec3 * 0.3f;
        }
        
        mesh.extraTriangles.Add(new []{p1, p5, p6});
        mesh.extraTriangles.Add(new []{p1, p6, p2});
        mesh.extraTriangles.Add(new []{p1, p7, p5});
        mesh.extraTriangles.Add(new []{p1, p3, p7});
        mesh.extraTriangles.Add(new []{p5, p4, p6});
        mesh.extraTriangles.Add(new []{p5, p7, p4});
    }

    public void genExtraLow(int x, int y, int z, CubeMesh mesh) {
        for (int i = 0; i < 27; i++) {
            var off = new Off3D(i);
            if (!off.IsSide || getNear(x, y, z, off)) continue;

            Off3D[] axis = {off.HorAxis, off.VerAxis, off.IHorAxis, off.IVerAxis};
            for (int j = 0; j < 4; j++) {
                var ax = axis[j];
                var nax = axis[(j + 1) % 4];
                var dax = ax + nax;
                var rax = axis[(j + 2) % 4];
                var rnax = axis[(j + 3) % 4];
                if (!IsSubBlockMarked(x + off.x, y + off.y, z + off.z, new Off3D(dax.vec - off.vec))) {
                    bool triAx = getNear(x, y, z, off.vec + ax.vec);
                    bool triNax = getNear(x, y, z, off.vec + nax.vec);
                    bool triDax = getNear(x, y, z, off.vec + dax.vec);
                    bool triLax = getNear(x, y, z, ax.vec);
                    bool triLnax = getNear(x, y, z, nax.vec);
                    bool triLdax = getNear(x, y, z, dax.vec);
                    bool closed = false;
                    if (triAx && !triDax && !triNax && !triLnax && triLdax) {
                        closed = true;
                        var rNax = axis[(j + 3) % 4];
                        if (!IsSubBlockMarked(x + off.x + nax.x, y + off.y + nax.y, z + off.z + nax.z,
                            new Off3D(ax.vec + rNax.vec - off.vec))) {
                            Vector3 p1 = getTempVertex(x, y, z, new Off3D(off.vec + nax.vec));
                            Vector3 p2 = getTempVertexOffset(x, y, z, off.vec, dax);
                            Vector3 p3 = getTempVertexOffset(x, y, z, nax.vec, new Off3D(off.vec + ax.vec));
                            mesh.extraTriangles.Add(new[] {p1, p2, p3});
                            markSubBlock(x + off.x + nax.x, y + off.y + nax.y, z + off.z + nax.z,
                                new Off3D(ax.vec + rNax.vec - off.vec));
                        }
                    }

                    if ((triAx && triNax)) {
                        Vector3 p1 = getTempVertex(x, y, z, off);
                        Vector3 p2 = getTempVertexOffset(x, y, z, off.vec, ax);
                        Vector3 p3 = getTempVertexOffset(x, y, z, off.vec, nax);
                        mesh.extraTriangles.Add(new[] {p1, p2, p3});
                    } else if (!triAx && !triNax && triLax && triLnax && triDax) {
                        Vector3 p1 = getTempVertex(x, y, z, new Off3D(off.vec + ax.vec));
                        Vector3 p2 = getTempVertex(x, y, z, new Off3D(off.vec + nax.vec));
                        Vector3 p3 = getTempVertexOffset(x, y, z, off.vec, dax);
                        mesh.extraTriangles.Add(new[] {p1, p2, p3});
                    } else {
                        if (triAx) {
                            Vector3 p1 = getTempVertex(x, y, z, off);
                            Vector3 p2 = getTempVertexOffset(x, y, z, off.vec, ax);
                            Vector3 p3 = getTempVertexOffset(x, y, z, off.vec, dax);
                            mesh.extraTriangles.Add(new[] {p1, p2, p3});
                        }

                        if (!triNax && (triAx || triDax)) {
                            Vector3 p1 = getTempVertex(x, y, z, off);
                            Vector3 p2 = getTempVertex(x, y, z, new Off3D(off.vec + nax.vec));
                            Vector3 p3 = getTempVertexOffset(x, y, z, off.vec, dax);
                            mesh.extraTriangles.Add(new[] {p1, p2, p3});
                        }

                        if (triNax) {
                            Vector3 p1 = getTempVertex(x, y, z, off);
                            Vector3 p2 = getTempVertexOffset(x, y, z, off.vec, nax);
                            Vector3 p3 = getTempVertexOffset(x, y, z, off.vec, dax);
                            mesh.extraTriangles.Add(new[] {p1, p2, p3});
                        }

                        if (!triAx && (triNax || triDax)) {
                            Vector3 p1 = getTempVertex(x, y, z, off);
                            Vector3 p2 = getTempVertex(x, y, z, new Off3D(off.vec + ax.vec));
                            Vector3 p3 = getTempVertexOffset(x, y, z, off.vec, dax);
                            mesh.extraTriangles.Add(new[] {p1, p2, p3});
                        }

                        if (!closed) {
                            if (triAx && !triNax && !triDax) {
                                Vector3 p1 = getTempVertex(x, y, z, new Off3D(off.vec + dax.vec));
                                Vector3 p2 = getTempVertex(x, y, z, new Off3D(off.vec + nax.vec));
                                Vector3 p3 = getTempVertexOffset(x, y, z, off.vec, dax);
                                mesh.extraTriangles.Add(new[] {p1, p2, p3});
                            }

                            if (triNax && !triAx && !triDax) {
                                Vector3 p1 = getTempVertex(x, y, z, new Off3D(off.vec + dax.vec));
                                Vector3 p2 = getTempVertex(x, y, z, new Off3D(off.vec + ax.vec));
                                Vector3 p3 = getTempVertexOffset(x, y, z, off.vec, dax);
                                mesh.extraTriangles.Add(new[] {p1, p2, p3});
                            }
                        }
                    }

                    markSubBlock(x + off.x, y + off.y, z + off.z, new Off3D(dax.vec - off.vec));
                }
            }
        }
    }

    public Vector3 GetWorldPos(Vector3Int local, int x, int y, int z) {
        return new Vector3(x + local.x * 0.5f + 0.5f, y + local.y * 0.5f + 0.5f, z + local.z * 0.5f + 0.5f);
    }

    public Vector3 GetWorldPos(Vector3Int local, int x, int y, int z, float into) {
        return new Vector3(
            x + local.x * 0.5f * DebugController.vertexScale * into + 0.5f,
            y + local.y * 0.5f * DebugController.vertexScale * into + 0.5f,
            z + local.z * 0.5f * DebugController.vertexScale * into + 0.5f
        );
    }

    public GameObject Instance(GameObject prefab, Vector3 pos) {
        var obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.transform.SetParent(owner, true);
        return obj;
    } 
    
    public GameObject Instance(GameObject prefab, Vector3 from, Vector3 to) {
        var obj = Instantiate(prefab, from, Quaternion.identity);
        obj.transform.SetParent(owner, true);
        var line = obj.GetComponent<LineRenderer>();
        line.SetPosition(0, from);
        line.SetPosition(1, to);
        return obj;
    } 
    
    public GameObject InstanceGrid(GameObject prefab, Vector3 pos) {
        var obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.transform.SetParent(gridOwner, true);
        return obj;
    } 
}