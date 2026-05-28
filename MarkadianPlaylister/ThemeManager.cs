
using System.Windows.Forms;

namespace MarkadianPlaylister
{
    /*
     * 
     * This object can change themes based on the enum AppTheme.
     */
    public static class ThemeManager
    {
        public static AppTheme CurrentTheme { get; private set; } = AppTheme.Dark;

        // Define your palette
        private static readonly Color LightBackColor = Color.White;
        private static readonly Color LightForeColor = Color.Black;
        private static readonly Color DarkBackColor = Color.FromArgb(32, 32, 32);
        private static readonly Color DarkForeColor = Color.WhiteSmoke;
        private static readonly Color DarkAccentColor = Color.FromArgb(45, 45, 45);

        // Keep list background fully black in dark mode
        private static readonly Color DarkListBackColor = Color.FromArgb(50, 50, 50);

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
                // Force a pure black background for ListView in dark theme to match design
                lv.BackColor = CurrentTheme == AppTheme.Dark ? DarkListBackColor : backColor;
                lv.ForeColor = foreColor;

                // Improve visual consistency for details view:
                lv.GridLines = false;
                lv.FullRowSelect = true;
                lv.BorderStyle = BorderStyle.None;
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

        // --- New helpers for drag/drop visuals ---

        /// <summary>
        /// Background color to use when a control is in drag-over state.
        /// </summary>
        public static Color GetDragOverColor()
        {
            return CurrentTheme == AppTheme.Dark ? DarkAccentColor : Color.FromArgb(230, 230, 230);
        }

        /// <summary>
        /// Default background for controls (honors list special-case).
        /// </summary>
        public static Color GetDefaultBackColor(Control control)
        {
            if (control is ListView)
                return CurrentTheme == AppTheme.Dark ? DarkListBackColor : LightBackColor;

            return CurrentTheme == AppTheme.Dark ? DarkBackColor : LightBackColor;
        }
    }
}