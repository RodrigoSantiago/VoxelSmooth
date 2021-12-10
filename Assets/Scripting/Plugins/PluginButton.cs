using System;
using UnityEngine;

public class PluginButton : MonoBehaviour { 
    public PluginGenerator plugin;
    
    public Color color0;
    public Color color1;
    public Color color2;
    public Color color3;
    public Material mat01;
    public Material mat02;
    private Vector3Int pos;
    private Renderer _renderer;
    
    
    void Start() {
        var p = transform.position;
        pos = new Vector3Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z));

        _renderer = GetComponentInChildren<Renderer>();
    }

    private void Update() {
        if (plugin.GetPoint(pos.x, pos.y, pos.z) < 0) {
            _renderer.material.color = Color.Lerp(color0, color3, -plugin.GetPoint(pos.x, pos.y, pos.z) / 1000f * 2);
        } else if (plugin.GetPoint(pos.x, pos.y, pos.z) / 1000f > 0.5) {
            _renderer.material.color = Color.Lerp(color0, color2, (plugin.GetPoint(pos.x, pos.y, pos.z) / 1000f - 0.5f) * 2f);
        } else {
            _renderer.material.color = Color.Lerp(color0, color1, plugin.GetPoint(pos.x, pos.y, pos.z) / 1000f * 2);
        }
    }

    private void OnMouseEnter() {
        transform.GetChild(0).localScale = new Vector3(0.18f, 0.18f, 0.18f);
    }

    private void OnMouseExit() {
        transform.GetChild(0).localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    void OnMouseDown(){
        if (Input.GetKey(KeyCode.LeftControl)) {
            plugin.SetPoint(pos.x, pos.y, pos.z, -1);
        } else {
            plugin.SetPoint(pos.x, pos.y, pos.z, 1);
        }
    }
}