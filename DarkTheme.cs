namespace LightFighter;

internal static class DarkTheme
{
    public static readonly Color Background = Color.FromArgb(20, 23, 28);
    public static readonly Color Surface = Color.FromArgb(31, 36, 43);
    public static readonly Color SurfaceHover = Color.FromArgb(43, 49, 58);
    public static readonly Color Border = Color.FromArgb(68, 76, 88);
    public static readonly Color Text = Color.FromArgb(234, 238, 242);
    public static readonly Color MutedText = Color.FromArgb(160, 171, 184);
    public static readonly Color Accent = Color.FromArgb(236, 164, 68);
    public static readonly Color AccentHover = Color.FromArgb(249, 183, 86);
    public static readonly Color ProfileAction = Color.FromArgb(151, 94, 42);
    public static readonly Color ProfileActionHover = Color.FromArgb(181, 116, 51);
    public static readonly Color Danger = Color.FromArgb(213, 93, 81);

    public static void Apply(Form form)
    {
        form.BackColor = Background;
        form.ForeColor = Text;
        form.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        ApplyToChildren(form.Controls);
    }

    public static void ConfigureListBox(ListBox listBox)
    {
        listBox.BackColor = Surface;
        listBox.ForeColor = Text;
        listBox.BorderStyle = BorderStyle.FixedSingle;
        listBox.DrawMode = DrawMode.OwnerDrawFixed;
        listBox.ItemHeight = 28;
        listBox.DrawItem += DrawListItem;
    }

    public static void ConfigureComboBox(ComboBox comboBox)
    {
        comboBox.BackColor = Surface;
        comboBox.ForeColor = Text;
        comboBox.FlatStyle = FlatStyle.Flat;
        comboBox.DrawMode = DrawMode.OwnerDrawFixed;
        comboBox.DrawItem += DrawComboItem;
    }

    public static void ConfigureButton(Button button, bool primary = false, bool destructive = false)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = primary ? Accent : destructive ? Danger : Border;
        button.FlatAppearance.MouseOverBackColor = primary ? AccentHover : SurfaceHover;
        button.FlatAppearance.MouseDownBackColor = primary ? Color.FromArgb(207, 142, 53) : Surface;
        button.BackColor = primary ? Accent : Surface;
        button.ForeColor = primary ? Background : Text;
        button.UseVisualStyleBackColor = false;
        button.Cursor = Cursors.Hand;
    }

    public static void ConfigureProfileAction(Button button)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = ProfileAction;
        button.FlatAppearance.MouseOverBackColor = ProfileActionHover;
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(121, 75, 34);
        button.BackColor = ProfileAction;
        button.ForeColor = Text;
        button.UseVisualStyleBackColor = false;
        button.Cursor = Cursors.Hand;
    }

    private static void ApplyToChildren(Control.ControlCollection controls)
    {
        foreach (Control control in controls)
        {
            switch (control)
            {
                case TabPage tabPage:
                    tabPage.BackColor = Background;
                    tabPage.ForeColor = Text;
                    break;
                case Label label:
                    label.ForeColor = Text;
                    break;
                case CheckBox checkBox:
                    checkBox.ForeColor = Text;
                    checkBox.BackColor = Background;
                    break;
                case TextBox textBox:
                    textBox.BackColor = Surface;
                    textBox.ForeColor = Text;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case NumericUpDown numericUpDown:
                    numericUpDown.BackColor = Surface;
                    numericUpDown.ForeColor = Text;
                    break;
                case TrackBar trackBar:
                    trackBar.BackColor = Background;
                    break;
            }

            if (control.HasChildren)
            {
                ApplyToChildren(control.Controls);
            }
        }
    }

    private static void DrawListItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not ListBox listBox || e.Index < 0)
        {
            return;
        }

        var selected = (e.State & DrawItemState.Selected) != 0;
        using var background = new SolidBrush(selected ? SurfaceHover : Surface);
        using var textBrush = new SolidBrush(Text);
        e.Graphics.FillRectangle(background, e.Bounds);
        TextRenderer.DrawText(
            e.Graphics,
            listBox.GetItemText(listBox.Items[e.Index]),
            listBox.Font,
            Rectangle.Inflate(e.Bounds, -10, 0),
            textBrush.Color,
            TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private static void DrawComboItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not ComboBox comboBox || e.Index < 0)
        {
            return;
        }

        var selected = (e.State & DrawItemState.Selected) != 0;
        using var background = new SolidBrush(selected ? SurfaceHover : Surface);
        using var textBrush = new SolidBrush(Text);
        e.Graphics.FillRectangle(background, e.Bounds);
        TextRenderer.DrawText(
            e.Graphics,
            comboBox.GetItemText(comboBox.Items[e.Index]),
            comboBox.Font,
            Rectangle.Inflate(e.Bounds, -8, 0),
            textBrush.Color,
            TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
}
