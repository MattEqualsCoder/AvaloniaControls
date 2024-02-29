using System;
using System.Threading;
using System.Threading.Tasks;
using AvaloniaControls.Controls;
using Microsoft.Extensions.Logging;

namespace AvaloniaControls.Services;

public class TaskService : ITaskService
{
   
    
    private ILogger<TaskService> _logger;
    
    public TaskService(ILogger<TaskService> logger)
    {
        _logger = logger;
        ITaskService.Instance = this;
    }
    
    public Task RunTask(Action action)
    {
        return Task.Factory.StartNew(action)
            .ContinueWith((t) =>
                {
                    ShowExceptionWindow(t.Exception);
                },
                TaskContinuationOptions.OnlyOnFaulted);
    }
    
    public Task RunTask(Action action, CancellationToken token)
    {
        return Task.Factory.StartNew(action, token)
            .ContinueWith((t) =>
                {
                    ShowExceptionWindow(t.Exception);
                },
                TaskContinuationOptions.OnlyOnFaulted);
    }

    private void ShowExceptionWindow(Exception? ex = null)
    {
        if (ex != null)
        {
            _logger.LogError(ex, "Task failed");
        }
        else
        {
            _logger.LogError("Task failed with unknown error)");
        }
        Avalonia.Threading.Dispatcher.UIThread.Invoke(() =>
        {
            var window = new ExceptionWindow();
            if (ExceptionWindow.ParentWindow == null)
            {
                window.Show();
            }
            else
            {
                window.ShowDialog(ExceptionWindow.ParentWindow);
            }
        });
    }
}