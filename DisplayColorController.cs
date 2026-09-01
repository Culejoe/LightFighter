namespace LightFighter;

internal static class DisplayColorController
{
    public static void Apply(string adapterDeviceName, GammaRamp ramp)
    {
        var hdc = NativeMethods.CreateDC("DISPLAY", adapterDeviceName, null, IntPtr.Zero);
        if (hdc == IntPtr.Zero)
        {
            throw new InvalidOperationException($"Could not create a device context for {adapterDeviceName}.");
        }

        try
        {
            if (!NativeMethods.SetDeviceGammaRamp(hdc, ref ramp))
            {
                throw new InvalidOperationException(
                    $"SetDeviceGammaRamp failed for {adapterDeviceName} (Windows rejects overly extreme ramps).");
            }
        }
        finally
        {
            NativeMethods.DeleteDC(hdc);
        }
    }
}
