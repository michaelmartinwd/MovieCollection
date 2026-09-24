using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionApplication.Services
{
    public class ToastService
    {
        public event Action<string>? OnShow;

        public void Show(string message)
        {
            OnShow?.Invoke(message);
        }
    }
}