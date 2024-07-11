namespace AvaloniaControls.Models;

public class ComboBoxAndSearchItem(object? value, string? display = null, string? description = null)
{
    public object? Value => value;
    public string Display => display ?? (value?.ToString() ?? "");
    public string? Description => description;

    public bool HasDescription => !string.IsNullOrEmpty(Description);

    public override string ToString() => Display;
}