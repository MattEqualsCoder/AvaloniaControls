using Material.Icons;

namespace AvaloniaControls.Models;

public class MessageWindowRequest
{
    public required string Message { get; init; } = "Unknown";
    public string? Title { get; init; }
    public MessageWindowIcon Icon { get; init; }
    public MessageWindowButtons Buttons { get; init; }
    public bool DisplayTextBox { get; init; }
    public string? CheckBoxText { get; init; }

    public string WindowTitle
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                return Title;
            }

            return Icon switch
            {
                MessageWindowIcon.Warning => "Warning",
                MessageWindowIcon.Error => "Error",
                MessageWindowIcon.Question => "Question",
                MessageWindowIcon.Info => "Info",
                _ => "Alert"
            };
        }
    }

    public MaterialIconKind MaterialIconKind
    {
        get
        {
            return Icon switch
            {
                MessageWindowIcon.Warning => MaterialIconKind.Alert,
                MessageWindowIcon.Error => MaterialIconKind.CloseCircle,
                MessageWindowIcon.Question => MaterialIconKind.HelpCircle,
                MessageWindowIcon.Info => MaterialIconKind.Info,
                _ => MaterialIconKind.Info
            };
        }
    }

    public bool IsIconVisible => Icon != MessageWindowIcon.None;

    public bool IsCheckBoxVisible => !string.IsNullOrWhiteSpace(CheckBoxText);
    
    public bool IsSecondButtonVisible => Buttons is MessageWindowButtons.YesNo or MessageWindowButtons.OKCancel;

    public string FirstButtonText
    {
        get
        {
            return Buttons switch
            {
                MessageWindowButtons.OK => "OK",
                MessageWindowButtons.YesNo => "Yes",
                MessageWindowButtons.OKCancel => "OK",
                MessageWindowButtons.Close => "Close",
                _ => "OK"
            };
        }
    }
    
    public string SecondButtonText
    {
        get
        {
            return Buttons switch
            {
                MessageWindowButtons.YesNo => "No",
                MessageWindowButtons.OKCancel => "Cancel",
                _ => ""
            };
        }
    }
}