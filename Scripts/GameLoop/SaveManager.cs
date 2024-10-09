using Godot;
using System;
using Godot.Collections;

namespace DoomSurvivor.Scripts.GameLoop;

public partial class SaveManager : Node
{
    public SceneTree mainLoop = CustomMainLoop.GetInstance();
    public LevelManager levelManager = CustomMainLoop.GetInstance().GetLevelManager();
    
    private Array<Node> saveNodes;
    public SaveManager()
    {
        GD.Print("SaveManager created");
    }

    public void Save(String filePath)
    {
        using var saveFile = FileAccess.Open("user://" + filePath, FileAccess.ModeFlags.Write);

        var saveNodes = GetTree().GetNodesInGroup("Persist");
        foreach (Node saveNode in saveNodes)
        {
            // Check the node is an instanced scene so it can be instanced again during load.
            if (string.IsNullOrEmpty(saveNode.SceneFilePath))
            {
                GD.Print($"persistent node '{saveNode.Name}' is not an instanced scene, skipped");
                continue;
            }

            // Check the node has a save function.
            if (!saveNode.HasMethod("Save"))
            {
                GD.Print($"persistent node '{saveNode.Name}' is missing a Save() function, skipped");
                continue;
            }

            var nodeData = saveNode.Call("Save");
            var jsonString = Json.Stringify(nodeData);
            saveFile.StoreLine(jsonString);
        }
    }
    
    public void Load(String filePath)
    {
        if (!FileAccess.FileExists("user://" + filePath))
        {
            GD.PrintErr("Save file does not exist.");
            return;
        }

        using var saveFile = FileAccess.Open("user://" + filePath, FileAccess.ModeFlags.Read);

        var saveNodes = GetTree().GetNodesInGroup("Persist");
        foreach (Node saveNode in saveNodes)
        {
            saveNode.QueueFree();
        }

        while (saveFile.GetPosition() < saveFile.GetLength())
        {
            var jsonString = saveFile.GetLine();
            var json = new Json();
            var parseResult = json.Parse(jsonString);
            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                continue;
            }

            var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
            var newObjectScene = GD.Load<PackedScene>(nodeData["Filename"].ToString());
            if (newObjectScene == null)
            {
                GD.PrintErr($"Failed to load scene: {nodeData["Filename"]}");
                continue;
            }

            var newObject = newObjectScene.Instantiate<Node>();
            var parentNode = mainLoop.GetRoot().GetNode(nodeData["Parent"].ToString());
            if (parentNode == null)
            {
                GD.PrintErr($"Parent node not found: {nodeData["Parent"]}");
                continue;
            }

            parentNode.AddChild(newObject);

            var playerNode = newObject.GetNodeOrNull<CharacterBody2D>("Player_8_axis");
            if (playerNode != null)
            {
                playerNode.Position = new Vector2((float)nodeData["PlayerPosX"], (float)nodeData["PlayerPosY"]);
                GD.Print($"Player position loaded: {playerNode.Position}");
            }
            else if ((bool)newObject.Call("IsLevel"))
            {
                levelManager.LoadLevel(nodeData["Filename"].ToString());
            }
            else
            {
                newObject.Set(Node2D.PropertyName.Position, new Vector2((float)nodeData["PosX"], (float)nodeData["PosY"]));
            }

            foreach (var (key, value) in nodeData)
            {
                if (key == "Filename" || key == "Parent" || key == "PosX" || key == "PosY" || key == "PlayerPosX" || key == "PlayerPosY")
                {
                    continue;
                }
                newObject.Set(key, value);
            }
        }
    }
}