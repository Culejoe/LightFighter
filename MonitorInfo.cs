namespace LightFighter;

internal sealed class MonitorInfo
{
    public required string FriendlyName { get; init; }
    public required string Manufacturer { get; init; }
    public string? AdapterDeviceName { get; init; }
    public bool IsPrimary { get; init; }

    // Stable hardware identity (survives reboots/adapter-number changes) - used to persist which
    // physical monitor a profile/trigger targets, since AdapterDeviceName (\\.\DISPLAY#) can shift.
    public string? Key { get; init; }

    public override string ToString() =>
        IsPrimary ? $"{FriendlyName} ({Manufacturer}) - Primary" : $"{FriendlyName} ({Manufacturer})";
}
