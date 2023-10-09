using Godot;
using System;

public class LoadingScreen : CanvasLayer
{
    private ResourceInteractiveLoader _resourceInteractiveLoader;

    [Signal]
    public delegate void ResourceLoaded(Resource resource);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_resourceInteractiveLoader == null) return;

        var error = _resourceInteractiveLoader.Poll();

        switch (error)
        {
            case Error.Ok:
                var progressBar = GetNode<ProgressBar>("ProgressBar");
                progressBar.Value = (float)_resourceInteractiveLoader.GetStage() / _resourceInteractiveLoader.GetStageCount() * 100;
                break;

            case Error.FileEof:
                EmitSignal(nameof(ResourceLoaded), _resourceInteractiveLoader.GetResource());
                _resourceInteractiveLoader = null;
                return;
        }
    }

    public void LoadResource(string resourcePath)
    {
        var loader = ResourceLoader.LoadInteractive(resourcePath);

        if (loader == null) return;

        _resourceInteractiveLoader = loader;
    }
}
