using Godot;
using System;
using Godot.Collections;

namespace DoomSurvivor.Scripts.GameLoop;

public partial class HUDManager : Node
{
    public SceneTree mainLoop = CustomMainLoop.GetInstance();
    
    private CanvasLayer _canvasLayer;
    private Node _hud;
    
    public HUDManager()
    {
        _canvasLayer = new CanvasLayer();
        this.AddChild(_canvasLayer);
        
        // Load and add the HUD node
        var hudPacked = GD.Load<PackedScene>("res://Scenes/HUD/Hud.tscn");
        if (hudPacked != null)
        {
            _hud = hudPacked.Instantiate();
            _canvasLayer.AddChild(_hud);
        }
        else
        {
            GD.Print("Failed to load HUD scene");
        }
        
        GD.Print("HUDManager created");
    }
    
}