using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaControls.Services;

public interface ITaskService
{
    protected static ITaskService? Instance { get; set; }
    
    public Task RunTask(Action action);
    public Task RunTask(Action action, CancellationToken token);
    public Task RunTask(Func<Task?> function);
    public Task RunTask(Func<Task?> function, CancellationToken token);

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
    
    public static Task Run(Func<Task?> function)
    {
        if (Instance == null)
        {
            throw new InvalidOperationException();
        }

        return Instance.RunTask(function);
    }
    
    public static Task Run(Func<Task?> function, CancellationToken token)
    {
        if (Instance == null)
        {
            throw new InvalidOperationException();
        }

        return Instance.RunTask(function, token);
    }
}