﻿using UI.Services.Factory;

namespace UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private IUIFactory _uiFactory;
        
        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.ShopWindow:
                    _uiFactory.CreateShop();
                    break;
                case WindowId.SettingsWindowController:
                    _uiFactory.CreateSettings();
                    break;
            }
        }
    }
}