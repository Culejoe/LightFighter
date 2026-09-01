using System.Reflection;

namespace LightFighter;

internal static class AppIcon
{
    public static Icon Load()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LightFighter.app.ico")
            ?? throw new InvalidOperationException("The embedded LightFighter icon could not be found.");
        return new Icon(stream);
    }
}
