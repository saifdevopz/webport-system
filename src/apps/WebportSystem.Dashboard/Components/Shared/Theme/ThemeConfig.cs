using MudBlazor;

namespace WebportSystem.Dashboard.Components.Shared.Theme;

public record ThemeItem(string Name, MudTheme Theme, string ColorHex);

public static class ThemeConfig
{
    static ThemeConfig()
    {
        CurrentTheme = Themes[0].Theme;
        CurrentThemeName = Themes[0].Name;
    }

    public static bool IsDarkMode { get; set; }
    public static MudTheme CurrentTheme { get; set; }
    public static string CurrentThemeName { get; set; }

    public static readonly List<ThemeItem> Themes =
    [
        new ThemeItem("Blue", new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary = Colors.Blue.Default,
                Secondary = Colors.Orange.Default,
                Tertiary = "#ffffff"
            },
            PaletteDark = new PaletteDark
            {
                Primary = Colors.Blue.Lighten1,
                Secondary = Colors.Orange.Lighten1,
            }
        }, "#2196F3"),

        new ThemeItem("Green", new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary = Colors.Green.Default,
                Secondary = Colors.Lime.Accent3,
            },
            PaletteDark = new PaletteDark
            {
                Primary = Colors.Green.Lighten1,
                Secondary = Colors.Lime.Lighten1,
            }
        }, "#4CAF50"),

        new ThemeItem("Purple", new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary = Colors.DeepPurple.Default,
                Secondary = Colors.Pink.Accent3,
            },
            PaletteDark = new PaletteDark
            {
                Primary = Colors.DeepPurple.Lighten1,
                Secondary = Colors.Pink.Lighten1,
            }
        }, "#673AB7")
    ];
}
