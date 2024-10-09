using Godot;
using System;

namespace DoomSurvivor.Scripts.GameLoop;

public partial class LevelManager : Node
{
    public SceneTree mainLoop = CustomMainLoop.GetInstance();
    public LevelManager()
    {
        GD.Print("LevelManager created");
    }
    public void LoadLevel(String path)
    {
        var levelPacked = GD.Load<PackedScene>("res://Scenes/Levels/" + path);
        if (levelPacked == null)
        {
            GD.Print("Failed to load PackedScene: " + "res://Scenes/Levels/" + path);
            return;
        }
        mainLoop.ChangeSceneToPacked(levelPacked);
        
    }
}