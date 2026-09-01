namespace LightFighter;

// ProfileName must match a ColorProfile.Name. On process exit, color always reverts to the
// built-in default (50/50/1.0) applied to that same profile's monitor target.
internal sealed class TriggerRule
{
    public required string ProcessName { get; set; }
    public required string ProfileName { get; set; }

    public override string ToString() => $"{ProcessName} -> {ProfileName}";
}
