using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaControls.Controls;

public class NumericUpDownNoScroll : NumericUpDown
{
    protected override Type StyleKeyOverride => typeof(NumericUpDown);

    protected override void OnLoaded(RoutedEventArgs e)
    {
        Spinned += (sender, args) =>
        {
            if (args.UsingMouseWheel)
            {
                args.Handled = true;
            }
        };
    }

    protected override void OnSpin(SpinEventArgs e)
    {
        if (e.UsingMouseWheel)
        {
            e.Handled = true;
        }
        else
        {
            base.OnSpin(e);
        }
    }
}