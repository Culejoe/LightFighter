using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LightFighter;

// Correlates Screen.AllScreens (adapter device names, e.g. \\.\DISPLAY2) with WMI's WmiMonitorID
// (friendly/manufacturer names) via the monitor's hardware ID, which both expose in different forms.
internal static partial class MonitorService
{
    private static readonly Regex DeviceIdPattern = new(@"^\\\\\?\\DISPLAY#([^#]+)#([^#]+)#", RegexOptions.Compiled);
    private static readonly Regex WmiInstancePattern = new(@"^DISPLAY\\([^\\]+)\\([^\\_]+)", RegexOptions.Compiled);

    public static List<MonitorInfo> GetMonitors()
    {
        var adapterMap = BuildAdapterMap();
        var monitors = new List<MonitorInfo>();

        using var searcher = new ManagementObjectSearcher(@"root\wmi", "SELECT * FROM WmiMonitorID");
        foreach (ManagementBaseObject wmiId in searcher.Get())
        {
            var friendlyName = ConvertCharArray(wmiId["UserFriendlyName"] as ushort[]);
            var manufacturer = ConvertCharArray(wmiId["ManufacturerName"] as ushort[]);
            var instanceName = (string)wmiId["InstanceName"];

            string? adapterName = null;
            var isPrimary = false;
            var match = WmiInstancePattern.Match(instanceName);
            string? key = null;
            if (match.Success)
            {
                key = $"{match.Groups[1].Value}\\{match.Groups[2].Value}".ToLowerInvariant();
                if (adapterMap.TryGetValue(key, out var adapter))
                {
                    adapterName = adapter.AdapterDeviceName;
                    isPrimary = adapter.IsPrimary;
                }
            }

            monitors.Add(new MonitorInfo
            {
                FriendlyName = string.IsNullOrEmpty(friendlyName) ? manufacturer : friendlyName,
                Manufacturer = manufacturer,
                AdapterDeviceName = adapterName,
                IsPrimary = isPrimary,
                Key = key
            });
        }

        return monitors;
    }

    private static Dictionary<string, (string AdapterDeviceName, bool IsPrimary)> BuildAdapterMap()
    {
        var map = new Dictionary<string, (string, bool)>();

        // Enumerating adapters via EnumDisplayDevices(NULL, ...) is unreliable on some systems/sessions,
        // so adapter device names come from Screen.AllScreens instead (backed by EnumDisplayMonitors).
        foreach (var screen in Screen.AllScreens)
        {
            var monitor = new DisplayDevice { cb = System.Runtime.InteropServices.Marshal.SizeOf<DisplayDevice>() };
            if (!NativeMethods.EnumDisplayDevices(screen.DeviceName, 0, ref monitor, NativeMethods.EDD_GET_DEVICE_INTERFACE_NAME))
            {
                continue;
            }

            var idMatch = DeviceIdPattern.Match(monitor.DeviceID);
            if (!idMatch.Success)
            {
                continue;
            }

            var key = $"{idMatch.Groups[1].Value}\\{idMatch.Groups[2].Value}".ToLowerInvariant();
            map[key] = (screen.DeviceName, screen.Primary);
        }

        return map;
    }

    private static string ConvertCharArray(ushort[]? codes)
    {
        if (codes is null || codes.Length == 0)
        {
            return string.Empty;
        }

        return new string(codes.Where(c => c != 0).Select(c => (char)c).ToArray());
    }
}
