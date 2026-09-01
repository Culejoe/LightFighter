namespace LightFighter;

internal sealed class ColorProfile
{
    public required string Name { get; set; }
    public int Brightness { get; set; } = 50;
    public int Contrast { get; set; } = 50;
    public double Gamma { get; set; } = 1.0;
    public bool AllMonitors { get; set; }

    // Null means "whichever monitor is primary at the time the profile is applied".
    public string? MonitorKey { get; set; }

    public override string ToString() => Name;
}
