using DoomSurvivor.Scripts.GameLoop;
using Godot;

[GlobalClass]
public partial class CustomMainLoop : SceneTree
{
    private static CustomMainLoop _instance;
    public LevelManager _levelManager;
    public SaveManager _saveManager;
    public HUDManager _hudManager;


    public override void _Initialize()
    {
        GD.Print("Initializing CustomMainLoop");
        _levelManager = new LevelManager();
        _saveManager = new SaveManager();
        _hudManager = new HUDManager();
        
        GetRoot().AddChild(_levelManager);
        GetRoot().AddChild(_saveManager);
        GetRoot().AddChild(_hudManager);
        
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
