using Blazored.LocalStorage;
using MudBlazor;
using RecoverUnsoldAdmin.Utils;

namespace RecoverUnsoldAdmin.Services;

public class ThemeService
{
    private MudTheme _theme;
    private readonly ILocalStorageService _localStorage;
    public bool IsDarkMode { get; set; }

    private static readonly MudTheme DarkTheme = new()
    {
        Palette = new PaletteDark
        {
            AppbarBackground = "#08283A",
            AppbarText = "#FAFAFA",
            Primary = "#7278E0",
            TextPrimary = "#FAFAFA",
            Background = "#3B3B3F",
            TextSecondary = "#E2EEF6",
            DrawerBackground = "#08283A",
            DrawerText = "#FAFAFA",
            Surface = "#3B3B3F",
            ActionDefault = "#2F678C",
            ActionDisabled = "#0C1217",
            TextDisabled = "#B0B0B0"
        },
        Typography = new Typography
        {
            Default = new Default
            {
                FontFamily = new[] { "Open Sans" }
            }
        }
    };

    private static readonly MudTheme LightTheme = new()
    {
        Palette = new PaletteLight
        {
            AppbarBackground = "#05141D",
            AppbarText = "#FAFAFA",
            Primary = "#8A92FF",
            TextPrimary = "#0C1217",
            Background = "#F4FDFF",
            TextSecondary = "#0C1217",
            DrawerBackground = "#05141D",
            DrawerText = "#FAFAFA",
            Surface = "#FFFBFF",
            ActionDefault = "#0C1217",
            ActionDisabled = "#2F678C",
            TextDisabled = "#676767"
        },
        Typography = new Typography
        {
            Default = new Default
            {
                FontFamily = new[] { "Open Sans" }
            }
        }
    };

    public ThemeService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _theme = LightTheme;
    }

    public MudTheme Theme()
    {
        return _theme;
    }

    public async Task LoadTheme()
    {
        IsDarkMode = await _localStorage.GetItemAsync<bool>(StorageItemKeys.ThemeKey);
        _theme = IsDarkMode ? DarkTheme : LightTheme;
    }

    public async Task ToggleDarkMode(bool isDarkMode)
    {
        await _localStorage.SetItemAsync(StorageItemKeys.ThemeKey, isDarkMode);
        IsDarkMode = isDarkMode;
        _theme = IsDarkMode ? DarkTheme : LightTheme;
    }
}