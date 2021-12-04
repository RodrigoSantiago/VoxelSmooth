using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundingGenerator : MeshGenerator {
    
    private Chunk data;
    private MeshEmitter mesh;
    private int width, height, length;
    private Vector3[] tempVectors;
    private bool[] subBlock;
    
    public RoundingGenerator(int width, int height, int length, bool padding, bool ignorePositive) {
        tempVectors = new Vector3[(width * 2 + 1) * (height * 2 + 1) * (length * 2 + 1)];
        subBlock = new bool[(width + 2) * 2 * (height + 2) * 2 * (length + 2) * 2];
        this.width = width;
        this.height = height;
        this.length = length;
    }

    public override void Generate(Chunk chunk, MeshEmitter meshEmitter) {
        data = chunk;
        mesh = meshEmitter;
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < length; z++) {
                    if (!get(x, y, z)) continue;
                    
                    WriteInternalMesh(x, y, z);
                }
            }
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < length; z++) {
                    if (!get(x, y, z)) continue;
                    
                    EmitVertex(x, y, z);
                }
            }
        }

        data = null;
        mesh = null;
    }

    public void AddTriangle(int x, int y, int z, Vector3 p1, Vector3 p2, Vector3 p3, Off3D side) {
        Vector3 cp = new Vector3(x, y, z);
        p1 = p1 * 0.5f + cp;
        p2 = p2 * 0.5f + cp;
        p3 = p3 * 0.5f + cp;
        mesh.AddTriangle(p1, p2, p3);
        if (side.x == -1 || side.y == -1 || side.z == -1) {
            mesh.AddTriangle(p1, p2, p3);
        } else {
            mesh.AddTriangle(p3, p2, p1);
        }
    }

    public override Mesh Regenerate(Chunk chunk, Mesh mesh, int x, int y, int z, int width, int height, int length) {
        throw new System.NotImplementedException();
    }

    private bool get(int x, int y, int z) {
        return data.GetSmooth(x, y, z);
    }
    
    private bool getNear(int x, int y, int z, Vector3Int off) {
        return data.GetSmooth(x + off.x, y + off.y, z + off.z);
    }
    
    private bool getNear(int x, int y, int z, Off3D off) {
        return data.GetSmooth(x + off.x, y + off.y, z + off.z);
    }

    public void WriteInternalMesh(int x, int y, int z) {
        bool[] near = new bool[27];
        for (int i = 0; i < 27; i++) {
            near[i] = getNear(x, y, z, i);
        }

        for (int i = 0; i < 27; i++) {
            var off = new Off3D(i);
            var point = off.vec3;
            var roundPoint = point;

            if (near[i]) {
                // NEVER ROUND

            } else if (off.IsSide) {
                roundPoint = WriteInternalSide(near, off);

            } else if (off.IsCorner) {
                roundPoint = WriteInternalCorner(near, off);

            } else if (off.IsEdge) {
                roundPoint = WriteInternalEdge(near, off);

            }

            SetTempVertex(x, y, z, off, roundPoint);
        }
    }

    private Vector3 WriteInternalSide(bool[] near, Off3D off) {
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

    private Vector3 WriteInternalCorner(bool[] near, Off3D off) {
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

    private Vector3 WriteInternalEdge(bool[] near, Off3D off) {
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

    public void EmitVertex(int x, int y, int z) {
        bool blocky = true;
        bool[] near = new bool[27];
        for (int i = 0; i < 27; i++) {
            near[i] = getNear(x, y, z, i);
            if (near[i]) blocky = false;
        }
        //if (blocky) return;

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
                        EmitVertexTripleRamp(x, y, z, off, ax, nax);
                        
                    } else if (near[ax + off]) {
                        EmitVertexRamp(x, y, z, off, ax, nax, false);
                        
                    } else if (near[nax + off]) {
                        EmitVertexRamp(x, y, z, off, nax, ax, true);
                        
                    } else if (near[dax + off]) {
                        EmitVertexDiagonal(x, y, z, off, ax, nax);
                        
                    } else {
                        EmitVertexFloor(x, y, z, off, ax, nax);
                        
                    }

                    MarkSubBlock(x + off.x, y + off.y, z + off.z, dax - off);
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

    private void EmitVertexFloor(int x, int y, int z, Off3D off, Off3D ax, Off3D nax) {
        var rAx = ax.Opposite;
        var rNax = nax.Opposite;
        
        Vector3Int pos = new Vector3Int(x, y, z);
        Vector3 p1 = GetTempVertex(pos, off);
        Vector3 p2 = GetTempVertex(pos, off + ax);
        Vector3 p3 = GetTempVertex(pos, off + nax);
        Vector3 p4 = GetTempVertex(pos, off + ax + nax);

        if (IsSideRound(pos, off)) {
            p1 += off.vec3 * 0.3f;
        }
        if (IsCornerRound(pos, 0, off, ax)) {
            p2 += off.vec3 * 0.3f;
        }
        if (IsCornerRound(pos, 0, off, nax)) {
            p3 += off.vec3 * 0.3f;
        }
        AddTriangle(x, y, z, p1, p4, p2, off);
        AddTriangle(x, y, z, p1, p3, p4, off);
    }

    private void EmitVertexRamp(int x, int y, int z, Off3D off, Off3D ax, Off3D nax, bool inverse) {
        var rAx = ax.Opposite;
        var rNax = nax.Opposite;
        Vector3Int pos = new Vector3Int(x, y, z);
        Vector3 p1 = GetTempVertex(pos, off);
        Vector3 p2 = GetTempVertex(pos, off + nax);
        Vector3 p3 = GetTempVertexOffset(pos, (off + ax).vec, rAx);
        Vector3 p4 = GetTempVertexOffset(pos, (off + ax).vec, rAx + nax);
        Vector3 p5 = (p1 + p3) / 2;
        Vector3 p6 = (p2 + p4) / 2;

        bool roundP2 = IsCornerRound(pos, 0, off, nax);
        bool roundP4 = IsCornerRound(pos + ax.vec + off.vec, 0, rAx, nax);
        Vector3 pp1 = IsSideRound(pos, off) ? p1 + off.vec3 * 0.3f : p1;
        Vector3 pp2 = roundP2 ? p2 + off.vec3 * 0.3f : p2;
        Vector3 pp3 = IsSideRound(pos + ax.vec + off.vec, rAx) ? p3 + rAx.vec3 * 0.3f : p3;
        Vector3 pp4 = roundP4 ? p4 + rAx.vec3 * 0.3f : p4;

        var offG = inverse ? off.Opposite : off;
        AddTriangle(x, y, z, pp1, pp2, p6, offG);
        AddTriangle(x, y, z, pp1, p6, p5, offG);
        AddTriangle(x, y, z, p5, p6, pp3, offG);
        AddTriangle(x, y, z, p6, pp4, pp3, offG);
        
        // Closure
        if (!getNear(x, y, z, off + ax + nax) && !getNear(x, y, z, nax)) {
            if (getNear(x, y, z, ax + nax)) {
                if (!IsSubBlockMarked(x + off.x + nax.x, y + off.y + nax.y, z + off.z + nax.z, ax + rNax - off)) {
                    Vector3 p7 = GetTempVertexOffset(pos, (ax + nax).vec, off + rAx);
                    Vector3 p8 = (pp4 + p7) / 2; // ?que ? nao seria p4
                    Vector3 p9 = (pp2 + p7) / 2;
                    Vector3 p0 = (pp2 + p4 + p7) / 3;
                    AddTriangle(x, y, z, pp2, p0, p6, offG);
                    AddTriangle(x, y, z, p6, p0, pp4, offG);
                    AddTriangle(x, y, z, pp4, p0, p8, offG);
                    AddTriangle(x, y, z, p8, p0, p7, offG);
                    AddTriangle(x, y, z, p7, p0, p9, offG);
                    AddTriangle(x, y, z, p9, p0, pp2, offG);
                    MarkSubBlock(x + off.x + nax.x, y + off.y + nax.y, z + off.z + nax.z, ax + rNax - off);
                }
            } else {
                Vector3 p7 = GetTempVertex(pos, off + ax + nax);
                AddTriangle(x, y, z, pp2, p7, p6, offG);
                if (roundP2) {
                    AddTriangle(x, y, z, p2, p7, pp2, offG);
                }
                AddTriangle(x, y, z, p6, p7, pp4, offG);
                if (roundP4) {
                    AddTriangle(x, y, z, pp4, p7, p4, offG);
                }
            }
        }
    }
    
    private void EmitVertexTripleRamp(int x, int y, int z, Off3D off, Off3D ax, Off3D nax) {
        var rAx = ax.Opposite;
        var rNax = nax.Opposite;
        Vector3Int pos = new Vector3Int(x, y, z);
        Vector3 p1 = GetTempVertex(pos, off);
        Vector3 p2 = GetTempVertexOffset(pos, (off + ax).vec, rAx);
        Vector3 p3 = GetTempVertexOffset(pos, (off + nax).vec, rNax);
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
        
        AddTriangle(x, y, z, p1, p0, p12, off);
        AddTriangle(x, y, z, p12, p0, p2, off);
        AddTriangle(x, y, z, p2, p0, p23, off);
        AddTriangle(x, y, z, p23, p0, p3, off);
        AddTriangle(x, y, z, p3, p0, p31, off);
        AddTriangle(x, y, z, p31, p0, p1, off);
    }
    
    private void EmitVertexDiagonal(int x, int y, int z, Off3D off, Off3D ax, Off3D nax) {
        var rAx = ax.Opposite;
        var rNax = nax.Opposite;
        Vector3Int pos = new Vector3Int(x, y, z);
        Vector3 p1 = GetTempVertex(pos, off);
        Vector3 p2 = GetTempVertex(pos, off + ax);
        Vector3 p3 = GetTempVertex(pos, off + nax);
        Vector3 p4 = GetTempVertexOffset(pos, (off + ax + nax).vec, rAx + rNax);
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
        
        AddTriangle(x, y, z, p5, p6, p1, off);
        AddTriangle(x, y, z, p6, p2, p1, off);
        AddTriangle(x, y, z, p7, p5, p1, off);
        AddTriangle(x, y, z, p3, p7, p1, off);
        AddTriangle(x, y, z, p4, p6, p5, off);
        AddTriangle(x, y, z, p7, p4, p5, off);
    }
    
    private void MarkSubBlock(int px, int py, int pz, Off3D off) {
        int x = px * 2 + (off.x == -1 ? 0 : 1) + 1;
        int y = py * 2 + (off.y == -1 ? 0 : 1) + 1;
        int z = pz * 2 + (off.z == -1 ? 0 : 1) + 1;
        int index = x + y * ((width + 2) * 2) + z * ((width + 2) * 2) * ((height + 2) * 2);
        if (index >= 0 && index < subBlock.Length) {
            subBlock[index] = true;
        }
    }

    private bool IsSubBlockMarked(int px, int py, int pz, Off3D off) {
        int x = px * 2 + (off.x == -1 ? 0 : 1) + 1;
        int y = py * 2 + (off.y == -1 ? 0 : 1) + 1;
        int z = pz * 2 + (off.z == -1 ? 0 : 1) + 1;
        int index = x + y * ((width + 2) * 2) + z * ((width + 2) * 2) * ((height + 2) * 2);
        if (index >= 0 && index < subBlock.Length) {
            return subBlock[index];
        }

        return false;
    }
    
    private Vector3 GetTempVertexOffset(int px, int py, int pz, Vector3Int relative, Off3D off) {
        return GetTempVertex(px + relative.x, py + relative.y, pz + relative.z, off) + relative * 2;
    }
    
    private Vector3 GetTempVertexOffset(Vector3Int pos, Vector3Int relative, Off3D off) {
        return GetTempVertex(pos.x + relative.x, pos.y + relative.y, pos.z + relative.z, off) + relative * 2;
    }
    
    private Vector3 GetTempVertex(Vector3Int pos, Off3D off) {
        return GetTempVertex(pos.x, pos.y, pos.z, off);
    }

    private Vector3 GetTempVertex(int px, int py, int pz, Off3D off) {
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
    
    private void SetTempVertex(int px, int py, int pz, Off3D off, Vector3 value) {
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
}
