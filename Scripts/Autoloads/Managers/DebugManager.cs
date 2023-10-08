using Godot;
using System;
using System.Collections.Generic;

public class DebugStat
{
    public Godot.Object StatObject;
    public String Reference;
    public Boolean IsMethod;

    public DebugStat(Godot.Object statObject, String reference, Boolean isMethod)
    {
        StatObject = statObject;
        Reference = reference;
        IsMethod = isMethod;
    }
}

public class DebugManager : CanvasLayer
{
    public static Boolean IsDebugging = true;

    public static Dictionary<String, DebugStat> Stats = new Dictionary<String, DebugStat>();

    public static Label Label;

    public override void _Ready()
    {

        /* UIManager.Add("debug", Constants.Screens.DEBUG_OVERLAY); */

        DebugManager.Add("FPS", this, nameof(GetFPS), true);
        DebugManager.Add("Static memory", this, nameof(GetStaticMemoryUsage), true);
        DebugManager.Add("MouseMode", this, nameof(GetMouseMode), true);

        Label = GetNode<Label>("Label");
    }



    public override void _Process(float delta)
    {

        /*   if (Input.IsActionJustPressed("debug"))
          {
              IsDebugging = !IsDebugging;
              UIManager.GetUI("debug").GetNode<Control>("Container").Visible = !UIManager.GetUI("debug").GetNode<Control>("Container").Visible;
          } */

        // Update debugging values
        if (IsDebugging)
        {
            UpdateDebugValues();
        }

    }

    public static void UpdateDebugValues()
    {
        string labelText = "";
        foreach (KeyValuePair<string, DebugStat> stat in Stats)
        {

            object value;
            if (stat.Value?.StatObject != null && WeakRef(stat.Value.StatObject)?.GetRef() != null)
            {
                if (stat.Value.IsMethod)
                {
                    value = stat.Value?.StatObject?.Call(stat.Value.Reference);
                }
                else
                {
                    value = stat.Value?.StatObject?.Get(stat.Value.Reference);
                }

                labelText += stat.Key + ": " + value;
                labelText += "\n";
            }

        }
        Label.Text = labelText;
    }

    public static void Add(String name, Godot.Object statObject = null, String reference = null, Boolean isMethod = false)
    {
        Stats.Add(name, new DebugStat(statObject, reference, isMethod));
    }

    public static void Remove(string name)
    {
        Stats.Remove(name);
    }

    public String GetStaticMemoryUsage()
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = OS.GetStaticMemoryUsage();
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
        // show a single decimal place, and no space.
        return String.Format("{0:0.##} {1}", len, sizes[order]);
    }

    public float GetFPS()
    {
        return Engine.GetFramesPerSecond();
    }

    public string GetMouseMode()
    {
        return Input.MouseMode.ToString();
    }
}
