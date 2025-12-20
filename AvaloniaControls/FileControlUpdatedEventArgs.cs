namespace AvaloniaControls;

public class FileControlUpdatedEventArgs(string? path)
{
    public string? Path { get; set; } = path;
}