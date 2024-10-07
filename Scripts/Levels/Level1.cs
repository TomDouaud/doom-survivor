using DoomSurvivor.Scripts.GameLoop;
using Godot;

namespace DoomSurvivor.Scripts.Levels;

public partial class Level1 : Node2D
{
	private LevelManager _levelManager = CustomMainLoop.GetInstance().GetLevelManager();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void OnLV2WarpBodyEntered(Node2D body)
	{
		// Use call_deferred to load the new level
		CallDeferred(nameof(DeferredLoadLevel));
	}

	private void DeferredLoadLevel()
	{
		_levelManager.LoadLevel("Level2.tscn");
	}
}