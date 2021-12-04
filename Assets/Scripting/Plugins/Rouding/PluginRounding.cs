using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PluginRounding : MonoBehaviour, PluginGenerator {

    public int width;
    public int height;
    public int length;
    public bool showCubes;
    private bool pshowCubes;
    public ChunkOwner chunkOwner;
    public GameObject prefabPluginBtn;
    public GameObject prefabCube;
    private Chunk chunk;
    private GameObject selectedPlugin;
    
    void Start() {
        chunk = chunkOwner.chunk;//new Chunk(width, height, length);
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < length; z++) {
                    var obj = Instantiate(prefabPluginBtn, new Vector3(x, y, z), Quaternion.identity);
                    obj.GetComponent<PluginButton>().plugin = this;
                    obj.transform.SetParent(transform, false);
                }
            }
        }

        Recalculate();
    }
    
    void Update() {
        if (pshowCubes != showCubes) {
            pshowCubes = showCubes;
            Recalculate();
        }
    }

    public int GetPoint(int x, int y, int z) {
        return chunk.Get(x, y, z);
    }

    public void SetPoint(int x, int y, int z, int v) {
        chunk.Set(x, y, z, Mathf.Clamp(chunk.Get(x, y, z) + 125 * v, 0, 1000));
        //chunk.Set(x, y, z, chunk.Get(x, y, z) == 1000 ? 0 : 1000);
        Recalculate();
    }

    public void Recalculate() {
        var generator = new RoundingGenerator2(width, height, length, false, false);
        var emitter = new MeshEmitter();
        generator.Generate(chunk, emitter);
        
        var mesh = emitter.Build();
        GetComponent<MeshFilter>().mesh = mesh;
        Debug.Log("Rounding : " + mesh.GetIndexCount(0) / 3);

        var objects = GameObject.FindGameObjectsWithTag("Cube");
        foreach (var obj in objects) {
            Destroy(obj);
        }
        
        if (showCubes) {
            GetComponent<MeshRenderer>().enabled = false;
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    for (int z = 0; z < length; z++) {
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
