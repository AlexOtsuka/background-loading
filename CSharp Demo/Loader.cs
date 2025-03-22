using Godot;
using AlexOtsuka.Loading;
using System;
using System.Collections.Generic;

namespace AlexOtsuka;

public partial class Loader : Node
{
    [Signal] public delegate void UpdateTextEventHandler(string cached, string requested, string instantiated);

    private List<string> _instantiated = [];
    private ulong _lastUpdateTime;

    private void RequestScene(string path) => BackgroundLoading.Request(path);

    private void CancelRequest(string path) => BackgroundLoading.Cancel(path);

    private void InstantiateScene(string path)
    {
        var scene = BackgroundLoading.GetResource<PackedScene>(path);
        if(scene != null)
        {
            AddChild(scene.Instantiate());
            _instantiated.Add(path);
        }
    }

    private void Clear()
    {
        foreach (var child in GetChildren())
            child.QueueFree();

        BackgroundLoading.Clear();
        _instantiated.Clear();
    }

    private void RunGarbageCollection() => GC.Collect();

    public override void _Process(double delta)
    {
        if (_lastUpdateTime + 100 > Time.GetTicksMsec())
            return;

        _lastUpdateTime = Time.GetTicksMsec();

        const string path1 = "res://scene_1.tscn";
        const string path2 = "res://scene_2.tscn";

        var instantiated = string.Join("  ", _instantiated);
        var requested = string.Join("  ", BackgroundLoading.Requested());
        var cached = (ResourceLoader.HasCached(path1) ? path1 : "") + " " + (ResourceLoader.HasCached(path2) ? path2 : "");
        if (instantiated == "") instantiated = "None";
        if (requested == "") requested = "None";
        if (cached == " ") cached = "None";

        EmitSignal(SignalName.UpdateText, cached, requested, instantiated);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_left"))
        {
            GD.Print(BackgroundLoading.Requested().Length);
        }
    }
}
