using Godot;
using System;

public partial class Level2 : Node2D
{
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
	
	// Return of the Solution found a 1:36AM because I have found nothing smart in the last 2 hours
	public bool IsLevel()
	{
		return true;
	}
}
