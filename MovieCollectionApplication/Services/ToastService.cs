using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionApplication.Services
{
    public class ToastService
    {
        public event Action<string, string, string?, string?>? OnShow;

        public void ShowSuccess(string message, string? linkUrl = null, string? linkText = null)
        {
            OnShow?.Invoke(message, "success", linkUrl, linkText);
        }

        public void ShowError(string message, string? linkUrl = null, string? linkText = null)
        {
            OnShow?.Invoke(message, "danger", linkUrl, linkText);
        }

        public void ShowWarning(string message, string? linkUrl = null, string? linkText = null)
        {
            OnShow?.Invoke(message, "warning", linkUrl, linkText);
        }

        public void ShowInfo(string message, string? linkUrl = null, string? linkText = null)
        {
            OnShow?.Invoke(message, "info", linkUrl, linkText);
        }
    }
}