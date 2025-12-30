using MudBlazor;

namespace Platform.Blazor.Services
{
    public class LayoutService
    {
        public MudTheme CurrentTheme { get; private set; } = new MudTheme();
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
