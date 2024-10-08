using DoomSurvivor.Scripts.GameLoop;
using Godot;

[GlobalClass]
public partial class CustomMainLoop : SceneTree
{
    private static CustomMainLoop _instance;
    private LevelManager _levelManager;
    private SaveManager _saveManager;
    private CanvasLayer _canvasLayer;
    private Node _hud;

    public override void _Initialize()
    {
        GD.Print("Initializing CustomMainLoop");
        _levelManager = new LevelManager();
        _saveManager = new SaveManager();
        _canvasLayer = new CanvasLayer();
        GetRoot().AddChild(_levelManager);
        GetRoot().AddChild(_saveManager);
        GetRoot().AddChild(_canvasLayer);
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
        
        _levelManager.LoadLevel("Level1.tscn");
        GD.Print("CustomMainLoop initialized");
    }

    public CustomMainLoop()
    {
        _instance = this;
        GD.Print("CustomMainLoop instantiated");
    }

    public static CustomMainLoop GetInstance()
    {
        if (_instance == null)
        {
            _instance = new CustomMainLoop();
        }
        return _instance;
    }

    public LevelManager GetLevelManager()
    {
        return _levelManager;
    }

    public SaveManager GetSaveManager()
    {
        return _saveManager;
    }
}
