using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeshGenerator {
    
    public abstract void Generate(Chunk chunk, MeshEmitter meshEmitter);
    
    public abstract Mesh Regenerate(Chunk chunk, Mesh mesh, int x, int y, int z, int width, int height, int length);
}
