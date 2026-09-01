using System.Runtime.InteropServices;

namespace LightFighter;

internal sealed class DarkTabControl : TabControl
{
    private const int WM_ERASEBKGND = 0x0014;
    private const int WM_PAINT = 0x000F;
    private const int TCM_ADJUSTRECT = 0x1328;

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(IntPtr hWnd, string? appName, string? idList);

    public DarkTabControl()
    {
        DrawMode = TabDrawMode.OwnerDrawFixed;
        SizeMode = TabSizeMode.Fixed;
        ItemSize = new Size(112, 36);
        BackColor = DarkTheme.Background;
        ForeColor = DarkTheme.Text;
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        SetWindowTheme(Handle, string.Empty, string.Empty);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= TabCount)
        {
            return;
        }

        var selected = (e.State & DrawItemState.Selected) != 0;
        using var background = new SolidBrush(selected ? DarkTheme.Surface : DarkTheme.Background);
        using var textBrush = new SolidBrush(selected ? DarkTheme.Text : DarkTheme.MutedText);
        using var accentBrush = new SolidBrush(DarkTheme.Accent);

        e.Graphics.FillRectangle(background, e.Bounds);
        if (selected)
        {
            e.Graphics.FillRectangle(accentBrush, e.Bounds.X, e.Bounds.Bottom - 3, e.Bounds.Width, 3);
        }

        TextRenderer.DrawText(
            e.Graphics,
            TabPages[e.Index].Text,
            Font,
            e.Bounds,
            textBrush.Color,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    protected override void WndProc(ref Message message)
    {
        if (message.Msg == TCM_ADJUSTRECT)
        {
            base.WndProc(ref message);
            return;
        }

        if (message.Msg == WM_ERASEBKGND)
        {
            using var graphics = Graphics.FromHdc(message.WParam);
            graphics.Clear(DarkTheme.Background);
            message.Result = (IntPtr)1;
            return;
        }

        base.WndProc(ref message);

        if (message.Msg == WM_PAINT)
        {
            using var graphics = CreateGraphics();
            using var brush = new SolidBrush(DarkTheme.Background);
            var content = DisplayRectangle;
            const int borderWidth = 2;

            graphics.FillRectangle(brush, 0, content.Y - borderWidth, ClientSize.Width, borderWidth);
            graphics.FillRectangle(brush, 0, content.Y, borderWidth, ClientSize.Height - content.Y);
            graphics.FillRectangle(brush, ClientSize.Width - borderWidth, content.Y, borderWidth, ClientSize.Height - content.Y);
            graphics.FillRectangle(brush, 0, ClientSize.Height - borderWidth, ClientSize.Width, borderWidth);

            if (TabCount > 0)
            {
                using var dividerPen = new Pen(DarkTheme.Border);
                var firstTab = GetTabRect(0);
                var lastTab = GetTabRect(TabCount - 1);
                graphics.DrawLine(dividerPen, firstTab.Left, content.Y - 1, lastTab.Right, content.Y - 1);
            }
        }
    }
}
