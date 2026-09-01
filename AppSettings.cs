namespace LightFighter;

internal sealed class AppSettings
{
    public List<ColorProfile> Profiles { get; set; } = new();
    public List<TriggerRule> Triggers { get; set; } = new();
    public bool TriggersEnabled { get; set; }
}
