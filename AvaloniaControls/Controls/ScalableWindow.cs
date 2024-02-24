using System;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvaloniaControls.Controls;

public class ScalableWindow : Window
{
    private static double _globalScaleFactor = 1;
    private static double _previousScaleFactor = 1;
    private static double _changeScaleFactor;

    public static double GlobalScaleFactor
    {
        get => _globalScaleFactor;
        set
        {
            _previousScaleFactor = _globalScaleFactor;
            _globalScaleFactor = value;
            _changeScaleFactor = _globalScaleFactor / _previousScaleFactor;

            if (Math.Abs(_previousScaleFactor - _globalScaleFactor) > .001)
            {
                GlobalScaleFactorChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }

    private static event EventHandler? GlobalScaleFactorChanged;

    private LayoutTransformControl? _layoutTransformControl;

    private double _defaultMinWidth;
    private double _defaultMinHeight;
    private double _defaultMaxWidth;
    private double _defaultMaxHeight;
    
    public ScalableWindow()
    {
        Loaded += (sender, args) =>
        {
            _layoutTransformControl = this.Find<LayoutTransformControl>("MainLayout");
            if (_layoutTransformControl == null)
            {
                return;
            }

            _defaultMinWidth = MinWidth;
            _defaultMinHeight = MinHeight;
            _defaultMaxWidth = MaxWidth;
            _defaultMaxHeight = MaxHeight;
            GlobalScaleFactorChanged += (_, _) =>
            {
                OnScaleChanged(false);
            };

            if (Math.Abs(GlobalScaleFactor - 1) > .001)
            {
                OnScaleChanged(true);
            }
        };

    }

    private void OnScaleChanged(bool init)
    {
        if (_layoutTransformControl == null) return;
        _layoutTransformControl.LayoutTransform = new ScaleTransform(_globalScaleFactor, _globalScaleFactor);
        UpdateWindowSize(init);
    }

    private void UpdateWindowSize(bool init)
    {
        if (this is not RestorableWindow || !init)
        {
            var changeScale = init ? _globalScaleFactor : _changeScaleFactor;
            Width *= changeScale;
            Height *= changeScale;    
        }
        
        if (_defaultMinWidth > 0)
        {
            MinWidth = _defaultMinWidth * GlobalScaleFactor;
        }
        if (_defaultMinHeight > 0)
        {
            MinHeight = _defaultMinHeight * GlobalScaleFactor;
        }
        if (_defaultMaxWidth > 0)
        {
            MaxWidth = _defaultMaxWidth * GlobalScaleFactor;
        }
        if (_defaultMaxHeight > 0)
        {
            MaxHeight = _defaultMaxHeight * GlobalScaleFactor;
        }
    }
}