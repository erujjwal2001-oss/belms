using MudBlazor;

namespace BELMS.Frontend.Theme;

public static class AppTheme
{
    public static MudTheme LightTheme { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2563EB",          // Modern blue
            Secondary = "#7C3AED",        // Purple accent

            Background = "#F8FAFC",       // Soft white-blue background
            Surface = "#FFFFFF",          // Cards / panels

            AppbarBackground = "#FFFFFF",
            AppbarText = "#111827",

            DrawerBackground = "#FFFFFF",
            DrawerText = "#334155",

            TextPrimary = "#0F172A",
            TextSecondary = "#64748B",

            ActionDefault = "#475569",

            Divider = "#E2E8F0",
            TableLines = "#E2E8F0",
            LinesDefault = "#E2E8F0",

            Success = "#16A34A",          // Green
            Warning = "#F59E0B",          // Amber
            Error = "#DC2626",            // Red
            Info = "#0284C7"              // Sky blue
        },

        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "12px"
        }
    };

    public static MudTheme DarkTheme { get; } = new()
    {
        PaletteDark = new PaletteDark
        {
            Primary = "#2196F3",
            Secondary = "#6c757d",

            Background = "#121212",
            Surface = "#1a1a1a",

            AppbarBackground = "#1a1a1a",
            DrawerBackground = "#1a1a1a",
            DrawerText = "#e0e0e0",

            TextPrimary = "#f8f9fa",
            TextSecondary = "#adb5bd",

            ActionDefault = "#adb5bd",

            Divider = "#2d2d2d",
            TableLines = "#2d2d2d",
            LinesDefault = "#2d2d2d",

            Success = "#20c997",
            Warning = "#ffc107",
            Error = "#dc3545",
            Info = "#0dcaf0"
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "8px"
        }
    };
}