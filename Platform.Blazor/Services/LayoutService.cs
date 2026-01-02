using MudBlazor;

namespace Platform.Blazor.Services
{
    public class LayoutService
    {
        public MudTheme CurrentTheme { get; private set; } = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#0D1E4C",
                AppbarBackground = "#0D1E4C"
            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#0D1E4C"
            }
        };
        public bool IsDarkMode { get; private set; }
        public event Action OnMajorUpdate;

        public void SetDarkMode(bool value)
        {
            IsDarkMode = value;
            OnMajorUpdate?.Invoke();
        }

        public void ToggleDarkMode()
        {
            SetDarkMode(!IsDarkMode);
        }
    }
}
