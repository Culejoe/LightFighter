namespace LightFighter;

public partial class MainForm : Form
{
    private const int DefaultBrightness = 50;
    private const int DefaultContrast = 50;
    private const double DefaultGamma = 1.0;

    private List<MonitorInfo> _monitors = new();
    private AppSettings _settings = new();
    private readonly ProcessTriggerWatcher _watcher = new();
    private bool _isExiting;
    private bool _isLoading;

    public MainForm()
    {
        InitializeComponent();
        ConfigureTheme();
        HandleCreated += MainForm_HandleCreated;
        Load += MainForm_Load;
        _watcher.TriggerStarted += trigger => BeginInvoke(() => OnTriggerStarted(trigger));
        _watcher.TriggerStopped += trigger => BeginInvoke(() => OnTriggerStopped(trigger));
    }

    private void ConfigureTheme()
    {
        DarkTheme.Apply(this);
        DarkTheme.ConfigureComboBox(cmbMonitors);
        DarkTheme.ConfigureComboBox(cmbSaveProfile);
        DarkTheme.ConfigureProfileAction(btnSaveCurrentProfile);
        DarkTheme.ConfigureListBox(lstProfiles);
        DarkTheme.ConfigureListBox(lstTriggers);
        DarkTheme.ConfigureButton(btnApply, primary: true);
        DarkTheme.ConfigureButton(btnReset);
        DarkTheme.ConfigureButton(btnLoadProfile);
        DarkTheme.ConfigureButton(btnApplyProfile, primary: true);
        DarkTheme.ConfigureButton(btnDeleteProfile, destructive: true);
        DarkTheme.ConfigureButton(btnSaveProfile);
        DarkTheme.ConfigureButton(btnAddTrigger, primary: true);
        DarkTheme.ConfigureButton(btnRemoveTrigger, destructive: true);
        lblBrightnessValue.ForeColor = DarkTheme.Accent;
        lblContrastValue.ForeColor = DarkTheme.Accent;
        lblStatus.ForeColor = DarkTheme.MutedText;
        lblTriggerStatus.ForeColor = DarkTheme.MutedText;
    }

    private void MainForm_HandleCreated(object? sender, EventArgs e)
    {
        WindowTheme.ApplyDarkTitleBar(this);
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        _isLoading = true;
        try
        {
            RefreshMonitors();
            SetSliderValues(DefaultBrightness, DefaultContrast, DefaultGamma);

            _settings = SettingsStore.Load();
            RefreshProfilesList();
            RefreshTriggersList();
            _watcher.UpdateTriggers(_settings.Triggers);
            chkEnableTriggers.Checked = _settings.TriggersEnabled;

            trayIcon.Visible = true;
            StartActivationListener();
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void StartActivationListener()
    {
        var thread = new Thread(() =>
        {
            using var activate = new EventWaitHandle(false, EventResetMode.AutoReset, Program.ActivateEventName);
            while (true)
            {
                activate.WaitOne();
                if (_isExiting || IsDisposed)
                {
                    break;
                }

                try
                {
                    BeginInvoke(RestoreWindow);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
        })
        {
            IsBackground = true
        };
        thread.Start();
    }

    private void RestoreWindow()
    {
        Show();
        WindowState = FormWindowState.Normal;
        Activate();
    }

    private void RefreshMonitors()
    {
        try
        {
            _monitors = MonitorService.GetMonitors();
        }
        catch (Exception ex)
        {
            SetStatus($"Failed to enumerate monitors: {ex.Message}", isError: true);
            return;
        }

        cmbMonitors.Items.Clear();
        foreach (var monitor in _monitors)
        {
            cmbMonitors.Items.Add(monitor);
        }

        if (cmbMonitors.Items.Count == 0)
        {
            SetStatus("No monitors detected.", isError: true);
            return;
        }

        var primaryIndex = _monitors.FindIndex(m => m.IsPrimary);
        cmbMonitors.SelectedIndex = primaryIndex >= 0 ? primaryIndex : 0;
    }

    private void SetSliderValues(int brightness, int contrast, double gamma)
    {
        trkBrightness.Value = Math.Clamp(brightness, trkBrightness.Minimum, trkBrightness.Maximum);
        trkContrast.Value = Math.Clamp(contrast, trkContrast.Minimum, trkContrast.Maximum);
        numGamma.Value = Math.Clamp((decimal)gamma, numGamma.Minimum, numGamma.Maximum);
        lblBrightnessValue.Text = $"{trkBrightness.Value}%";
        lblContrastValue.Text = $"{trkContrast.Value}%";
    }

    private void chkAllMonitors_CheckedChanged(object? sender, EventArgs e)
    {
        cmbMonitors.Enabled = !chkAllMonitors.Checked;
    }

    private void trkBrightness_ValueChanged(object? sender, EventArgs e) =>
        lblBrightnessValue.Text = $"{trkBrightness.Value}%";

    private void trkContrast_ValueChanged(object? sender, EventArgs e) =>
        lblContrastValue.Text = $"{trkContrast.Value}%";

    private void btnReset_Click(object? sender, EventArgs e)
    {
        SetSliderValues(DefaultBrightness, DefaultContrast, DefaultGamma);
        ApplyRamp(DefaultBrightness, DefaultContrast, DefaultGamma, GetSelectedAdjustTabTargets());
    }

    private void btnApply_Click(object? sender, EventArgs e)
    {
        ApplyRamp(trkBrightness.Value, trkContrast.Value, (double)numGamma.Value, GetSelectedAdjustTabTargets());
    }

    private void cmbSaveProfile_TextChanged(object? sender, EventArgs e)
    {
        btnSaveCurrentProfile.Text = _settings.Profiles.Any(profile =>
            string.Equals(profile.Name, cmbSaveProfile.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            ? "Update"
            : "Save";
    }

    private void btnSaveCurrentProfile_Click(object? sender, EventArgs e)
    {
        var name = cmbSaveProfile.Text.Trim();
        if (name.Length == 0)
        {
            MessageBox.Show(this, "Choose an existing profile or enter a name for a new one.", "Save Profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var existed = _settings.Profiles.Any(profile => string.Equals(profile.Name, name, StringComparison.OrdinalIgnoreCase));
        SaveCurrentSettingsToProfile(name);
        SetStatus(existed ? $"Updated profile '{name}'." : $"Created profile '{name}'.", isError: false);
    }

    private List<MonitorInfo> GetSelectedAdjustTabTargets() =>
        chkAllMonitors.Checked
            ? _monitors
            : (cmbMonitors.SelectedItem as MonitorInfo) is { } selected ? new List<MonitorInfo> { selected } : new List<MonitorInfo>();

    private static List<MonitorInfo> ResolveTargets(bool allMonitors, string? monitorKey, List<MonitorInfo> monitors)
    {
        if (allMonitors)
        {
            return monitors;
        }

        if (monitorKey is not null)
        {
            var match = monitors.FirstOrDefault(m => string.Equals(m.Key, monitorKey, StringComparison.OrdinalIgnoreCase));
            if (match is not null)
            {
                return new List<MonitorInfo> { match };
            }
        }

        var primary = monitors.FirstOrDefault(m => m.IsPrimary);
        return primary is not null ? new List<MonitorInfo> { primary } : new List<MonitorInfo>();
    }

    private void ApplyRamp(int brightness, int contrast, double gamma, List<MonitorInfo> targets)
    {
        if (targets.Count == 0)
        {
            SetStatus("No monitor selected.", isError: true);
            return;
        }

        var ramp = GammaRampCalculator.Build(brightness, contrast, gamma);
        var applied = new List<string>();
        var failed = new List<string>();

        foreach (var monitor in targets)
        {
            if (monitor.AdapterDeviceName is null)
            {
                failed.Add($"{monitor.FriendlyName} (unresolved adapter)");
                continue;
            }

            try
            {
                DisplayColorController.Apply(monitor.AdapterDeviceName, ramp);
                applied.Add(monitor.FriendlyName);
            }
            catch (Exception ex)
            {
                failed.Add($"{monitor.FriendlyName} ({ex.Message})");
            }
        }

        if (failed.Count == 0)
        {
            SetStatus($"Applied to: {string.Join(", ", applied)}", isError: false);
        }
        else
        {
            var summary = applied.Count > 0 ? $"Applied to: {string.Join(", ", applied)}. " : string.Empty;
            SetStatus($"{summary}Failed: {string.Join(", ", failed)}", isError: true);
        }
    }

    private void ApplyProfile(ColorProfile profile)
    {
        var monitors = MonitorService.GetMonitors();
        var targets = ResolveTargets(profile.AllMonitors, profile.MonitorKey, monitors);
        ApplyRamp(profile.Brightness, profile.Contrast, profile.Gamma, targets);
    }

    private void SetStatus(string message, bool isError)
    {
        lblStatus.Text = message;
        lblStatus.ForeColor = isError ? DarkTheme.Danger : DarkTheme.MutedText;
    }

    // --- Profiles tab ---

    private void RefreshProfilesList()
    {
        var selectedProfileName = cmbSaveProfile.Text;
        lstProfiles.Items.Clear();
        cmbSaveProfile.Items.Clear();
        foreach (var profile in _settings.Profiles)
        {
            lstProfiles.Items.Add(profile);
            cmbSaveProfile.Items.Add(profile.Name);
        }

        cmbSaveProfile.Text = selectedProfileName;
    }

    private void btnSaveProfile_Click(object? sender, EventArgs e)
    {
        var name = txtProfileName.Text.Trim();
        if (name.Length == 0)
        {
            MessageBox.Show(this, "Enter a name for the profile.", "Save Profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var existing = _settings.Profiles.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
        {
            var overwrite = MessageBox.Show(this, $"A profile named '{name}' already exists. Overwrite it?", "Save Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (overwrite != DialogResult.Yes)
            {
                return;
            }
        }

        SaveCurrentSettingsToProfile(name);
        txtProfileName.Clear();
        SetStatus($"Saved profile '{name}'.", isError: false);
    }

    private void SaveCurrentSettingsToProfile(string name)
    {
        var selectedMonitor = cmbMonitors.SelectedItem as MonitorInfo;
        var profile = _settings.Profiles.FirstOrDefault(existing =>
            string.Equals(existing.Name, name, StringComparison.OrdinalIgnoreCase));

        if (profile is null)
        {
            profile = new ColorProfile { Name = name };
            _settings.Profiles.Add(profile);
        }

        profile.Brightness = trkBrightness.Value;
        profile.Contrast = trkContrast.Value;
        profile.Gamma = (double)numGamma.Value;
        profile.AllMonitors = chkAllMonitors.Checked;
        profile.MonitorKey = chkAllMonitors.Checked ? null : selectedMonitor?.Key;

        SettingsStore.Save(_settings);
        RefreshProfilesList();
        cmbSaveProfile.Text = profile.Name;
    }

    private void btnLoadProfile_Click(object? sender, EventArgs e)
    {
        if (lstProfiles.SelectedItem is not ColorProfile profile)
        {
            MessageBox.Show(this, "Select a profile to load.", "Load Profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetSliderValues(profile.Brightness, profile.Contrast, profile.Gamma);
        chkAllMonitors.Checked = profile.AllMonitors;
        if (!profile.AllMonitors && profile.MonitorKey is not null)
        {
            var index = _monitors.FindIndex(m => string.Equals(m.Key, profile.MonitorKey, StringComparison.OrdinalIgnoreCase));
            if (index >= 0)
            {
                cmbMonitors.SelectedIndex = index;
            }
        }

        tabControl.SelectedTab = tabAdjust;
        SetStatus($"Loaded profile '{profile.Name}' into the Adjust tab.", isError: false);
    }

    private void btnApplyProfile_Click(object? sender, EventArgs e)
    {
        if (lstProfiles.SelectedItem is not ColorProfile profile)
        {
            MessageBox.Show(this, "Select a profile to apply.", "Apply Profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ApplyProfile(profile);
    }

    private void btnDeleteProfile_Click(object? sender, EventArgs e)
    {
        if (lstProfiles.SelectedItem is not ColorProfile profile)
        {
            MessageBox.Show(this, "Select a profile to delete.", "Delete Profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(this, $"Delete profile '{profile.Name}'? Any triggers using it will also be removed.", "Delete Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes)
        {
            return;
        }

        _settings.Profiles.Remove(profile);
        _settings.Triggers.RemoveAll(t => string.Equals(t.ProfileName, profile.Name, StringComparison.OrdinalIgnoreCase));
        SettingsStore.Save(_settings);
        RefreshProfilesList();
        RefreshTriggersList();
        _watcher.UpdateTriggers(_settings.Triggers);
        SetStatus($"Deleted profile '{profile.Name}'.", isError: false);
    }

    // --- Triggers tab ---

    private void RefreshTriggersList()
    {
        lstTriggers.Items.Clear();
        foreach (var trigger in _settings.Triggers)
        {
            lstTriggers.Items.Add(trigger);
        }

        UpdateTriggerStatusLabel();
    }

    private void UpdateTriggerStatusLabel()
    {
        lblTriggerStatus.Text = chkEnableTriggers.Checked
            ? $"Monitoring {_settings.Triggers.Count} trigger(s)."
            : "Background monitoring is disabled.";
    }

    private void btnAddTrigger_Click(object? sender, EventArgs e)
    {
        if (_settings.Profiles.Count == 0)
        {
            MessageBox.Show(this, "Create a profile in the Profiles tab first.", "Add Trigger", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dialog = new AddTriggerForm(_settings.Profiles.Select(p => p.Name));
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _settings.Triggers.RemoveAll(t => string.Equals(t.ProcessName, dialog.ProcessName, StringComparison.OrdinalIgnoreCase));
        _settings.Triggers.Add(new TriggerRule { ProcessName = dialog.ProcessName, ProfileName = dialog.SelectedProfileName! });
        SettingsStore.Save(_settings);
        RefreshTriggersList();
        _watcher.UpdateTriggers(_settings.Triggers);
    }

    private void btnRemoveTrigger_Click(object? sender, EventArgs e)
    {
        if (lstTriggers.SelectedItem is not TriggerRule trigger)
        {
            MessageBox.Show(this, "Select a trigger to remove.", "Remove Trigger", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _settings.Triggers.Remove(trigger);
        SettingsStore.Save(_settings);
        RefreshTriggersList();
        _watcher.UpdateTriggers(_settings.Triggers);
    }

    private void chkEnableTriggers_CheckedChanged(object? sender, EventArgs e)
    {
        _watcher.Enabled = chkEnableTriggers.Checked;
        _settings.TriggersEnabled = chkEnableTriggers.Checked;
        if (!_isLoading)
        {
            SettingsStore.Save(_settings);
        }

        UpdateTriggerStatusLabel();
    }

    private void OnTriggerStarted(TriggerRule trigger)
    {
        var profile = _settings.Profiles.FirstOrDefault(p => string.Equals(p.Name, trigger.ProfileName, StringComparison.OrdinalIgnoreCase));
        if (profile is null)
        {
            SetStatus($"'{trigger.ProcessName}' started but profile '{trigger.ProfileName}' no longer exists.", isError: true);
            return;
        }

        ApplyProfile(profile);
        trayIcon.ShowBalloonTip(2000, "LightFighter", $"'{trigger.ProcessName}' started - applied '{profile.Name}'.", ToolTipIcon.Info);
        SetStatus($"'{trigger.ProcessName}' started - applied profile '{profile.Name}'.", isError: false);
    }

    private void OnTriggerStopped(TriggerRule trigger)
    {
        var profile = _settings.Profiles.FirstOrDefault(p => string.Equals(p.Name, trigger.ProfileName, StringComparison.OrdinalIgnoreCase));
        var monitors = MonitorService.GetMonitors();
        var targets = profile is not null
            ? ResolveTargets(profile.AllMonitors, profile.MonitorKey, monitors)
            : ResolveTargets(false, null, monitors);

        ApplyRamp(DefaultBrightness, DefaultContrast, DefaultGamma, targets);
        trayIcon.ShowBalloonTip(2000, "LightFighter", $"'{trigger.ProcessName}' closed - reverted to default.", ToolTipIcon.Info);
        SetStatus($"'{trigger.ProcessName}' closed - reverted to default.", isError: false);
    }

    // --- Tray icon ---

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing && !_isExiting)
        {
            e.Cancel = true;
            Hide();
            return;
        }

        trayIcon.Visible = false;
        _watcher.Dispose();
        using var activate = new EventWaitHandle(false, EventResetMode.AutoReset, Program.ActivateEventName);
        activate.Set();
    }

    private void menuOpen_Click(object? sender, EventArgs e) => RestoreWindow();

    private void menuExit_Click(object? sender, EventArgs e)
    {
        _isExiting = true;
        Close();
    }
}

