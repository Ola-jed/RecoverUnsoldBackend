using Blazored.LocalStorage;
using MudBlazor;

namespace RecoverUnsoldAdmin.Services;

public class ThemeService
{
    private const string ThemeKey = "dark-theme";
    private MudTheme _theme;
    private readonly ILocalStorageService _localStorage;
    public bool IsDarkMode { get; set; }

    private readonly MudTheme _darkTheme = new()
    {
        Palette = new Palette
        {
            AppbarBackground = "#7278E0",
            AppbarText = "#FFFFFF",
            Primary = "#7278E0",
            TextPrimary = "#FFFFFF",
            Background = "#3B3B3F",
            TextSecondary = "#E2EEF6",
            DrawerBackground = "#08283A",
            DrawerText = "#FAFAFA",
            Surface = "#3B3B3F",
            ActionDefault = "#2F678C",
            ActionDisabled = "#0C1217",
            TextDisabled = "#B0B0B0"
        }
    };

    private readonly MudTheme _lightTheme = new()
    {
        Palette = new Palette
        {
            AppbarBackground = "#121B9E",
            AppbarText = "#FFFFFF",
            Primary = "#121B9E",
            TextPrimary = "#0C1217",
            Background = "#F4FDFF",
            TextSecondary = "#0C1217",
            DrawerBackground = "#05141D",
            DrawerText = "#FAFAFA",
            Surface = "#FFFBFF",
            ActionDefault = "#0C1217",
            ActionDisabled = "#2F678C",
            TextDisabled = "#676767"
        }
    };
    
    public ThemeService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _theme = _lightTheme;
    }

    public MudTheme Theme()
    {
        return _theme;
    }

    public async Task LoadTheme()
    {
        IsDarkMode = await _localStorage.GetItemAsync<bool>(ThemeKey);
        _theme = IsDarkMode ? _darkTheme : _lightTheme;
    }
    
    public async Task ToggleDarkMode(bool isDarkMode)
    {
        await _localStorage.SetItemAsync(ThemeKey, isDarkMode);
        IsDarkMode = isDarkMode;
        _theme = IsDarkMode ? _darkTheme : _lightTheme;
    }
}