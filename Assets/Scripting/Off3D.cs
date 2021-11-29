using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Off3D {

    private static Vector3Int[] mapVectors;
    private static int[] mapTypes;
    private static Off3D[] mapVerAxis;
    private static Off3D[] mapHorAxis;
    private static Off3D[] mapIVerAxis;
    private static Off3D[] mapIHorAxis;
    private static Off3D[] mapTangent;
    private static Off3D[] mapITangent;
    private static Off3D[] mapOpposite;
    
    private static Off3D[] mapCloserX;
    private static Off3D[] mapCloserY;
    private static Off3D[] mapCloserZ;
    private static Off3D[] mapCloserXY;
    private static Off3D[] mapCloserYZ;
    private static Off3D[] mapCloserZX;
    static Off3D() {
        
        mapVectors  = new Vector3Int[27];
        mapTypes  = new int[27];
        mapVerAxis  = new Off3D[27];
        mapIVerAxis  = new Off3D[27];
        mapHorAxis  = new Off3D[27];
        mapIHorAxis  = new Off3D[27];
        mapIHorAxis  = new Off3D[27];
        mapTangent  = new Off3D[27];
        mapITangent  = new Off3D[27];
        mapOpposite  = new Off3D[27];
        
        mapCloserX  = new Off3D[27];
        mapCloserY  = new Off3D[27];
        mapCloserZ  = new Off3D[27];
        mapCloserXY  = new Off3D[27];
        mapCloserYZ  = new Off3D[27];
        mapCloserZX  = new Off3D[27];

        for (int i = 0; i < 27; i++) {
            mapVectors[i] = MapVectors(i);
        }

        for (int i = 0; i < 27; i++) {
            mapTypes[i] = MapTypes(i);
            mapVerAxis[i] = MapVerAxis(i);
            mapIVerAxis[i] = MapIVerAxis(i);
            mapHorAxis[i] = MapHorAxis(i);
            mapIHorAxis[i] = MapIHorAxis(i);
            mapTangent[i] = MapTangent(i);
            mapITangent[i] = MapITangent(i);
            mapOpposite[i] = MapOpposite(i);
            
            mapCloserX[i] = MapCloserX(i);
            mapCloserY[i] = MapCloserY(i);
            mapCloserZ[i] = MapCloserZ(i);
            mapCloserXY[i] = MapCloserXY(i);
            mapCloserYZ[i] = MapCloserYZ(i);
            mapCloserZX[i] = MapCloserZX(i);
        }
    }
    
    private static int find(int x, int y, int z) {
        return (x + 1) + (y + 1) * 3 + (z + 1) * 9;
    }
    
    private static Vector3Int MapVectors(int id) {
        return new Vector3Int(id % 3 - 1, (id % 9) / 3 - 1, id / 9 - 1);
    }

    private static int MapTypes(int id) {
        var vec = mapVectors[id];
        
        return (vec.x != 0 ? 1 : 0) + (vec.y != 0 ? 1 : 0) + (vec.z != 0 ? 1 : 0);
    }

    private static int MapVerAxis(int id) {
        var vec = mapVectors[id];
        
        if (mapTypes[id] == 1) {
            if (vec.x != 0) return find(0, 0, vec.x);
            if (vec.y != 0) return find(vec.y, 0, 0);
            if (vec.z != 0) return find(0, vec.z, 0);
        } else if (mapTypes[id] == 2) {
            if (vec.x == 0) return find(0, 0, vec.z);
            if (vec.y == 0) return find(vec.x, 0, 0);
            if (vec.z == 0) return find(0, vec.y, 0);
        }
        
        return id;

    }

    private static int MapIVerAxis(int id) {
        var vec = mapVectors[id];
        
        if (mapTypes[id] == 1) {
            if (vec.x != 0) return find(0, 0, -vec.x);
            if (vec.y != 0) return find(-vec.y, 0, 0);
            if (vec.z != 0) return find(0, -vec.z, 0);
        } else if (mapTypes[id] == 2) {
            if (vec.x == 0) return find(0, 0, -vec.z);
            if (vec.y == 0) return find(-vec.x, 0, 0);
            if (vec.z == 0) return find(0, -vec.y, 0);
        }
        
        return id;

    }
    
    private static int MapHorAxis(int id) {
        var vec = mapVectors[id];
        
        if (mapTypes[id] == 1) {
            if (vec.x != 0) return find(0, vec.x, 0);
            if (vec.y != 0) return find(0, 0, vec.y);
            if (vec.z != 0) return find(vec.z, 0, 0);
        } else if (mapTypes[id] == 2) {
            if (vec.x == 0) return find(0, vec.y, 0);
            if (vec.y == 0) return find(0, 0, vec.z);
            if (vec.z == 0) return find(vec.x, 0, 0);
        }
        
        return id;

    }
    
    private static int MapIHorAxis(int id) {
        var vec = mapVectors[id];
        
        if (mapTypes[id] == 1) {
            if (vec.x != 0) return find(0, -vec.x, 0);
            if (vec.y != 0) return find(0, 0, -vec.y);
            if (vec.z != 0) return find(-vec.z, 0, 0);
        } else if (mapTypes[id] == 2) {
            if (vec.x == 0) return find(0, -vec.y, 0);
            if (vec.y == 0) return find(0, 0, -vec.z);
            if (vec.z == 0) return find(-vec.x, 0, 0);
        }
        
        return id;

    }
    
    private static Off3D MapTangent(int id) {
        var vec = mapVectors[id];
        
        if (vec.x == 0) return find(1, 0, 0);
        if (vec.y == 0) return find(0, 1, 0);
        if (vec.z == 0) return find(0, 0, 1);
        return id;
    }
    
    private static Off3D MapITangent(int id) {
        var vec = mapVectors[id];
        
        if (vec.x == 0) return find(-1, 0, 0);
        if (vec.y == 0) return find(0, -1, 0);
        if (vec.z == 0) return find(0, 0, -1);
        return id;
    }
    
    private static Off3D MapOpposite(int off) {
        var val = new Off3D(off);
        return find(-val.x, -val.y, -val.z);
    }
    
    private static Off3D MapCloserX(int off) {
        return new Off3D(((Off3D)off).x, 0, 0);
    }
    
    private static Off3D MapCloserY(int off) {
        return new Off3D(0, ((Off3D)off).y, 0);
    }
    
    private static Off3D MapCloserZ(int off) {
        return new Off3D(0, 0, ((Off3D)off).z);
    }
    
    private static Off3D MapCloserXY(int off) {
        Off3D val = off;
        return new Off3D(val.x, val.y, 0);
    }
    
    private static Off3D MapCloserYZ(int off) {
        Off3D val = off;
        return new Off3D(0, val.y, val.z);
    }
    
    private static Off3D MapCloserZX(int off) {
        Off3D val = off;
        return new Off3D(val.x, 0, val.z);
    }
    
    public Off3D VerAxis {
        get {
            return mapVerAxis[id];
        }
    }
    
    public Off3D IVerAxis {
        get {
            return mapIVerAxis[id];
        }
    }
    
    public Off3D HorAxis {
        get {
            return mapHorAxis[id];
        }
    }
    
    public Off3D IHorAxis {
        get {
            return mapIHorAxis[id];
        }
    }
    
    public Off3D CloserX {
        get {
            return mapCloserX[id];
        }
    }

    public Off3D CloserY {
        get {
            return mapCloserY[id];
        }
    }

    public Off3D CloserZ {
        get {
            return mapCloserZ[id];
        }
    }
    
    public Off3D CloserXY {
        get {
            return mapCloserXY[id];
        }
    }

    public Off3D CloserYZ {
        get {
            return mapCloserYZ[id];
        }
    }

    public Off3D CloserZX {
        get {
            return mapCloserZX[id];
        }
    }
    
    public Off3D Tangent {
        get {
            return mapTangent[id];
        }
    }

    public Off3D ITangent {
        get {
            return mapITangent[id];
        }
    }

    public Off3D Opposite {
        get {
            return mapOpposite[id];
        }
    }
    
    public bool IsCenter {
        get {
            return mapTypes[id] == 0;
        }
    }

    public bool IsSide {
        get {
            return mapTypes[id] == 1;
        }
    }
    
    public bool IsCorner {
        get {
            return mapTypes[id] == 2;
        }
    }

    public bool IsEdge {
        get {
            return mapTypes[id] == 3;
        }
    }

    public int x {
        get {
            return vec.x;
        }
    }
    
    public int y {
        get {
            return vec.y;
        }
    }
    
    public int z {
        get {
            return vec.z;
        }
    }

    public Vector3 vec3 {
        get {
            return vec;
        }
    }
    
    public readonly Vector3Int vec;
    public readonly int id;

    public Off3D(int id) {
        this.id = id;
        this.vec = mapVectors[id];
    }

    public Off3D(int x1, int y1, int z1) {
        x1 = x1 > 0 ? 2 : x1 == 0 ? 1 : 0;
        y1 = y1 > 0 ? 2 : y1 == 0 ? 1 : 0;
        z1 = z1 > 0 ? 2 : z1 == 0 ? 1 : 0;
        
        this.id = x1 + y1 * 3 + z1 * 9;
        this.vec = mapVectors[id];
    }
    
    public Off3D(Vector3 pos) {
        int x1 = Mathf.RoundToInt(pos.x);
        int y1 = Mathf.RoundToInt(pos.y);
        int z1 = Mathf.RoundToInt(pos.z);
        x1 = x1 > 0 ? 2 : x1 == 0 ? 1 : 0;
        y1 = y1 > 0 ? 2 : y1 == 0 ? 1 : 0;
        z1 = z1 > 0 ? 2 : z1 == 0 ? 1 : 0;
        
        this.id = x1 + y1 * 3 + z1 * 9;
        this.vec = mapVectors[id];
    }

    public static Off3D operator +(Off3D a, Off3D b) {
        return new Off3D(a.vec + b.vec);   
    }

    public static Off3D operator -(Off3D a, Off3D b) {
        return new Off3D(a.vec - b.vec);
    }
    
    public static implicit operator int(Off3D off) {
        return off.id;
    }

    public static implicit operator Off3D(int off) {
        return new Off3D(off);
    }

}