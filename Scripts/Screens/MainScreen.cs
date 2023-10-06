using Godot;
using System;

public class MainScreen : Node
{
    private CustomButton _playButton;

    private CustomButton _1v1GameModeButton;

    private CustomButton _3v3GameModeButton;

    private CustomButton _5v5GameModeButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _playButton = GetNode<CustomButton>("HBoxContainer/MainContainer/PlayButtonContainer/PlayButton");
        _1v1GameModeButton = GetNode<CustomButton>("HBoxContainer/MainContainer/GameModeContainer/1v1Container/CustomButtonTexture");
        _3v3GameModeButton = GetNode<CustomButton>("HBoxContainer/MainContainer/GameModeContainer/3v3Container/CustomButtonTexture");
        _5v5GameModeButton = GetNode<CustomButton>("HBoxContainer/MainContainer/GameModeContainer/5v5Container/CustomButtonTexture");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public void On1v1GameModeButtonToggled(bool isActivated)
    {
        if (_3v3GameModeButton.IsActivated) _3v3GameModeButton.SetIsActivated(!_3v3GameModeButton.IsActivated);
        if (_5v5GameModeButton.IsActivated) _5v5GameModeButton.SetIsActivated(!_5v5GameModeButton.IsActivated);

        if (_playButton.IsDisabled == isActivated)
        {
            _playButton.IsDisabled = !isActivated;
        }
    }

    public void On3v3GameModeButtonToggled(bool isActivated)
    {
        if (_1v1GameModeButton.IsActivated) _1v1GameModeButton.SetIsActivated(!_1v1GameModeButton.IsActivated);
        if (_5v5GameModeButton.IsActivated) _5v5GameModeButton.SetIsActivated(!_5v5GameModeButton.IsActivated);

        if (_playButton.IsDisabled == isActivated)
        {
            _playButton.IsDisabled = !isActivated;
        }

    }

    public void On5v5GameModeButtonToggled(bool isActivated)
    {
        if (_1v1GameModeButton.IsActivated) _1v1GameModeButton.SetIsActivated(!_1v1GameModeButton.IsActivated);
        if (_3v3GameModeButton.IsActivated) _3v3GameModeButton.SetIsActivated(!_3v3GameModeButton.IsActivated);

        if (_playButton.IsDisabled == isActivated)
        {
            _playButton.IsDisabled = !isActivated;
        }

    }

    public void OnPlayButtonPressed()
    {
        GD.Print("hi");
    }
}
