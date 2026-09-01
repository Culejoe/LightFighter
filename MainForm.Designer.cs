namespace LightFighter;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private DarkTabControl tabControl;
    private System.Windows.Forms.TabPage tabAdjust;
    private System.Windows.Forms.TabPage tabProfiles;
    private System.Windows.Forms.TabPage tabTriggers;

    private System.Windows.Forms.Label lblMonitor;
    private System.Windows.Forms.ComboBox cmbMonitors;
    private System.Windows.Forms.CheckBox chkAllMonitors;
    private System.Windows.Forms.Label lblBrightness;
    private System.Windows.Forms.TrackBar trkBrightness;
    private System.Windows.Forms.Label lblBrightnessValue;
    private System.Windows.Forms.Label lblContrast;
    private System.Windows.Forms.TrackBar trkContrast;
    private System.Windows.Forms.Label lblContrastValue;
    private System.Windows.Forms.Label lblGamma;
    private System.Windows.Forms.NumericUpDown numGamma;
    private System.Windows.Forms.Label lblSaveProfile;
    private System.Windows.Forms.ComboBox cmbSaveProfile;
    private System.Windows.Forms.Button btnApply;
    private System.Windows.Forms.Button btnReset;
    private System.Windows.Forms.Button btnSaveCurrentProfile;

    private System.Windows.Forms.ListBox lstProfiles;
    private System.Windows.Forms.TextBox txtProfileName;
    private System.Windows.Forms.Button btnSaveProfile;
    private System.Windows.Forms.Button btnLoadProfile;
    private System.Windows.Forms.Button btnApplyProfile;
    private System.Windows.Forms.Button btnDeleteProfile;

    private System.Windows.Forms.ListBox lstTriggers;
    private System.Windows.Forms.Button btnAddTrigger;
    private System.Windows.Forms.Button btnRemoveTrigger;
    private System.Windows.Forms.CheckBox chkEnableTriggers;
    private System.Windows.Forms.Label lblTriggerStatus;

    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.NotifyIcon trayIcon;
    private System.Windows.Forms.ContextMenuStrip trayMenu;
    private System.Windows.Forms.ToolStripMenuItem menuOpen;
    private System.Windows.Forms.ToolStripMenuItem menuExit;

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.tabControl = new DarkTabControl();
        this.tabAdjust = new TabPage();
        this.tabProfiles = new TabPage();
        this.tabTriggers = new TabPage();

        this.lblMonitor = new Label();
        this.cmbMonitors = new ComboBox();
        this.chkAllMonitors = new CheckBox();
        this.lblBrightness = new Label();
        this.trkBrightness = new TrackBar();
        this.lblBrightnessValue = new Label();
        this.lblContrast = new Label();
        this.trkContrast = new TrackBar();
        this.lblContrastValue = new Label();
        this.lblGamma = new Label();
        this.numGamma = new NumericUpDown();
        this.lblSaveProfile = new Label();
        this.cmbSaveProfile = new ComboBox();
        this.btnApply = new Button();
        this.btnReset = new Button();
        this.btnSaveCurrentProfile = new Button();

        this.lstProfiles = new ListBox();
        this.txtProfileName = new TextBox();
        this.btnSaveProfile = new Button();
        this.btnLoadProfile = new Button();
        this.btnApplyProfile = new Button();
        this.btnDeleteProfile = new Button();

        this.lstTriggers = new ListBox();
        this.btnAddTrigger = new Button();
        this.btnRemoveTrigger = new Button();
        this.chkEnableTriggers = new CheckBox();
        this.lblTriggerStatus = new Label();

        this.lblStatus = new Label();
        this.trayIcon = new NotifyIcon(this.components);
        this.trayMenu = new ContextMenuStrip(this.components);
        this.menuOpen = new ToolStripMenuItem();
        this.menuExit = new ToolStripMenuItem();

        this.tabControl.SuspendLayout();
        this.tabAdjust.SuspendLayout();
        this.tabProfiles.SuspendLayout();
        this.tabTriggers.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.trkBrightness)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.trkContrast)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numGamma)).BeginInit();
        this.trayMenu.SuspendLayout();
        this.SuspendLayout();

        // --- Adjust tab ---
        this.lblMonitor.AutoSize = true;
        this.lblMonitor.Location = new Point(16, 18);
        this.lblMonitor.Text = "Monitor:";

        this.cmbMonitors.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbMonitors.Location = new Point(100, 15);
        this.cmbMonitors.Size = new Size(280, 23);

        this.chkAllMonitors.AutoSize = true;
        this.chkAllMonitors.Location = new Point(100, 44);
        this.chkAllMonitors.Text = "Apply to all monitors";
        this.chkAllMonitors.CheckedChanged += new EventHandler(this.chkAllMonitors_CheckedChanged);

        this.lblBrightness.AutoSize = true;
        this.lblBrightness.Location = new Point(16, 82);
        this.lblBrightness.Text = "Brightness:";

        this.trkBrightness.Minimum = 0;
        this.trkBrightness.Maximum = 100;
        this.trkBrightness.TickFrequency = 10;
        this.trkBrightness.Location = new Point(100, 78);
        this.trkBrightness.Size = new Size(240, 45);
        this.trkBrightness.ValueChanged += new EventHandler(this.trkBrightness_ValueChanged);

        this.lblBrightnessValue.AutoSize = true;
        this.lblBrightnessValue.Location = new Point(346, 82);
        this.lblBrightnessValue.Size = new Size(34, 15);
        this.lblBrightnessValue.Text = "50%";

        this.lblContrast.AutoSize = true;
        this.lblContrast.Location = new Point(16, 130);
        this.lblContrast.Text = "Contrast:";

        this.trkContrast.Minimum = 0;
        this.trkContrast.Maximum = 100;
        this.trkContrast.TickFrequency = 10;
        this.trkContrast.Location = new Point(100, 126);
        this.trkContrast.Size = new Size(240, 45);
        this.trkContrast.ValueChanged += new EventHandler(this.trkContrast_ValueChanged);

        this.lblContrastValue.AutoSize = true;
        this.lblContrastValue.Location = new Point(346, 130);
        this.lblContrastValue.Size = new Size(34, 15);
        this.lblContrastValue.Text = "50%";

        this.lblGamma.AutoSize = true;
        this.lblGamma.Location = new Point(16, 180);
        this.lblGamma.Text = "Gamma:";

        this.numGamma.DecimalPlaces = 2;
        this.numGamma.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
        this.numGamma.Minimum = new decimal(new int[] { 30, 0, 0, 131072 });
        this.numGamma.Maximum = new decimal(new int[] { 280, 0, 0, 131072 });
        this.numGamma.Value = new decimal(new int[] { 100, 0, 0, 131072 });
        this.numGamma.Location = new Point(100, 178);
        this.numGamma.Size = new Size(80, 23);

        this.lblSaveProfile.AutoSize = true;
        this.lblSaveProfile.Location = new Point(16, 218);
        this.lblSaveProfile.Text = "Profile:";

        this.cmbSaveProfile.FormattingEnabled = true;
        this.cmbSaveProfile.Location = new Point(100, 214);
        this.cmbSaveProfile.Size = new Size(180, 23);
        this.cmbSaveProfile.TextChanged += new EventHandler(this.cmbSaveProfile_TextChanged);

        this.btnApply.Location = new Point(100, 254);
        this.btnApply.Size = new Size(110, 32);
        this.btnApply.Text = "Apply";
        this.btnApply.UseVisualStyleBackColor = true;
        this.btnApply.Click += new EventHandler(this.btnApply_Click);

        this.btnReset.Location = new Point(220, 254);
        this.btnReset.Size = new Size(160, 32);
        this.btnReset.Text = "Reset to Defaults";
        this.btnReset.UseVisualStyleBackColor = true;
        this.btnReset.Click += new EventHandler(this.btnReset_Click);

        this.btnSaveCurrentProfile.Location = new Point(290, 212);
        this.btnSaveCurrentProfile.Size = new Size(90, 30);
        this.btnSaveCurrentProfile.Text = "Save";
        this.btnSaveCurrentProfile.UseVisualStyleBackColor = true;
        this.btnSaveCurrentProfile.Click += new EventHandler(this.btnSaveCurrentProfile_Click);

        this.tabAdjust.Controls.Add(this.lblMonitor);
        this.tabAdjust.Controls.Add(this.cmbMonitors);
        this.tabAdjust.Controls.Add(this.chkAllMonitors);
        this.tabAdjust.Controls.Add(this.lblBrightness);
        this.tabAdjust.Controls.Add(this.trkBrightness);
        this.tabAdjust.Controls.Add(this.lblBrightnessValue);
        this.tabAdjust.Controls.Add(this.lblContrast);
        this.tabAdjust.Controls.Add(this.trkContrast);
        this.tabAdjust.Controls.Add(this.lblContrastValue);
        this.tabAdjust.Controls.Add(this.lblGamma);
        this.tabAdjust.Controls.Add(this.numGamma);
        this.tabAdjust.Controls.Add(this.lblSaveProfile);
        this.tabAdjust.Controls.Add(this.cmbSaveProfile);
        this.tabAdjust.Controls.Add(this.btnApply);
        this.tabAdjust.Controls.Add(this.btnReset);
        this.tabAdjust.Controls.Add(this.btnSaveCurrentProfile);
        this.tabAdjust.Text = "Adjust";
        this.tabAdjust.UseVisualStyleBackColor = true;

        // --- Profiles tab ---
        this.lstProfiles.Location = new Point(16, 16);
        this.lstProfiles.Size = new Size(200, 184);

        this.btnLoadProfile.Location = new Point(224, 16);
        this.btnLoadProfile.Size = new Size(160, 30);
        this.btnLoadProfile.Text = "Load into Adjust Tab";
        this.btnLoadProfile.UseVisualStyleBackColor = true;
        this.btnLoadProfile.Click += new EventHandler(this.btnLoadProfile_Click);

        this.btnApplyProfile.Location = new Point(224, 50);
        this.btnApplyProfile.Size = new Size(160, 30);
        this.btnApplyProfile.Text = "Apply Profile Now";
        this.btnApplyProfile.UseVisualStyleBackColor = true;
        this.btnApplyProfile.Click += new EventHandler(this.btnApplyProfile_Click);

        this.btnDeleteProfile.Location = new Point(224, 84);
        this.btnDeleteProfile.Size = new Size(160, 30);
        this.btnDeleteProfile.Text = "Delete Profile";
        this.btnDeleteProfile.UseVisualStyleBackColor = true;
        this.btnDeleteProfile.Click += new EventHandler(this.btnDeleteProfile_Click);

        this.txtProfileName.Location = new Point(16, 210);
        this.txtProfileName.Size = new Size(200, 23);
        this.txtProfileName.PlaceholderText = "New profile name";

        this.btnSaveProfile.Location = new Point(224, 208);
        this.btnSaveProfile.Size = new Size(160, 30);
        this.btnSaveProfile.Text = "Save Adjust Tab As...";
        this.btnSaveProfile.UseVisualStyleBackColor = true;
        this.btnSaveProfile.Click += new EventHandler(this.btnSaveProfile_Click);

        this.tabProfiles.Controls.Add(this.lstProfiles);
        this.tabProfiles.Controls.Add(this.btnLoadProfile);
        this.tabProfiles.Controls.Add(this.btnApplyProfile);
        this.tabProfiles.Controls.Add(this.btnDeleteProfile);
        this.tabProfiles.Controls.Add(this.txtProfileName);
        this.tabProfiles.Controls.Add(this.btnSaveProfile);
        this.tabProfiles.Text = "Profiles";
        this.tabProfiles.UseVisualStyleBackColor = true;

        // --- Triggers tab ---
        this.lstTriggers.Location = new Point(16, 16);
        this.lstTriggers.Size = new Size(280, 184);

        this.btnAddTrigger.Location = new Point(304, 16);
        this.btnAddTrigger.Size = new Size(80, 30);
        this.btnAddTrigger.Text = "Add...";
        this.btnAddTrigger.UseVisualStyleBackColor = true;
        this.btnAddTrigger.Click += new EventHandler(this.btnAddTrigger_Click);

        this.btnRemoveTrigger.Location = new Point(304, 50);
        this.btnRemoveTrigger.Size = new Size(80, 30);
        this.btnRemoveTrigger.Text = "Remove";
        this.btnRemoveTrigger.UseVisualStyleBackColor = true;
        this.btnRemoveTrigger.Click += new EventHandler(this.btnRemoveTrigger_Click);

        this.chkEnableTriggers.AutoSize = true;
        this.chkEnableTriggers.Location = new Point(16, 210);
        this.chkEnableTriggers.Text = "Enable background monitoring (runs in tray)";
        this.chkEnableTriggers.CheckedChanged += new EventHandler(this.chkEnableTriggers_CheckedChanged);

        this.lblTriggerStatus.AutoSize = true;
        this.lblTriggerStatus.ForeColor = DarkTheme.MutedText;
        this.lblTriggerStatus.Location = new Point(16, 236);
        this.lblTriggerStatus.Size = new Size(368, 15);

        this.tabTriggers.Controls.Add(this.lstTriggers);
        this.tabTriggers.Controls.Add(this.btnAddTrigger);
        this.tabTriggers.Controls.Add(this.btnRemoveTrigger);
        this.tabTriggers.Controls.Add(this.chkEnableTriggers);
        this.tabTriggers.Controls.Add(this.lblTriggerStatus);
        this.tabTriggers.Text = "Triggers";
        this.tabTriggers.UseVisualStyleBackColor = true;

        // --- Tab control ---
        this.tabControl.Location = new Point(12, 12);
        this.tabControl.Size = new Size(410, 340);
        this.tabControl.Controls.Add(this.tabAdjust);
        this.tabControl.Controls.Add(this.tabProfiles);
        this.tabControl.Controls.Add(this.tabTriggers);

        // --- Status label ---
        this.lblStatus.AutoSize = true;
        this.lblStatus.ForeColor = DarkTheme.MutedText;
        this.lblStatus.Location = new Point(16, 360);
        this.lblStatus.Size = new Size(384, 15);
        this.lblStatus.Text = string.Empty;

        // --- Tray icon ---
        this.menuOpen.Text = "Open";
        this.menuOpen.Click += new EventHandler(this.menuOpen_Click);

        this.menuExit.Text = "Exit";
        this.menuExit.Click += new EventHandler(this.menuExit_Click);

        this.trayMenu.Items.Add(this.menuOpen);
        this.trayMenu.Items.Add(this.menuExit);

        this.trayIcon.Text = "LightFighter";
        this.trayIcon.Icon = AppIcon.Load();
        this.trayIcon.ContextMenuStrip = this.trayMenu;
        this.trayIcon.DoubleClick += new EventHandler(this.menuOpen_Click);

        // --- MainForm ---
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(434, 390);
        this.Icon = AppIcon.Load();
        this.Controls.Add(this.tabControl);
        this.Controls.Add(this.lblStatus);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Text = "LightFighter";
        this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);

        this.tabControl.ResumeLayout(false);
        this.tabAdjust.ResumeLayout(false);
        this.tabAdjust.PerformLayout();
        this.tabProfiles.ResumeLayout(false);
        this.tabProfiles.PerformLayout();
        this.tabTriggers.ResumeLayout(false);
        this.tabTriggers.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.trkBrightness)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.trkContrast)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numGamma)).EndInit();
        this.trayMenu.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}

