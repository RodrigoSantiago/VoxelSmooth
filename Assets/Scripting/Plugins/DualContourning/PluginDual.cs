using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginDual : MonoBehaviour, PluginGenerator {
public int width;
     public int height;
     public int length;
     public int brushSize;
     public bool showCubes;
     private bool pshowCubes;
     public ChunkOwner ChunkOwner;
     public GameObject prefabPluginBtn;
     public GameObject prefabCube;
     public Chunk chunk;
     
     private GameObject selectedPlugin;
     void Start() {
         chunk = ChunkOwner.chunk;//new Chunk(width, height, length);
         
         for (int x = 0; x < width; x++) {
             for (int y = 0; y < height; y++) {
                 for (int z = 0; z < length; z++) {
                     var obj = Instantiate(prefabPluginBtn, new Vector3(x, y, z), Quaternion.identity);
                     var btn = obj.GetComponent<PluginButton>(); 
                     btn.plugin = this;
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
         chunk.Set(x, y, z, Mathf.Clamp(chunk.Get(x, y, z) + 250 * v, 0, 1000));
         
         Recalculate();
     }

     public void Recalculate() {
         var generator = new DualGenerator(width, height, length);
         var emitter = new MeshEmitter();
         generator.Generate(chunk, emitter);

         var mesh = emitter.Build();
         GetComponent<MeshFilter>().mesh = mesh;
         Debug.Log("Dual : " + mesh.vertexCount);

         var objects = GameObject.FindGameObjectsWithTag("Cube");
         foreach (var obj in objects) {
             Destroy(obj);
         }

         if (showCubes) {
             GetComponent<MeshRenderer>().enabled = false;
             for (int x = 0; x < width; x++) {
                 for (int y = 0; y < height; y++) {
                     for (int z = 0; z < length; z++) {
                         if (chunk.Get(x, y, z) != 0) {
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
