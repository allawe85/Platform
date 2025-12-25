using System;

namespace Platform.Blazor.Services
{
    public class LayoutService
    {
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
