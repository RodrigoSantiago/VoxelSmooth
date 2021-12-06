using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PluginRounding : MonoBehaviour, PluginGenerator {
    public enum Algor {
        ROUDING, MARCHING
    }

    public Algor algorithm = Algor.ROUDING;
    private Algor prevAlgorithm = Algor.ROUDING;
    public bool showLines;
    public bool showCubes;
    public bool rouding;
    public bool effector;
    private bool prevRouding;
    private bool prevEffector;
    private bool pshowCubes;
    public ChunkOwner chunkOwner;
    public GameObject prefabPluginBtn;
    public GameObject prefabCube;
    private Chunk chunk;
    private GameObject selectedPlugin;
    
    void Start() {
        chunk = chunkOwner.chunk;//new Chunk(width, height, length);

        if (showLines) {
            for (int x = 0; x < chunk.Width; x++) {
                for (int y = 0; y < chunk.Height; y++) {
                    for (int z = 0; z < chunk.Length; z++) {
                        var obj = Instantiate(prefabPluginBtn, new Vector3(x, y, z), Quaternion.identity);
                        obj.GetComponent<PluginButton>().plugin = this;
                        obj.transform.SetParent(transform, false);
                    }
                }
            }
        }

        Recalculate();
    }
    
    void Update() {
        if (pshowCubes != showCubes || prevAlgorithm != algorithm || prevRouding != rouding || effector != prevEffector) {
            pshowCubes = showCubes;
            prevAlgorithm = algorithm;
            prevRouding = rouding;
            prevEffector = effector;
            Recalculate();
        }
    }

    public int GetPoint(int x, int y, int z) {
        return chunk.Get(x, y, z);
    }

    public void SetPoint(int x, int y, int z, int v) {
        chunk.Set(x, y, z, Mathf.Clamp(chunk.Get(x, y, z) + 143 * v, 0, 1000));
        //chunk.Set(x, y, z, chunk.Get(x, y, z) == 1000 ? 0 : 1000);
        Recalculate();
    }

    public void Recalculate() {
        var emitter = new MeshEmitter(true);
        Mesh mesh = null; 
        if (algorithm == Algor.ROUDING) {
            var generator = new RoundingGenerator2(chunk.Width, chunk.Height, chunk.Length, rouding, effector);
            generator.Generate(chunk, emitter);
            
        } else if (algorithm == Algor.MARCHING) {
            var generator = new MarchingGenerator(chunk.Width, chunk.Height, chunk.Length);
            generator.Generate(chunk, emitter);
        }

        mesh = emitter.Build();
        GetComponent<MeshFilter>().mesh = mesh;
        Debug.Log("Rounding : " + mesh.GetIndexCount(0) / 3);

        var objects = GameObject.FindGameObjectsWithTag("Cube");
        foreach (var obj in objects) {
            Destroy(obj);
        }
        
        if (showCubes) {
            GetComponent<MeshRenderer>().enabled = false;
            for (int x = 0; x < chunk.Width; x++) {
                for (int y = 0; y < chunk.Height; y++) {
                    for (int z = 0; z < chunk.Length; z++) {
                        if (chunk.Get(x, y, z) > 0) {
                            var obj = Instantiate(prefabCube, new Vector3(x, y, z), Quaternion.identity);
                            obj.transform.SetParent(transform, true);
                        }
                    }
                }
            }
        } else {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
