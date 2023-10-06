using Godot;
using System;
using System.Diagnostics;

public class MinionSpawner : Spatial
{


    [Export]
    private PackedScene _minion;

    [Export]
    private PackedScene _cannonMinion;

    [Export]
    private TeamColor _assignedTeam;

    [Export]
    private NodePath _minionPath;

    [Export]
    private int _minionCount;

    private int _currentSpawnedMinions;

    [Export]
    private int _cannonMinionCount;

    private int _currentSpawnedCannonMinions;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public void OnInitialMinionSpawnTimerTimeout()
    {
        SpawnMinion();
        GetNode<Timer>("MinionWaveSpawnIntervalTimer").Start();
        GetNode<Timer>("MinionSpawnIntervalTimer").Start();
    }

    public void OnMinionWaveSpawnIntervalTimerTimeout()
    {
        SpawnMinion();
        GetNode<Timer>("MinionSpawnIntervalTimer").Start();
    }

    public void OnMinionSpawnIntervalTimerTimeout()
    {
        SpawnMinion();
    }

    private void SpawnMinion()
    {
        if (_minionCount == _currentSpawnedMinions && _cannonMinionCount == _currentSpawnedCannonMinions)
        {
            _currentSpawnedMinions = 0;
            _currentSpawnedCannonMinions = 0;
            GetNode<Timer>("MinionSpawnIntervalTimer").Stop();
            return;
        }

        if (_minionCount != _currentSpawnedMinions)
        {

            var newMinion = (Minion)_minion.Instance();
            newMinion.AssignedTeam = _assignedTeam;
            newMinion.CustomPathNodePath = _minionPath;

            GetParent().AddChild(newMinion);
            newMinion.GlobalTranslation = GlobalTranslation;
            _currentSpawnedMinions++;

            return;
        }

        if (_cannonMinionCount != _currentSpawnedCannonMinions)
        {

            var newCannonMinion = (CannonMinion)_cannonMinion.Instance();
            newCannonMinion.AssignedTeam = _assignedTeam;
            newCannonMinion.CustomPathNodePath = _minionPath;

            GetParent().AddChild(newCannonMinion);
            newCannonMinion.GlobalTranslation = GlobalTranslation;
            _currentSpawnedCannonMinions++;

            return;
        }
    }
}
