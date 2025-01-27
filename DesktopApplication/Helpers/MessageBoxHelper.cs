using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using ProPresenter7WEB.DesktopApplication.Properties;
using System;
using System.Collections.Generic;

namespace ProPresenter7WEB.DesktopApplication.Helpers
{
    public static class MessageBoxHelper
    {
        private const string ERROR_ICON = "avares://ProPresenter7WEB.DesktopApplication/Assets/error.ico";

        public static IMsBox<string> GetFailedConnectionMessageBox(string messageText)
        {
            return MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ContentTitle = MessageBoxResources.FailConnectionTitle,
                ContentMessage = messageText,
                WindowIcon = new WindowIcon(
                    new Bitmap(AssetLoader.Open(new Uri(ERROR_ICON)))),
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                SizeToContent = SizeToContent.Height,
                MinWidth = 300,
                MaxWidth = 800,
                ButtonDefinitions = new List<ButtonDefinition>
                {
                    new ButtonDefinition{ Name = MessageBoxResources.OkButtonText },
                },
            });
        }
    }
}
