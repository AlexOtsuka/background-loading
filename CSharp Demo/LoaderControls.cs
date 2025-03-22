using Godot;

namespace AlexOtsuka;

public partial class LoaderControls : AspectRatioContainer
{
    [Export] private Label Cached {  get; set; }
    [Export] private Label Requested {  get; set; }
    [Export] private Label Instantiated {  get; set; }

    private void Update(string cached, string requested, string instantiated)
    {
        Cached.Text = cached;
        Requested.Text = requested;
        Instantiated.Text = instantiated;
    }
}