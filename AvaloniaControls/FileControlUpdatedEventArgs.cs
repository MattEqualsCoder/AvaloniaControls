namespace AvaloniaControls;

public class FileControlUpdatedEventArgs
{
    public string? Path { get; set; }

    public FileControlUpdatedEventArgs(string? path)
    {
        Path = path;
    }
}