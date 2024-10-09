using Godot;
using System;
using DoomSurvivor.Scripts.GameLoop;

public partial class Hud : Node
{
	private SaveManager _saveManager = CustomMainLoop.GetInstance().GetSaveManager();

	public void _on_save_pressed()
	{
		GD.Print("Save button pressed");
		_saveManager.Save("savegame.save");
	}
	
	public void _on_load_pressed()
	{
		GD.Print("Load button pressed");
		_saveManager.Load("savegame.save");
	}
}
