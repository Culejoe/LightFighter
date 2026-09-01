namespace LightFighter;

static class Program
{
    internal const string MutexName = @"Local\LightFighter.SingleInstance";
    internal const string ActivateEventName = @"Local\LightFighter.Activate";

    [STAThread]
    static void Main()
    {
        using var activate = new EventWaitHandle(false, EventResetMode.AutoReset, ActivateEventName);
        using var mutex = new Mutex(true, MutexName, out var createdNew);
        if (!createdNew)
        {
            activate.Set();
            return;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
