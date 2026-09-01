using System.Diagnostics;

namespace LightFighter;

// Polls for configured trigger processes starting/stopping (no admin rights or WMI eventing
// needed - GetProcessesByName is cheap enough for a handful of rules on a multi-second interval).
internal sealed class ProcessTriggerWatcher : IDisposable
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(2);

    private readonly System.Threading.Timer _timer;
    private readonly HashSet<string> _runningTriggerProcesses = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _lock = new();
    private List<TriggerRule> _triggers = new();

    public event Action<TriggerRule>? TriggerStarted;
    public event Action<TriggerRule>? TriggerStopped;

    public bool Enabled { get; set; }

    public ProcessTriggerWatcher()
    {
        _timer = new System.Threading.Timer(Poll, null, PollInterval, PollInterval);
    }

    public void UpdateTriggers(IEnumerable<TriggerRule> triggers)
    {
        lock (_lock)
        {
            _triggers = triggers.ToList();
        }
    }

    private void Poll(object? state)
    {
        if (!Enabled)
        {
            return;
        }

        List<TriggerRule> triggers;
        lock (_lock)
        {
            triggers = _triggers;
        }

        foreach (var trigger in triggers)
        {
            var processes = Process.GetProcessesByName(trigger.ProcessName);
            var isRunning = processes.Length > 0;
            foreach (var process in processes)
            {
                process.Dispose();
            }

            var wasRunning = _runningTriggerProcesses.Contains(trigger.ProcessName);
            if (isRunning && !wasRunning)
            {
                _runningTriggerProcesses.Add(trigger.ProcessName);
                TriggerStarted?.Invoke(trigger);
            }
            else if (!isRunning && wasRunning)
            {
                _runningTriggerProcesses.Remove(trigger.ProcessName);
                TriggerStopped?.Invoke(trigger);
            }
        }
    }

    public void Dispose() => _timer.Dispose();
}
