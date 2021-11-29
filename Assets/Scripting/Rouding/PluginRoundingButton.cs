using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginRoundingButton : MonoBehaviour {

    public PluginRounding plugin;

    public Material matNormal;
    public Material matHover;
    private Vector3Int pos;
    private Renderer _renderer;
    
    void Start() {
        var p = transform.position;
        pos = new Vector3Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z));

        _renderer = GetComponentInChildren<Renderer>();
    }
    
    void OnMouseEnter() {
        _renderer.material = matHover;
    }
    
    void OnMouseExit() {
        _renderer.material = matNormal;
    }
    
    void OnMouseDown(){
        plugin.SetPoint(pos.x, pos.y, pos.z);
    }
}
