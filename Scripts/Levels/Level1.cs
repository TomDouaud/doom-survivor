using DoomSurvivor.Scripts.GameLoop;
using Godot;

namespace DoomSurvivor.Scripts.Levels;

public partial class Level1 : Node2D
{
	private LevelManager _levelManager = CustomMainLoop.GetInstance().GetLevelManager();
	
	public void OnLV2WarpBodyEntered(Node2D body)
	{
		// Use call_deferred to load the new level
		CallDeferred(nameof(DeferredLoadLevel));
	}

	private void DeferredLoadLevel()
	{
		_levelManager.LoadLevel("Level2.tscn");
	}
	
	
	public Godot.Collections.Dictionary<string, Variant> Save()
	{
		var player = GetNode<CharacterBody2D>("Player_8_axis");
		return new Godot.Collections.Dictionary<string, Variant>()
		{
			{ "Filename", SceneFilePath },
			{ "Parent", GetParent().GetPath() },
			{ "PosX", Position.X }, // Vector2 is not supported by JSON
			{ "PosY", Position.Y },
			{ "PlayerPosX", player.Position.X },
			{ "PlayerPosY", player.Position.Y },
		};
	}
	
	// Solution found a 1:36AM because I have found nothing smart in the last 2 hours
	public bool IsLevel()
	{
		return true;
	}
}