# Resource Background Loader for Godot

This is a class I wrote as a solution to Godot's current lack of a feature allowing discarding assets pre-loaded through `ResourceLoader.load_threaded_request()`.

While this is far from perfect and needs testing for various edge cases, I believe it to be the best approach to a common problem.

My `BackgroundLoading` class acts as a wrapper for the most commonly used `ResourceLoader` methods and provides extra, much-needed features.

## Features

Download and drop `background_loading.gd` (GDScript) or `BackgroundLoading.cs` (C#) anywhere in your project to instantly have access to the following functions:

### BackgroundLoading.request()

A drop-in replacement for `ResourceLoader.load_threaded_request()` that keeps track of currently requested resources.

### BackgroundLoading.requested()

A list of all currently requested resources.

### BackgroundLoading.get_resource()

A drop-in replacement for `ResourceLoader.load_threaded_get()`.

### BackgroundLoading.get_status()

A drop-in replacement for `ResourceLoader.load_threaded_get_status()`.

### BackgroundLoading.cancel()

A non-blocking way to unload a previously requested resource in a separate thread.

### BackgroundLoading.clear()

Wipes all previously requested resources from memory.

## C# Version

The C# version works the exact same way as in GDScript, using `CamelCase` syntax instead of `snake_case` like every native Godot method.
A few notable differences:

- Has better encapsulation and lives in its own namespace (you must use `using AlexOtsuka.Loading`).
- Features a generic version of `GetResource()`, `GetResource()<T>`, allowing for cleaner code with less manual type casting.
- Probably more robust in terms of thread safety.
- When using C#, you're relying on automatic Garbage Collection. This means that cleared resources may linger in the cache for longer than expected.
A quick remedy that ensures your unloaded resources are properly disposed of is to call `GC.Collect()`, as shown in the demo project.

## Included demos

You may download either the GDScript demo (GDScript only) or the CSharp demo (features both GDScript and C#) for a quick look at how to use every `BackgroundLoading` function.

## Additional notes

My script isn't able to track or unload resources loaded using `ResourceLoader.load_threaded_request()`.
For this reason, it is better not to use both classes simultaneously.

This is NOT production ready and should only be used as inspiration for your own implementation of a reliable background loading system that fits your needs.

The "Damaged Helmet" 3D model used in the demo is property of ctxwing under the CC license. (https://sketchfab.com/3d-models/damaged-helmet-a1de6f1e738d446da3d50a3eebffe883).

Feel free to contact me on Discord (alexotsuka).
