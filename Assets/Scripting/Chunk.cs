
using UnityEngine;

public class Chunk {

    public int[] blocks;
    private int width, height, length;

    public int Width {
        get {
            return width;
        }
    }

    public int Height {
        get {
            return height;
        }
    }

    public int Length {
        get {
            return length;
        }
    }

    public Chunk(int width, int height, int length) {
        this.width = width;
        this.height = height;
        this.length = length;
        blocks = new int[width * height * length];
    }
    
    public float GetFloat(int x, int y, int z) {
        if (x < 0 || y < 0 || z < 0 || x >= width || y >= height || z >= length) return 0;
        return blocks[x + y * width + z * width * height] / 1000f;
    }

    public int Get(int index) {
        return blocks[index];
    }
    
    public int Get(int x, int y, int z) {
        if (x < 0 || y < 0 || z < 0 || x >= width || y >= height || z >= length) return 0;
        return blocks[x + y * width + z * width * height];
    }
    
    public bool GetSmooth(int x, int y, int z) {
        return Get(x, y, z) >= 500;
    }
    
    public void Set(int x, int y, int z, int block) {
        blocks[x + y * width + z * width * height] = block;
    }
    
    public void Set(int x, int y, int z, int w, int h, int l, int[] block) {
        
    }
    
}