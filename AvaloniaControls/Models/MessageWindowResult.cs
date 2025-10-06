namespace AvaloniaControls.Models;

public enum ButtonType
{
    None,
    Primary,
    Secondary,
    Tertiary
}

public class MessageWindowResult
{
    public ButtonType PressedButton { get; set; }
    public bool PressedAcceptButton => PressedButton == ButtonType.Primary;
    public bool CheckedBox { get; set; }
    public string? ResponseText { get; set; }
}