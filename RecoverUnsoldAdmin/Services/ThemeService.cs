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
            AppbarBackground = "#0097FF",
            AppbarText = "#FFFFFF",
            Primary = "#007CD1",
            TextPrimary = "#FFFFFF",
            Background = "#001524",
            TextSecondary = "#E2EEF6",
            DrawerBackground = "#093958",
            DrawerText = "#FFFFFF",
            Surface = "#093958",
            ActionDefault = "#0C1217",
            ActionDisabled = "#2F678C",
            TextDisabled = "#B0B0B0",
        }
    };

    private readonly MudTheme _lightTheme = new()
    {
        Palette = new Palette
        {
            AppbarBackground = "#0097FF",
            AppbarText = "#FFFFFF",
            Primary = "#007CD1",
            TextPrimary = "#0C1217",
            Background = "#F4FDFF",
            TextSecondary = "#0C1217",
            DrawerBackground = "#C5E5FF",
            DrawerText = "#0C1217",
            Surface = "#E4FAFF",
            ActionDefault = "#0C1217",
            ActionDisabled = "#2F678C",
            TextDisabled = "#676767",
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