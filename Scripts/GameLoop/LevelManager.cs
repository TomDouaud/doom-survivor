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
    public void LoadLevel(String levelName)
    {
        var levelPacked = GD.Load<PackedScene>("res://Scenes/Levels/" + levelName);
        if (levelPacked == null)
        {
            GD.Print("Failed to load PackedScene: " + "res://Scenes/Levels/" + levelName);
            return;
        }
        mainLoop.UnloadCurrentScene();
        foreach (Node node in mainLoop.GetRoot().GetChildren())
        {
            if (node.HasMethod("IsLevel"))
            {
                node.QueueFree();
            }
        }
        mainLoop.ChangeSceneToPacked(levelPacked);
        
    }
}