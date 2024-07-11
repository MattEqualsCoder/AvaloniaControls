using AvaloniaControls.Models;

namespace AvaloniaControls;

public class ComboBoxAndSearchValueChangedArgs(ComboBoxAndSearchItem item)
{
    public object? Value => item.Value;
    public string DisplayValue => item.Display;
    public ComboBoxAndSearchItem Item => item;
}