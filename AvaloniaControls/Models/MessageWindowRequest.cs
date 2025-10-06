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
    public string? LinkText { get; init; }
    public string? LinkUrl { get; set; }
    public bool DisplayLink => !string.IsNullOrEmpty(LinkText) && !string.IsNullOrEmpty(LinkUrl);
    public string? PrimaryButtonText { get; init; }
    public string? SecondaryButtonText { get; init; }
    public string? TertiaryButtonText { get; init; }
    public MessageWindowProgressBarType ProgressBar { get; init; }
    public bool DisplayProgressBar => ProgressBar != MessageWindowProgressBarType.None;
    public bool ProgressBarIndeterminate => ProgressBar == MessageWindowProgressBarType.Indeterminate;


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

    public bool IsSecondButtonVisible => Buttons is MessageWindowButtons.YesNo or MessageWindowButtons.OKCancel or MessageWindowButtons.YesNoCancel;

    public bool IsThirdButtonVisible => Buttons == MessageWindowButtons.YesNoCancel;
    
    public string FirstButtonText
    {
        get
        {
            if (!string.IsNullOrEmpty(PrimaryButtonText))
            {
                return PrimaryButtonText;
            }

            return Buttons switch
            {
                MessageWindowButtons.OK => "OK",
                MessageWindowButtons.YesNo => "Yes",
                MessageWindowButtons.YesNoCancel => "Yes",
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
            if (!string.IsNullOrEmpty(SecondaryButtonText))
            {
                return SecondaryButtonText;
            }

            return Buttons switch
            {
                MessageWindowButtons.YesNo => "No",
                MessageWindowButtons.YesNoCancel => "No",
                MessageWindowButtons.OKCancel => "Cancel",
                _ => ""
            };
        }
    }

    public string ThirdButtonText
    {
        get
        {
            if (!string.IsNullOrEmpty(TertiaryButtonText))
            {
                return TertiaryButtonText;
            }

            return Buttons switch
            {
                MessageWindowButtons.YesNoCancel => "Cancel",
                _ => ""
            };
        }
    }

}