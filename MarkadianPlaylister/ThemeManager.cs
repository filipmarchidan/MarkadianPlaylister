using System.Drawing;
using System.Windows.Forms;

namespace MarkadianPlaylister
{
    public static class ThemeManager
    {
        public static AppTheme CurrentTheme { get; private set; } = AppTheme.Dark;

        // Define your palette
        private static readonly Color LightBackColor = Color.White;
        private static readonly Color LightForeColor = Color.Black;
        private static readonly Color DarkBackColor = Color.FromArgb(32, 32, 32);
        private static readonly Color DarkForeColor = Color.WhiteSmoke;
        private static readonly Color DarkAccentColor = Color.FromArgb(45, 45, 45);

        public static void ApplyTheme(Control control)
        {
            if (control == null) return;

            Color backColor, foreColor;
            if (CurrentTheme == AppTheme.Dark)
            {
                backColor = DarkBackColor;
                foreColor = DarkForeColor;
            }
            else
            {
                backColor = LightBackColor;
                foreColor = LightForeColor;
            }

            // Apply to parent
            control.BackColor = backColor;
            control.ForeColor = foreColor;

            // Apply recursively to all children
            foreach (Control child in control.Controls)
                ApplyTheme(child);

            // Optional: special cases (e.g., panels, buttons, listviews)
            if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = foreColor;
                btn.FlatAppearance.MouseOverBackColor =
                    CurrentTheme == AppTheme.Dark ? DarkAccentColor : Color.LightGray;
            }
            else if (control is ListView lv)
            {
                lv.BackColor = backColor;
                lv.ForeColor = foreColor;
            }
            else if (control is TextBox tb)
            {
                tb.BackColor = CurrentTheme == AppTheme.Dark ? DarkAccentColor : Color.White;
                tb.ForeColor = foreColor;
                tb.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        public static void SetTheme(AppTheme theme)
        {
            CurrentTheme = theme;
        }
    }



}
