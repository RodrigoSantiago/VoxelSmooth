using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PluginRounding : MonoBehaviour {

    public int width;
    public int height;
    public int length;
    public GameObject prefabPluginBtn;
    private Chunk chunk;
    private GameObject selectedPlugin;
    
    void Start() {
        chunk = new Chunk(width, height, length);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < length; z++) {
                    var obj = Instantiate(prefabPluginBtn, new Vector3(x, y, z), Quaternion.identity);
                    obj.GetComponent<PluginRoundingButton>().plugin = this;
                    obj.transform.SetParent(transform, true);
                    
                    chunk.Set(x, y, z, Random.value < 0.25f ? 1 : 0);
                }
            }
        }
        Recalculate();
    }
    
    void Update() {
        
    }

    public void SetPoint(int x, int y, int z) {
        chunk.Set(x, y, z, chunk.Get(x, y, z) == 1 ? 0 : 1);
        Recalculate();
    }

    public void Recalculate() {
        var generator = new RoundingGenerator(width, height, length, false, false);
        var emitter = new MeshEmitter();
        generator.Generate(chunk, emitter);
        
        var mesh = emitter.Build();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshFilter>().mesh = mesh;
        Debug.Log(mesh.vertexCount);
    }
}
