using System;
using Avalonia;
using Avalonia.Controls;

namespace AvaloniaControls.Controls;

public class ContextMenuButton : Button
{
    protected override Type StyleKeyOverride => typeof(Button);
    
    protected override void OnClick()
    {
        base.OnClick();
        
        StyledElement? currentControl = this;
        while (currentControl is not null)
        {
            if (currentControl is Control { ContextMenu: { } contextMenu })
            {
                contextMenu.Open();
            }
            currentControl = currentControl.Parent;
        }
    }
}