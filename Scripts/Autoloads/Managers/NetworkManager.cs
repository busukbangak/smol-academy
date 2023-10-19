using Globals;
using Godot;
using Godot.Collections;
using System;

public class NetworkManager : Node
{
    public static NetworkManager Instance;

    public const string DEFAULT_IP = "127.0.0.1";
    public const int DEFAULT_PORT = 1337;

    public NetworkedMultiplayerENet Network = new NetworkedMultiplayerENet();

    public string SelectedIP;
    public int SelectedPort;

    public int LocalPlayerID = 0;

    [Sync]
    public Dictionary<int, Entity> Players = new Dictionary<int, Entity>();

    [Sync]
    public Entity PlayerData;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            QueueFree();
        }
        else
        {
            Instance = this;
        }

        GetTree().Connect("network_peer_connected", this, nameof(OnPlayerConnected));
        GetTree().Connect("network_peer_disconnected", this, nameof(OnPlayerDisconnected));
        GetTree().Connect("connection_failed", this, nameof(OnConnectionFailed));
        GetTree().Connect("server_disconnected", this, nameof(OnServerDisconnected));
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void ConnectToServer()
    {
        GetTree().Connect("connected_to_server", this, nameof(OnConnection));
        Network.CreateClient(DEFAULT_IP, DEFAULT_PORT);
        GetTree().NetworkPeer = Network;
    }

    public void OnConnection()
    {
        GD.Print("Connected to server");
    }

    public void OnPlayerConnected(int id)
    {
        GD.Print("Player: ", id, " Connected");
    }

    public void OnPlayerDisconnected(int id)
    {
        GD.Print("Player: ", id, " Disconnected");
    }

    public void OnConnectionFailed()
    {
        GD.Print("Failed to connect to server");
    }

    public void OnServerDisconnected()
    {
        GD.Print("Server disconnected");
    }
}
