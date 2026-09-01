namespace LightFighter;

public partial class AddTriggerForm : Form
{
    public string ProcessName => StripExeExtension(txtProcessName.Text.Trim());

    public string? SelectedProfileName => cmbProfile.SelectedItem as string;

    public AddTriggerForm(IEnumerable<string> profileNames)
    {
        InitializeComponent();
        ConfigureTheme();
        cmbProfile.Items.AddRange(profileNames.ToArray());
        if (cmbProfile.Items.Count > 0)
        {
            cmbProfile.SelectedIndex = 0;
        }
    }

    private void ConfigureTheme()
    {
        DarkTheme.Apply(this);
        DarkTheme.ConfigureComboBox(cmbProfile);
        DarkTheme.ConfigureButton(btnOk, primary: true);
        DarkTheme.ConfigureButton(btnCancel);
    }

    private static string StripExeExtension(string name) =>
        name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ? name[..^4] : name;

    private void btnOk_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtProcessName.Text))
        {
            MessageBox.Show(this, "Enter a process name.", "Add Trigger", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (SelectedProfileName is null)
        {
            MessageBox.Show(this, "Select a profile to apply, or create one first in the Profiles tab.", "Add Trigger", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
