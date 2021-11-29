using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour {

    public static float vertexScale = 0.95f;
    public static float meshScale = 0.75f;
    public static bool showX;
    public static bool showY;
    public static bool showZ;
    public static bool blocks;
    
    public float vertex = 0.95f;
    public float mesh = 0.75f;
    public bool x, y, z;
    public bool blockOnly;

    private void Awake() {
        vertexScale = vertex;
        meshScale = mesh;
        showX = x;
        showY = y;
        showZ = z;
    }

    void Update() {
        vertexScale = vertex;
        meshScale = mesh;
        showX = x;
        showY = y;
        showZ = z;
        blocks = blockOnly;
    }
}
