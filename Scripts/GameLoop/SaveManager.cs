using Godot;
using System;
using Godot.Collections;

namespace DoomSurvivor.Scripts.GameLoop;

public partial class SaveManager : Node
{
    // private CustomMainLoop mainLoop = CustomMainLoop.Get();
    private Array<Node> saveNodes;
    public SaveManager()
    {
        GD.Print("SaveManager created");
    }

    private void Save(String filePath)
    {
        using var saveFile = FileAccess.Open("user://" + filePath, FileAccess.ModeFlags.Write);
        //saveNodes = mainLoop.GetRoot().GetTree().GetNodesInGroup("Persist");
        
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

            // Call the node's save function.
            var nodeData = saveNode.Call("Save");

            // Json provides a static method to serialized JSON string.
            var jsonString = Json.Stringify(nodeData);

            // Store the save dictionary as a new line in the save file.
            saveFile.StoreLine(jsonString);
        }
    }
    
    private void Load(String filePath)
    {
        using var saveFile = FileAccess.Open("user://" + filePath, FileAccess.ModeFlags.Write);
        
        if (!FileAccess.FileExists("user://savegame.save"))
         {
            return; // Error! We don't have a save to load.
         }

        // We need to revert the game state so we're not cloning objects during loading.
        // This will vary wildly depending on the needs of a project, so take care with
        // this step.
        // For our example, we will accomplish this by deleting saveable objects.
        //saveNodes = mainLoop.GetRoot().GetTree().GetNodesInGroup("Persist");
        foreach (Node saveNode in saveNodes)
        {
            saveNode.QueueFree();
        }

        while (saveFile.GetPosition() < saveFile.GetLength())
        {
            var jsonString = saveFile.GetLine();

            // Creates the helper class to interact with JSON
            var json = new Json();
            var parseResult = json.Parse(jsonString);
            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                continue;
            }

            // Get the data from the JSON object
            var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);

            // Firstly, we need to create the object and add it to the tree and set its position.
            var newObjectScene = GD.Load<PackedScene>(nodeData["Filename"].ToString());
            var newObject = newObjectScene.Instantiate<Node>();
            //mainLoop.GetRoot().GetNode(nodeData["Parent"].ToString()).AddChild(newObject);
            newObject.Set(Node2D.PropertyName.Position, new Vector2((float)nodeData["PosX"], (float)nodeData["PosY"]));

            // Now we set the remaining variables.
            foreach (var (key, value) in nodeData)
            {
                if (key == "Filename" || key == "Parent" || key == "PosX" || key == "PosY")
                {
                    continue;
                }
                newObject.Set(key, value);
            }
        }
    }
}