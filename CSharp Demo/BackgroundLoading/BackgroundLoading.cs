using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AlexOtsuka.Loading;

public static class BackgroundLoading
{
    private static HashSet<string> _requested = [];

    public static Error Request(string path, string typeHint = "", bool useSubThreads = false, ResourceLoader.CacheMode cacheMode = ResourceLoader.CacheMode.Reuse)
    {
        if (_requested.Contains(path))
            return Error.AlreadyExists;

        var error = ResourceLoader.LoadThreadedRequest(path, typeHint, useSubThreads, cacheMode);
        if (error != Error.Ok) { return error; }

        _requested.Add(path);
        return Error.Ok;
    }

    public static string[] Requested() => _requested.ToArray();

    public static Resource GetResource(string path)
    {
        if (ResourceLoader.LoadThreadedGetStatus(path) == ResourceLoader.ThreadLoadStatus.InvalidResource)
            return new Resource();

        _requested.Remove(path);
        return ResourceLoader.LoadThreadedGet(path);
    }

    public static T GetResource<T>(string path) where T : Resource => GetResource(path) as T;

    public static ResourceLoader.ThreadLoadStatus GetStatus(string path)
    {
        if (_requested.Contains(path))
            return ResourceLoader.LoadThreadedGetStatus(path);

        return ResourceLoader.ThreadLoadStatus.InvalidResource;
    }

    public static void Cancel(string path)
    {
        _requested.Remove(path);

        if (ResourceLoader.LoadThreadedGetStatus(path) == ResourceLoader.ThreadLoadStatus.Loaded)
        {
            ResourceLoader.LoadThreadedGet(path);
        }
        else if (ResourceLoader.LoadThreadedGetStatus(path) == ResourceLoader.ThreadLoadStatus.InProgress)
        {
            var thread = new Thread(() => ResourceLoader.LoadThreadedGet(path));
            thread.Start();
        }
    }

    public static void Clear()
    {
        foreach(string path in _requested)
            Cancel(path);

        _requested.Clear();
    }
}
