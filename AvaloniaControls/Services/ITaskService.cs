using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaControls.Services;

public interface ITaskService
{
    protected static TaskService? Instance { get; set; }
    
    public Task RunTask(Action action);
    public Task RunTask(Action action, CancellationToken token);

    public static Task Run(Action action)
    {
        if (Instance == null)
        {
            throw new InvalidOperationException();
        }

        return Instance.RunTask(action);
    }
    
    public static Task Run(Action action, CancellationToken token)
    {
        if (Instance == null)
        {
            throw new InvalidOperationException();
        }

        return Instance.RunTask(action, token);
    }
}