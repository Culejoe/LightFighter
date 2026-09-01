namespace LightFighter;

internal static class GammaRampCalculator
{
    public static GammaRamp Build(int brightness, int contrast, double gamma)
    {
        var contrastFactor = contrast / 50.0;
        var brightnessOffset = (brightness - 50) / 100.0;

        var channel = new ushort[256];
        for (var i = 0; i < 256; i++)
        {
            var normalized = i / 255.0;
            var gammaCorrected = Math.Pow(normalized, 1.0 / gamma);
            var value = (gammaCorrected - 0.5) * contrastFactor + 0.5 + brightnessOffset;
            value = Math.Clamp(value, 0.0, 1.0);
            channel[i] = (ushort)Math.Round(value * 65535);
        }

        return new GammaRamp
        {
            Red = (ushort[])channel.Clone(),
            Green = (ushort[])channel.Clone(),
            Blue = (ushort[])channel.Clone()
        };
    }
}
