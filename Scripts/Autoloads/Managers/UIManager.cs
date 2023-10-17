using Godot;
using System;
using System.Collections.Generic;

public class UIManager : Node
{

    private static Node _ui;

    private static Dictionary<string, Node> _layerDict = new Dictionary<string, Node>();

    public override void _Ready()
    {
        _ui = new Node
        {
            Name = "UI"
        };

        GetTree().Root.CallDeferred("add_child", _ui);
    }

    public static Node Add(string id, string scenePath)
    {
        PackedScene newPackedScene = (PackedScene)GD.Load(scenePath);
        return Add(id, newPackedScene.Instance());
    }


    public static Node Add(string id, Node scene)
    {
        if (_layerDict.ContainsKey(id))
        {
            GD.PushWarning($"UIManager: {scene.Name} is Already added to the UI");
            return _layerDict[id];
        }
        _ui.AddChild(scene);
        _layerDict.Add(id, scene);
        return _layerDict[id];
    }

    public static void Remove(string id)
    {
        if (!_layerDict.ContainsKey(id))
        {
            GD.PushWarning($"UIManager: Cant Remove {id}, because it doesnt exist");
            return;
        }

        _layerDict[id].QueueFree();
        _layerDict.Remove(id);
    }

    public static Node GetUI(string id)
    {
        if (!_layerDict.ContainsKey(id))
        {
            GD.PushWarning($"UIManager: Cant Get {id}, because it doesnt exist");
            return null;
        }

        return _layerDict[id];
    }

}
