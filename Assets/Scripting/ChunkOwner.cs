using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkOwner : MonoBehaviour {
    
    public bool useStatic;
    public int width, height, length;
    [HideInInspector] public Chunk chunk;
    
    void Awake() {
        chunk = new Chunk(width, height, length);
       
        if (useStatic) {
            
            var objects = GameObject.FindGameObjectsWithTag("Point");
            foreach (var obj in objects) {
                var p = obj.transform.position;
                chunk.Set(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z), 1000);
            }
            chunk.Set(3, 3, 2, 750);
            chunk.Set(4, 3, 2, 750);
            //chunk.Set(3, 2, 2, 750);
            //chunk.Set(4, 2, 2, 750);
        } else {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    for (int z = 0; z < length; z++) {
                        float d = Mathf.Sqrt((x - width / 2f) * (x - width / 2f) +
                                             (y - height / 2f) * (y - height / 2f) +
                                             (z - length / 2f) * (z - length / 2f));
                        d = 1 - Mathf.Clamp01(d - (width * 0.4f));
                        
                        int v = (int) (d * 1000);
                        v = Mathf.RoundToInt(v / 143f) * 143;
                        chunk.Set(x, y, z, v);
                        //chunk.Set(x, y, z, Random.value < 0.25f ? 1000 : 0);
                    }
                }
            }
        }
    }
}
