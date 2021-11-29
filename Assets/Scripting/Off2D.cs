
using UnityEngine;

public struct Off2D {

    private static bool[] mapTypes = {
        true, false, true,
        false, false, false,
        true, false, true
    };

    private static Vector2Int[] mapVectors = {
        new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
        new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0),
        new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1)
    };
    
    private static Off2D[] mapHor = {
        1,
        2,
        5,
        0,
        5,
        8,
        3,
        6,
        7
    };

    private static Off2D[] mapIHor = {
        3,
        0,
        1,
        6,
        3,
        2,
        7,
        8,
        5
    };

    private static Off2D[] mapNextCorner = {
        0,
        5,
        2,
        1,
        4,
        7,
        6,
        3,
        8
    };

    private static Off2D[] mapPrevCorner = {
        0,
        3,
        2,
        7,
        4,
        1,
        6,
        5,
        8
    };

    public readonly int off;
    public Off2D(int off) {
        this.off = off;
    }
    
    public static implicit operator int(Off2D off) {
        return off.off;
    }

    public static implicit operator Off2D(int off) {
        return new Off2D(off);
    }

    public int x {
        get {
            return mapVectors[off].x;
        }
    }

    public int y {
        get {
            return mapVectors[off].y;
        }
    }

    public Vector2Int vec {
        get {
            return mapVectors[off];
        }
    }

    public bool IsCorner {
        get {
            return mapTypes[off] && off != 4;
        }
    }
    
    public bool IsSide {
        get {
            return !mapTypes[off] && off != 4;
        }
    }
    
    public Off2D Left {
        get {
            return mapIHor[off];
        }
    }
    
    public Off2D LeftDown {
        get {
            return mapPrevCorner[off];
        }
    }
    
    public Off2D Right {
        get {
            return mapHor[off];
        }
    }
    
    public Off2D RightDown {
        get {
            return mapNextCorner[off];
        }
    }
}