namespace LightFighter;

partial class AddTriggerForm
{
    private System.Windows.Forms.Label lblProcessName;
    private System.Windows.Forms.TextBox txtProcessName;
    private System.Windows.Forms.Label lblProcessHint;
    private System.Windows.Forms.Label lblProfile;
    private System.Windows.Forms.ComboBox cmbProfile;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;

    private void InitializeComponent()
    {
        this.lblProcessName = new Label();
        this.txtProcessName = new TextBox();
        this.lblProcessHint = new Label();
        this.lblProfile = new Label();
        this.cmbProfile = new ComboBox();
        this.btnOk = new Button();
        this.btnCancel = new Button();
        this.SuspendLayout();

        // lblProcessName
        this.lblProcessName.AutoSize = true;
        this.lblProcessName.Location = new Point(16, 18);
        this.lblProcessName.Text = "Process name:";

        // txtProcessName
        this.txtProcessName.Location = new Point(130, 15);
        this.txtProcessName.Size = new Size(200, 23);

        // lblProcessHint
        this.lblProcessHint.AutoSize = true;
        this.lblProcessHint.ForeColor = DarkTheme.MutedText;
        this.lblProcessHint.Location = new Point(130, 41);
        this.lblProcessHint.Text = "Without \".exe\", e.g. Firefox";

        // lblProfile
        this.lblProfile.AutoSize = true;
        this.lblProfile.Location = new Point(16, 76);
        this.lblProfile.Text = "Apply profile:";

        // cmbProfile
        this.cmbProfile.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbProfile.Location = new Point(130, 73);
        this.cmbProfile.Size = new Size(200, 23);

        // btnOk
        this.btnOk.DialogResult = DialogResult.None;
        this.btnOk.Location = new Point(130, 112);
        this.btnOk.Size = new Size(90, 30);
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        this.btnOk.Click += new EventHandler(this.btnOk_Click);

        // btnCancel
        this.btnCancel.DialogResult = DialogResult.Cancel;
        this.btnCancel.Location = new Point(240, 112);
        this.btnCancel.Size = new Size(90, 30);
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;

        // AddTriggerForm
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(350, 160);
        this.Controls.Add(this.lblProcessName);
        this.Controls.Add(this.txtProcessName);
        this.Controls.Add(this.lblProcessHint);
        this.Controls.Add(this.lblProfile);
        this.Controls.Add(this.cmbProfile);
        this.Controls.Add(this.btnOk);
        this.Controls.Add(this.btnCancel);
        this.CancelButton = this.btnCancel;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Add Trigger";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
