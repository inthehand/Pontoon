//-----------------------------------------------------------------------
// <copyright file="Clipboard.wpa81.cs" company="In The Hand Ltd">
//     Copyright © 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Windows
{
    internal static class Clipboard
    {
        /// <summary>
        /// For Windows XAML Phone Applications use a separate app to push items to the Clipboard
        /// </summary>
        /// <param name="text"></param>
        public static void SetText(string text)
        {
            global::Windows.System.LauncherOptions options = new global::Windows.System.LauncherOptions();
            options.PreferredApplicationDisplayName = "Clipboarder";
            options.PreferredApplicationPackageFamilyName = "InTheHandLtd.Clipboarder";
            options.DisplayApplicationPicker = false;
            global::Windows.System.Launcher.LaunchUriAsync(new Uri(string.Format("clipboard:Set?Text={0}", Uri.EscapeDataString(text))), options);
        }
    }
}